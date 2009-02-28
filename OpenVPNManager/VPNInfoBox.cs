using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenVPN;
using System.Diagnostics.CodeAnalysis;

namespace OpenVPNManager
{
    /// <summary>
    /// this class represents a usercontrol whichs shows a
    /// little summary about a vpn
    /// </summary>

    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VPN")]
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
        public void Init()
        {
            // wrong thread? invoke!
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new initDelegate(Init));
                }
                catch (ObjectDisposedException)
                { }
                return;
            }

            // display name
            lblName.Text = m_config.Name;

            btnEdit.Enabled = !m_config.IsService;

            // if there is an error
            if (m_config.VPNConnection == null)
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
                m_config.VPNConnection.ConnectionStateChanged += new EventHandler(m_vpn_stateChanged);
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
                    this.BeginInvoke(new EventHandler(m_vpn_stateChanged), sender, e);
                }
                catch (ObjectDisposedException)
                { }
                catch (InvalidAsynchronousStateException)
                { }
                return; 
            }

            // display buttons and text as needed
            if(m_config.VPNConnection.State == VPNConnectionState.Initializing) {
                pbStatus.Image = Properties.Resources.STATE_Initializing;
                btnDisconnect.Enabled = false;
                btnConnect.Enabled = false;
                llIP.SetIP(null);
            } else if(m_config.VPNConnection.State == VPNConnectionState.Running) {
                pbStatus.Image = Properties.Resources.STATE_Running;
                btnDisconnect.Enabled = true;
                btnConnect.Enabled = false;
                llIP.SetIP(m_config.VPNConnection.IP);
            } else if(m_config.VPNConnection.State == VPNConnectionState.Stopped) {
                pbStatus.Image = Properties.Resources.STATE_Stopped;
                btnDisconnect.Enabled = false;
                btnConnect.Enabled = true;
                llIP.SetIP(null);
            } else if(m_config.VPNConnection.State == VPNConnectionState.Stopping) {
                pbStatus.Image = Properties.Resources.STATE_Stopping;
                btnDisconnect.Enabled = false;
                btnConnect.Enabled = false;
                llIP.SetIP(null);
            } else if(m_config.VPNConnection.State == VPNConnectionState.Error) {
                pbStatus.Image = Properties.Resources.STATE_Error;
                btnDisconnect.Enabled = false;
                btnConnect.Enabled = true;
                llIP.SetIP(null);
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
            m_config.Connect();
        }

        /// <summary>
        /// disconnect button was pressed <br />
        /// just disconnect via parent config
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            m_config.Disconnect();
        }

        /// <summary>
        /// show button was clicked <br />
        /// just show details via parrend
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnShow_Click(object sender, EventArgs e)
        {
            m_config.ShowStatus();
        }

        /// <summary>
        /// edit button was pressed <br />
        /// just edit via parent config
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            m_config.Edit();
        }

        /// <summary>
        /// error label was pressed <br />
        /// just show information via parrent
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void llReadError_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            m_config.ShowErrors();
        }
    }
}
