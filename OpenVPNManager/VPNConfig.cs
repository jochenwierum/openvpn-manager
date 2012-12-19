using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows.Forms;
using OpenVPNUtils;
using OpenVPNUtils.States;
using System.Reflection;

namespace OpenVPNManager
{
    /// <summary>
    /// represents a single vpn config,
    /// initializes and controls OVPN using the config it represents
    /// </summary
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VPN")]
    public class VPNConfig : IDisposable
    {
        #region variables

        /// <summary>
        /// the menu item which holds all other menu items to interact with the vpn
        /// </summary>
        private ToolStripMenuItem m_menu;

        /// <summary>
        /// shows the status
        /// </summary>
        private ToolStripMenuItem m_menu_show;

        /// <summary>
        /// connects to vpn
        /// </summary>
        private ToolStripMenuItem m_menu_connect;

        /// <summary>
        /// disconnects from vpn
        /// </summary>
        private ToolStripMenuItem m_menu_disconnect;

        /// <summary>
        /// shows information about the error
        /// </summary>
        private ToolStripMenuItem m_menu_error;

        /// <summary>
        /// edits the configuration
        /// </summary>
        private ToolStripMenuItem m_menu_edit;

        /// <summary>
        /// holds the infobox control for this vpn
        /// </summary>
        private VPNInfoBox m_infobox;

        /// <summary>
        /// holds the status form for this vpn
        /// </summary>
        private FrmStatus m_status;

        /// <summary>
        /// holds a parent for the menu items<br />
        /// this is needed for invokes
        /// </summary>
        private FrmGlobalStatus m_parent;

        /// <summary>
        /// the vpn itself
        /// </summary>
        private Connection m_vpn;

        /// <summary>
        /// the error message of the new OVPN() call<br />
        /// null means, there was no error
        /// </summary>
        private string m_error_message;

        /// <summary>
        /// the config file which OpenVPN will use
        /// </summary>
        private string m_file;

        /// <summary>
        /// the path to the openvpn binary
        /// </summary>
        private string m_bin;

        /// <summary>
        /// the verbose level of the OVPN debug log
        /// </summary>
        private int m_dbglevel;

        /// <summary>
        /// used to disconnect when we are in an event
        /// </summary>
        private System.Timers.Timer m_disconnectTimer;

        /// <summary>
        /// Do we start openvpn by ourselves, or are we using a system service?
        /// </summary>
        private bool m_isService;

        private FrmPasswd m_frmpw;

        private FrmLoginAndPasswd m_frmlpw;

        private FrmSelectPKCS11Key m_frmkey;

        #endregion

        /// <summary>
        /// Constructs a new object.
        /// Loads the configuration and prepares a connection.
        /// A already startet VPN service will be used.
        /// </summary>
        /// <param name="bin">path to the openvpn executable</param>
        /// <param name="file">path to the configuration of this openvpn</param>
        /// <param name="dbglevel">the debug level for internal logs</param>
        /// <param name="smartCardSupport">enable smartCard support</param>
        /// <param name="parent">the parent of the menu</param>
        /// <seealso cref="init" />
        static public VPNConfig CreateServiceConnection(string file,
            int dbglevel, bool smartCardSupport, FrmGlobalStatus parent)
        {
            VPNConfig vc = new VPNConfig();
            vc.m_file = file;
            vc.m_parent = parent;
            vc.m_dbglevel = dbglevel;
            vc.m_isService = true;
            vc.m_smartCard = smartCardSupport;
            vc.init();
            return vc;
        }

        /// <summary>
        /// Constructs a new object.
        /// Loads the configuration and prepares a connection.
        /// A userspace VPN connection will be used.
        /// </summary>
        /// <param name="bin">path to the openvpn executable</param>
        /// <param name="file">path to the configuration of this openvpn</param>
        /// <param name="dbglevel">the debug level for internal logs</param>
        /// <param name="smartCardSupport">enable smartCard support</param>
        /// <param name="parent">the parent of the menu</param>
        /// <seealso cref="init" />
        static public VPNConfig CreateUserspaceConnection(string bin,
            string file, int dbglevel, bool smartCardSupport, FrmGlobalStatus parent)
        {
            VPNConfig vc = new VPNConfig();
            vc.m_file = file;
            vc.m_bin = bin;
            vc.m_parent = parent;
            vc.m_dbglevel = dbglevel;
            vc.m_isService = false;
            vc.m_smartCard = smartCardSupport;
            vc.init();
            return vc;
        }

        /// <summary>
        /// Creates a desciptive (file-)name for showing to the user which connection is used, and if it is a service or not.
        /// Also used for the logfile names for the OpenVPNManager Service client processes.
        /// </summary>
        /// <param name="m_file"></param>
        /// <returns>A descriptive (file-)name</returns>
        public static String GetDescriptiveName(String m_file)
        {
            string dir = Path.GetDirectoryName(m_file);
            FileInfo fi = new FileInfo(m_file);

            String parentFolder = "";
            if (dir.StartsWith(Properties.Settings.Default.vpnconf))
                parentFolder = Properties.Settings.Default.vpnconf;

            if (dir.StartsWith(UtilsHelper.FixedConfigDir))
                parentFolder = UtilsHelper.FixedConfigDir;

            // if we have a subdirectory, extract it and add its name in brackets
            if (!String.IsNullOrEmpty(parentFolder) && dir.Length > parentFolder.Length)
                return fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) +
                    " (" + Path.GetDirectoryName(m_file).Substring(parentFolder.Length + 1) + ")";
            // nosubdirectory, show just the filename
            else
                return fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
        }

        #region constructor
        [SuppressMessage("Microsoft.Mobility", "CA1601:DoNotUseTimersThatPreventPowerStateChanges")]
        private VPNConfig()
        {
            m_menu = new ToolStripMenuItem();
            m_infobox = new VPNInfoBox(this);
            m_status = new FrmStatus(this);

            m_disconnectTimer = new System.Timers.Timer(100);
            m_disconnectTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_disconnectTimer_Elapsed);
        }
        #endregion

        #region properties
        /// <summary>
        /// provides the connection
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VPN")]
        public Connection VPNConnection
        {
            get { return m_vpn; }
        }

        /// <summary>
        /// the name of this configuration
        /// </summary>
        //HACK: I think, this is crazy
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// return the menu which holds all the submenu entries
        /// </summary>
        public ToolStripMenuItem Menuitem
        {
            get { return m_menu; }
        }

        /// <summary>
        /// reutrn the infobox
        /// </summary>
        public VPNInfoBox InfoBox
        {
            get { return m_infobox; }
        }

        /// <summary>
        /// indicates whether this configuration uses a windows service
        /// </summary>
        public bool IsService
        {
            get { return m_isService; }
        }
        #endregion

        /// <summary>
        /// (re)initialize all controls and data<br />
        /// this is needed if the configuration has changed
        /// </summary>
        private void init()
        {
            if (m_parent.InvokeRequired)
            {
                try
                {
                    m_parent.Invoke(new UtilsHelper.Action(init));
                }
                catch (ObjectDisposedException)
                {
                }
                return;
            }

            m_vpn = null;
            m_menu.DropDownItems.Clear();
            m_status.Hide();

            try
            {
                if (!m_isService)
                {
                    m_vpn = new UserSpaceConnection(m_bin, m_file,
                        new EventHandler<LogEventArgs>(addLog),
                        m_dbglevel, m_smartCard);
                }
                else
                {
                    m_vpn = new ServiceConnection(m_file,
                        new EventHandler<LogEventArgs>(addLog),
                        m_dbglevel, m_smartCard);
                }
            }
            catch (ApplicationException e)
            {
                m_error_message = e.Message;
            }

            Name = VPNConfig.GetDescriptiveName(m_file);
            if (m_isService)
                Name += " (" + Program.res.GetString("DIALOG_Service") + ")";

            m_menu.Text = Name;
            m_infobox.Init();

            if (m_error_message != null)
            {
                m_menu_error = new ToolStripMenuItem(Program.res.GetString("TRAY_Error_Information"));
                m_menu_error.Click += new EventHandler(m_menu_error_Click);
                m_menu.DropDownItems.Add(m_menu_error);

                return;
            }

            m_vpn.Logs.DebugLevel = m_dbglevel;
            m_vpn.State.StateChanged += new EventHandler<StateChangedEventArgs>(State_StateChanged);
            m_vpn.NeedCardId += new EventHandler<NeedCardIdEventArgs>(m_vpn_needCardID);
            m_vpn.NeedPassword += new EventHandler<NeedPasswordEventArgs>(m_vpn_needPassword);
            m_vpn.NeedLoginAndPassword += new EventHandler<NeedLoginAndPasswordEventArgs>(m_vpn_needLoginAndPassword);

            m_status.Init();

            m_menu_connect = new ToolStripMenuItem(Program.res.GetString("TRAY_Connect"));
            m_menu_connect.Image = Properties.Resources.BUTTON_Connect;
            m_menu_connect.Click += new EventHandler(m_menu_connect_Click);
            m_menu.DropDownItems.Add(m_menu_connect);

            m_menu_disconnect = new ToolStripMenuItem(Program.res.GetString("TRAY_Disconnect"));
            m_menu_disconnect.Image = Properties.Resources.BUTTON_Disconnect;
            m_menu_disconnect.Click += new EventHandler(m_menu_disconnect_Click);
            m_menu_disconnect.Visible = false;
            m_menu.DropDownItems.Add(m_menu_disconnect);

            m_menu_show = new ToolStripMenuItem(Program.res.GetString("TRAY_Show"));
            m_menu_show.Image = Properties.Resources.BUTTON_Details;
            m_menu_show.Click += new EventHandler(m_menu_show_Click);
            m_menu.DropDownItems.Add(m_menu_show);

            m_menu_edit = new ToolStripMenuItem(Program.res.GetString("TRAY_Edit"));
            m_menu_edit.Enabled = !m_isService;
            m_menu_edit.Image = Properties.Resources.BUTTON_Edit;
            m_menu_edit.Click += new EventHandler(m_menu_edit_Click);
            m_menu.DropDownItems.Add(m_menu_edit);

            m_menu.Image = Properties.Resources.STATE_Stopped;
        }

        /// <summary>
        /// OVPN changes it status.
        /// Show or hide elements.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">the new state</param>
        /// <seealso cref="stateChanged"/>
        void State_StateChanged(object sender, StateChangedEventArgs e)
        {
            try
            {
                if (m_parent.InvokeRequired)
                {
                    m_parent.BeginInvoke(
                        new EventHandler<StateChangedEventArgs>(
                            State_StateChanged),sender, e);
                    return;
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            switch (e.NewState.ConnectionState)
            {
                case VPNConnectionState.Initializing:
                    m_menu_disconnect.Visible = true;
                    m_menu_connect.Visible = false;
                    m_menu.Image = Properties.Resources.STATE_Initializing;
                    break;
                case VPNConnectionState.Running:

                    m_menu_disconnect.Visible = true;
                    m_menu_connect.Visible = false;
                    m_menu.Image = Properties.Resources.STATE_Running;

                    // show assigned ip if possible
                    string text = Program.res.GetString("STATE_Connected");
                    if (m_vpn.IP != null)
                        text += Environment.NewLine +
                            "IP: " + m_vpn.IP;

                    m_parent.ShowPopup(Name, text);
                    break;
                case VPNConnectionState.Stopped:
                    m_menu_disconnect.Visible = false;
                    m_menu_connect.Visible = true;
                    m_menu.Image = Properties.Resources.STATE_Stopped;
                    break;
                case VPNConnectionState.Stopping:
                    m_menu_disconnect.Visible = false;
                    m_menu_connect.Visible = false;
                    m_menu.Image = Properties.Resources.STATE_Stopping;
                    break;
                case VPNConnectionState.Error:
                default:
                    m_menu_disconnect.Visible = false;
                    m_menu_connect.Visible = true;
                    m_menu.Image = Properties.Resources.STATE_Error;

                    if (m_vpn.LogFile != null)
                    {
                        if (RTLMessageBox.Show(m_status,
                            Program.res.GetString("BOX_VPN_Error"),
                            MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2,
                            MessageBoxIcon.Error) == DialogResult.Yes)
                        {
                            ProcessStartInfo pi = new ProcessStartInfo();
                            pi.Arguments = "\"" + m_vpn.LogFile + "\"";
                            pi.ErrorDialog = true;
                            pi.FileName = "notepad.exe";
                            pi.UseShellExecute = true;


                            Process.Start(pi);
                        }
                    }
                    else
                    {
                        RTLMessageBox.Show(m_status,
                            Program.res.GetString("BOX_VPNS_Error"),
                            MessageBoxButtons.OK, MessageBoxDefaultButton.Button2,
                            MessageBoxIcon.Error);
                    }
                    break;
            }

            m_parent.SetTrayIconAndPopupText();
        }

        /// <summary>
        /// adds a log message to the log window
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">contains the message</param>
        private void addLog(Object sender, LogEventArgs e)
        {
            if(m_status != null)
                m_status.AddLog(e.MessageType, e.Message);
        }

        /// <summary>
        /// shows the status window
        /// </summary>
        public void ShowStatus()
        {
            m_status.Show();
        }

        /// <summary>
        /// Show error detail in a message box
        /// </summary>
        public void ShowErrors()
        {
            RTLMessageBox.Show(m_parent,
                Program.res.GetString("BOX_Error_Information") +
                Environment.NewLine + m_error_message,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// connect to the VPN <br />
        /// show message box on error
        /// </summary>
        public void Connect()
        {
            try
            {
                m_vpn.Connect();
            }
            catch (InvalidOperationException e)
            {
                /* 
                 * TODO it would be nicer if the message would hold less detail
                 * about the problem
                 */
                RTLMessageBox.Show(m_parent,
                    Program.res.GetString("BOX_Error_Connect") +
                    Environment.NewLine + e.Message,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// disconnect from vpn <br />
        /// leave status window open
        /// </summary>
        public void Disconnect()
        {
            Disconnect(false);
        }

        /// <summary>
        /// disconnect from vpn
        /// </summary>
        /// <param name="closeForm">
        ///     close status window, this is important if you
        ///     change the configuration, because the status window will
        ///     still use the old configuration
        /// </param>
        public void Disconnect(bool closeForm)
        {
            // disconnect if needed
            if (m_vpn != null)
                try
                {
                    m_vpn.Disconnect();
                }
                catch (InvalidOperationException)
                {
                }

            if (m_frmpw != null)
                m_frmpw.Invoke(new UtilsHelper.Action<Form>(closeSubForm), m_frmpw);

            if (m_frmkey != null)
                m_frmkey.Invoke(new UtilsHelper.Action<Form>(closeSubForm), m_frmkey);

            if (m_frmlpw != null)
                m_frmlpw.Invoke(new UtilsHelper.Action<Form>(closeSubForm), m_frmlpw);

            // close the window if needed
            if (closeForm && m_status != null)
                m_status.Close();
        }

        private void closeSubForm(Form f) { f.Close(); }


        /// <summary>
        /// edit a configuration <br />
        /// this method simply starts notepad and opens the configuration file
        /// </summary>
        public void Edit()
        {
            ProcessStartInfo pi = new ProcessStartInfo();
            pi.Arguments = "\"" + m_file + "\"";
            pi.ErrorDialog = true;
            pi.FileName = "notepad.exe";
            pi.UseShellExecute = true;

            Process p = new Process();
            p.StartInfo = pi;
            p.EnableRaisingEvents = true;
            p.Exited += new EventHandler(p_Exited);

            p.Start();

        }

        /// <summary>
        /// close the connection after timespan has elapsed
        /// </summary>
        /// <param name="sender">timer which raised the evend</param>
        /// <param name="e">ignored</param>
        void m_disconnectTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ((System.Timers.Timer)sender).Stop();
            Disconnect();
        }

        /// <summary>
        /// notepad exited <br />
        /// reload configuration
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void p_Exited(object sender, EventArgs e)
        {
            // was a connection established?
            bool wasConnected = false;
            VPNConnectionState state = m_vpn.State.CreateSnapshot().ConnectionState;
            if (m_vpn != null)
                wasConnected = state == VPNConnectionState.Initializing ||
                    state == VPNConnectionState.Running;

            // close the connection if needed, reload the configuration
            Disconnect();
            init();

            // reconnect, if the user wants it
            if (wasConnected && m_error_message == null)
                if (RTLMessageBox.Show(m_parent,
                    Program.res.GetString("BOX_Reconnect"),
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                    Connect();
        }

        /// <summary>
        /// OVPN requests a password <br />
        /// generates and shows a form, answers via e
        /// </summary>
        /// <param name="sender">OVPN which requests the password</param>
        /// <param name="e">Information, what is needed</param>
        private void m_vpn_needPassword(object sender, NeedPasswordEventArgs e)
        {
            m_frmpw = new FrmPasswd();
            e.Password = m_frmpw.AskPass(e.PasswordType, Name);

            // if no password was entered, disconnect
            if (e.Password == null &&
                VPNConnection.State.CreateSnapshot().ConnectionState
                == VPNConnectionState.Initializing)
            {
                m_disconnectTimer.Start();
            }
            m_frmpw = null;
        }

        /// <summary>
        /// OVPN requests a username and password <br />
        /// generates and shows a form, answers via e
        /// </summary>
        /// <param name="sender">OVPN which requests the username and password</param>
        /// <param name="e">Information, what is needed</param>
        private void m_vpn_needLoginAndPassword(object sender, NeedLoginAndPasswordEventArgs e)
        {
            m_frmlpw = new FrmLoginAndPasswd();
            string[] loginfo = null;
            loginfo = m_frmlpw.AskLoginAndPass(e.PasswordType, Name);
            if (loginfo != null)
            {
                e.UserName = loginfo[0];
                e.Password = loginfo[1];
            }

            // if no password was entered, disconnect
            if ((e.Password == null || e.UserName == null) &&
                VPNConnection.State.CreateSnapshot().ConnectionState ==
                VPNConnectionState.Initializing)
            {
                m_disconnectTimer.Start();
            }

            m_frmlpw = null;
        }

        /// <summary>
        /// OVPN requests a SmardCard id <br />
        /// generates and shows a form, answers via e
        /// </summary>
        /// <param name="sender">OVPN which requests the id</param>
        /// <param name="e">Information, what was found</param>
        private void m_vpn_needCardID(object sender, NeedCardIdEventArgs e)
        {
            // if there is no id
            if (e.CardDetails.Count == 0)
            {
                if (RTLMessageBox.Show(m_parent,
                    Program.res.GetString("BOX_NoKey"),
                    MessageBoxButtons.RetryCancel,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxIcon.Warning) == DialogResult.Retry)

                    e.SelectedId = NeedCardIdEventArgs.Retry;
                else
                {
                    e.SelectedId = NeedCardIdEventArgs.None;
                    m_disconnectTimer.Start();

                }
            }

            // if there is only one id, use it
            else if (e.CardDetails.Count == 1)
                e.SelectedId = e.CardDetails[0].Number;
            else
            {
                // request key
                m_frmkey = new FrmSelectPKCS11Key();
                int res = m_frmkey.SelectKey(e.CardDetails, this.Name);
                if (res == -1)
                {
                    e.SelectedId = NeedCardIdEventArgs.None;
                    if (VPNConnection.State.CreateSnapshot().ConnectionState
                        == VPNConnectionState.Initializing)
                    {
                        m_disconnectTimer.Start();
                    }
                }
                else if (res == -2)
                    e.SelectedId = NeedCardIdEventArgs.Retry;
                else
                    e.SelectedId = res;
                m_frmkey = null;
            }
        }

        /// <summary>
        /// "Error Information" was selected.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        void m_menu_error_Click(object sender, EventArgs e)
        {
            ShowErrors();
        }

        /// <summary>
        /// edit was selected in the context menu
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        void m_menu_edit_Click(object sender, EventArgs e)
        {
            Edit();
        }

        /// <summary>
        /// disconnect was selected in the context menu
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void m_menu_disconnect_Click(object sender, EventArgs e)
        {
            VPNConnectionState state = m_vpn.State.CreateSnapshot().ConnectionState;
            if (state == VPNConnectionState.Initializing ||
                state == VPNConnectionState.Running)

                Disconnect();
        }

        /// <summary>
        /// connect was selected in the context menu
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void m_menu_connect_Click(object sender, EventArgs e)
        {
            // connect only, if we are disconnected
            StateSnapshot ss = m_vpn.State.CreateSnapshot();
            if (ss.ConnectionState == VPNConnectionState.Stopped ||
                ss.ConnectionState == VPNConnectionState.Error)
                Connect();
        }

        /// <summary>
        /// show was selected in the context menu
        /// </summary>
        /// <param name="sender">ignore</param>
        /// <param name="e">ignore</param>
        private void m_menu_show_Click(object sender, EventArgs e)
        {
            // show status window
            ShowStatus();
        }

        /// <summary>
        /// Determines, whether this configuration is connected.
        /// </summary>
        public bool Running
        {
            get
            {
                if (m_vpn != null)
                {
                    var state = m_vpn.State.CreateSnapshot();
                    if (state.ConnectionState == VPNConnectionState.Stopped
                        || state.ConnectionState == VPNConnectionState.Error)
                        return false;
                    return true;
                } else
                    return false;
            }
        }

        #region IDisposable Members

        private bool disposed;
        private bool m_smartCard;
        ~VPNConfig()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    m_disconnectTimer.Dispose();
                    m_frmkey.Dispose();
                    m_frmlpw.Dispose();
                    m_frmpw.Dispose();
                    m_infobox.Dispose();
                    m_menu.Dispose();
                    m_menu_connect.Dispose();
                    m_menu_disconnect.Dispose();
                    m_menu_edit.Dispose();
                    m_menu_error.Dispose();
                    m_menu_show.Dispose();
                    m_parent.Dispose();
                    m_status.Dispose();
                    m_vpn.Dispose();
                }

                m_vpn = null;
                m_status = null;
                m_parent = null;
                m_menu_show = null;
                m_menu_error = null;
                m_menu_edit = null;
                m_menu_disconnect = null;
                m_menu_connect = null;
                m_menu = null;
                m_infobox = null;
                m_frmpw = null;
                m_frmlpw = null;
                m_frmkey = null;
                m_disconnectTimer = null;

                disposed = true;
            }
        }

        #endregion
    }
}
