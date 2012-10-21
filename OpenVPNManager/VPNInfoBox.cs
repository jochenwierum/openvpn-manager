using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using OpenVPN.States;

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
                    this.Invoke(new helper.Action(Init));
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
                btnShow.Visible = false;
                pbStatus.Visible = false;
                llReadError.Visible = true;
            }
            else
            {
                // display buttons
                btnConnect.Visible = true;
                btnShow.Visible = true;
                pbStatus.Visible = true;
                llReadError.Visible = false;

                // react on state changes
                m_config.VPNConnection.State.StateChanged += 
                    new EventHandler<StateChangedEventArgs>(State_StateChanged);
                setState(m_config.VPNConnection.State.CreateSnapshot());
            }
        }

        /// <summary>
        /// the connection state changed
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">the new state</param>
        private void State_StateChanged(object sender, StateChangedEventArgs e)
        {
            // wrong thred? revoke!
            if (this.InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(new helper.Action<StateSnapshot>(setState),
                        e.NewState);
                }
                catch (ObjectDisposedException) { }
                catch (InvalidAsynchronousStateException) { }
                return;
            }
            else
            {
                setState(e.NewState);
            }
        }

        /// <summary>
        /// display buttons and text as needed
        /// </summary>
        /// <param name="ss">new State</param>
        private void setState(StateSnapshot ss) {
            bool showConnect = true;// If false then show Disconnect
            bool showConnectEnable = true;
            String ip = null;
            switch (ss.ConnectionState)
            {
                case VPNConnectionState.Initializing:
                    pbStatus.Image = Properties.Resources.STATE_Initializing;
                    showConnect = false;
                    break;
                case VPNConnectionState.Running:
                    pbStatus.Image = Properties.Resources.STATE_Running;
                    showConnect = false;
                    ip = m_config.VPNConnection.IP;
                    break;
                case VPNConnectionState.Stopped:
                    pbStatus.Image = Properties.Resources.STATE_Stopped;
                    break;
                case VPNConnectionState.Stopping:
                    showConnectEnable = false;
                    pbStatus.Image = Properties.Resources.STATE_Stopping;
                    break;
                case VPNConnectionState.Error:
                default:
                    pbStatus.Image = Properties.Resources.STATE_Error;
                    break;
            }
            if (showConnect)
            {
                btnConnect.Image = Properties.Resources.BUTTON_Connect;
                toolTip.SetToolTip(btnConnect, Program.res.GetString("QUICKINFO_Connect"));
            }
            else
            {
                toolTip.SetToolTip(btnConnect, Program.res.GetString("QUICKINFO_Disconnect"));
                btnConnect.Image = Properties.Resources.BUTTON_Disconnect;
            }
            btnConnect.Enabled = showConnectEnable;
            llIP.SetIP(ip);         
        }

        /// <summary>
        /// connect button was pressed <br />
        /// just connect via parent config
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            VPNConnectionState state =
                m_config.VPNConnection.State.CreateSnapshot().ConnectionState;

            // connect only if we are disconnected, clear the list
            if (state == VPNConnectionState.Stopped ||
                state == VPNConnectionState.Error)
            {
                //lstLog.Items.Clear();
                m_config.Connect();
            }

            // disconnect only if we are connected
            else if (state == VPNConnectionState.Initializing ||
                state == VPNConnectionState.Running)
            {
                m_config.Disconnect();
            }
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
