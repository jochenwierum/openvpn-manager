using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenVPN;

namespace OpenVPNManager
{
    /// <summary>
    /// this class represents a usercontrol whichs shows a
    /// little summary about a vpn
    /// </summary>
    public partial class VPNInfoBox : UserControl
    {
        #region variables
        /// <summary>
        /// parent to which this control belongs
        /// </summary>
        private VPNConfig m_config;
        #endregion

        #region constructor
        /// <summary>
        /// Creats a usercontrol
        /// </summary>
        /// <param name="config">parent which holgs this control</param>
        /// <seealso cref="init"/>
        public VPNInfoBox(VPNConfig config)
        {
            InitializeComponent();
            m_config = config;
        }
        #endregion

        /// <summary>
        /// delegate for init
        /// </summary>
        private delegate void initDelegate();

        /// <summary>
        /// (re)initialize the control <br />
        /// this is necessary if a new OVPN object is created
        /// </summary>
        public void init()
        {
            // wrong thread? invoke!
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new initDelegate(init));
                }
                catch (ObjectDisposedException)
                { }
                return;
            }

            // display name
            lblName.Text = m_config.name;

            // if there is an error
            if (m_config.vpn == null)
            {
                // display no buttons
                btnConnect.Visible = false;
                btnDisconnect.Visible = false;
                btnShow.Visible = false;
                pbStatus.Visible = false;
                llReadError.Visible = true;
            }
            else
            {
                // display buttons
                btnConnect.Visible = true;
                btnDisconnect.Visible = true;
                btnShow.Visible = true;
                pbStatus.Visible = true;
                llReadError.Visible = false;

                // react on state changes
                m_config.vpn.stateChanged += new EventHandler(m_vpn_stateChanged);
                m_vpn_stateChanged(null, null);
            }
        }

        /// <summary>
        /// the connection state changed
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void m_vpn_stateChanged(object sender, EventArgs e)
        {
            // wrong thred? revoke!
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new EventHandler(m_vpn_stateChanged), sender, e);
                }
                catch (ObjectDisposedException)
                { }
                catch (InvalidAsynchronousStateException)
                { }
                return; 
            }

            // display buttons and text as needed
            if(m_config.vpn.state == OVPNConnection.OVPNState.INITIALIZING) {
                pbStatus.Image = Properties.Resources.STATE_Initializing;
                btnDisconnect.Enabled = false;
                btnConnect.Enabled = false;
                llIP.setIP(null);
            } else if(m_config.vpn.state == OVPNConnection.OVPNState.RUNNING) {
                pbStatus.Image = Properties.Resources.STATE_Running;
                btnDisconnect.Enabled = true;
                btnConnect.Enabled = false;
                llIP.setIP(m_config.vpn.ip);
            } else if(m_config.vpn.state == OVPNConnection.OVPNState.STOPPED) {
                pbStatus.Image = Properties.Resources.STATE_Stopped;
                btnDisconnect.Enabled = false;
                btnConnect.Enabled = true;
                llIP.setIP(null);
            } else if(m_config.vpn.state == OVPNConnection.OVPNState.STOPPING) {
                pbStatus.Image = Properties.Resources.STATE_Stopping;
                btnDisconnect.Enabled = false;
                btnConnect.Enabled = false;
                llIP.setIP(null);
            } else if(m_config.vpn.state == OVPNConnection.OVPNState.ERROR) {
                pbStatus.Image = Properties.Resources.STATE_Error;
                btnDisconnect.Enabled = false;
                btnConnect.Enabled = true;
                llIP.setIP(null);
            }
        }

        /// <summary>
        /// connect button was pressed <br />
        /// just connect via parent config
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            m_config.connect();
        }

        /// <summary>
        /// disconnect button was pressed <br />
        /// just disconnect via parent config
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            m_config.disconnect();
        }

        /// <summary>
        /// show button was clicked <br />
        /// just show details via parrend
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnShow_Click(object sender, EventArgs e)
        {
            m_config.showStatus();
        }

        /// <summary>
        /// edit button was pressed <br />
        /// just edit via parent config
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            m_config.edit();
        }

        /// <summary>
        /// error label was pressed <br />
        /// just show information via parrent
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void llReadError_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            m_config.showErrors();
        }
    }
}
