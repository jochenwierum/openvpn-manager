using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using OpenVPN;

namespace OpenVPNManager
{
    /// <summary>
    /// represents a single vpn config,
    /// initializes and controls OVPN using the config it represents
    /// </summary>
    public class VPNConfig
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
        private frmStatus m_status;

        /// <summary>
        /// holds a parent for the menu items<br />
        /// this is needed for invokes
        /// </summary>
        private frmGlobalStatus m_parent;

        /// <summary>
        /// the vpn itself
        /// </summary>
        private OVPNConnection m_vpn;
        
        /// <summary>
        /// the error message of the new OVPN() call<br />
        /// null means, there was no error
        /// </summary>
        private string m_error_message = null;

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
        /// File to store openvpn logs in
        /// </summary>
        private String m_tempLog = Path.GetTempFileName();

        /// <summary>
        /// Do we start openvpn by ourselves, or are we using a system service?
        /// </summary>
        private bool m_isService;

        private frmPasswd m_frmpw = null;

        private frmLoginAndPasswd m_frmlpw = null;

        private frmSelectPKCS11Key m_frmkey = null;
        
        #endregion

        /// <summary>
        /// Constructs a new object.
        /// Loads the configuration and prepares a connection.
        /// A already startet VPN service will be used.
        /// </summary>
        /// <param name="bin">path to the openvpn executable</param>
        /// <param name="file">path to the configuration of this openvpn</param>
        /// <param name="dbglevel">the debug level for internal logs</param>
        /// <param name="parent">the parent of the menu</param>
        /// <seealso cref="init" />
        static public VPNConfig createServiceConnection(string file, 
            int dbglevel, frmGlobalStatus parent)
        {
            VPNConfig vc = new VPNConfig();
            vc.m_file = file;
            vc.m_parent = parent;
            vc.m_dbglevel = dbglevel;
            vc.m_isService = true;
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
        /// <param name="parent">the parent of the menu</param>
        /// <seealso cref="init" />
        static public VPNConfig createUserspaceConnection(string bin, 
            string file, int dbglevel, frmGlobalStatus parent)
        {
            VPNConfig vc = new VPNConfig();
            vc.m_file = file;
            vc.m_bin = bin;
            vc.m_parent = parent;
            vc.m_dbglevel = dbglevel;
            vc.m_isService = false;
            vc.init();
            return vc;
        }

        #region constructor
        private VPNConfig()
        {
            m_menu = new ToolStripMenuItem();
            m_infobox = new VPNInfoBox(this);
            m_status = new frmStatus(this);

            m_disconnectTimer = new System.Timers.Timer(100);
            m_disconnectTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_disconnectTimer_Elapsed);
        }

        ~VPNConfig()
        {
            try
            {
                File.Delete(m_tempLog);
            }
            catch (IOException)
            {
            }
        }
        #endregion

        #region properties
        /// <summary>
        /// provides the connection
        /// </summary>
        public OVPNConnection vpn
        {
            get { return m_vpn; }
        }

        /// <summary>
        /// the name of this configuration
        /// </summary>
        public string name
        {
            get
            {
                string filename = Path.GetFileName(m_file);
                string dir = Path.GetDirectoryName(m_file);

                // if we have a subdirectory, extract it and add its name in brackets
                if (dir.Length > Properties.Settings.Default.vpnconf.Length)
                    return filename.Substring(0, filename.Length - 5) + " (" +
                        Path.GetDirectoryName(m_file).Substring(
                        Properties.Settings.Default.vpnconf.Length + 1) + ")";

                // nosubdirectory, show just the filename
                else
                    return filename.Substring(0, filename.Length - 5);
            }
        }

        /// <summary>
        /// return the menu which holds all the submenu entries
        /// </summary>
        public ToolStripMenuItem menuitem
        {
            get { return m_menu; }
        }

        /// <summary>
        /// reutrn the infobox
        /// </summary>
        public VPNInfoBox infoBox
        {
            get { return m_infobox; }
        }
        #endregion

        /// <summary>
        /// delegate if init needs to be invoked by another thread
        /// </summary>
        private delegate void initDelegate();

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
                    m_parent.Invoke(new initDelegate(init));
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
                    m_vpn = new OVPNUserConnection(m_bin, m_file, m_tempLog,
                        new OVPNLogManager.LogEventDelegate(m_status.logs_LogEvent),
                        m_dbglevel);
                }
                else
                {
                    m_vpn = new OVPNServiceConnection(m_file,
                        new OVPNLogManager.LogEventDelegate(m_status.logs_LogEvent),
                        m_dbglevel);
                }
            }
            catch (ApplicationException e)
            {
                m_error_message = e.Message;
            }

            m_menu.Text = name;
            m_infobox.init();

            if (m_error_message != null)
            {
                m_menu_error = new ToolStripMenuItem(Program.res.GetString("TRAY_Error_Information"));
                m_menu_error.Click +=new EventHandler(m_menu_error_Click);
                m_menu.DropDownItems.Add(m_menu_error);

                return;
            }

            m_vpn.logs.debugLevel = m_dbglevel;
            m_vpn.stateChanged += new EventHandler(m_vpn_stateChanged);
            m_vpn.needCardID += new OVPNConnection.NeedCardIDEventDelegate(m_vpn_needCardID);
            m_vpn.needPassword += new OVPNConnection.NeedPasswordEventDelegate(m_vpn_needPassword);
            m_vpn.needLoginAndPassword += new OVPNConnection.NeedLoginAndPasswordEventDelegate(m_vpn_needLoginAndPassword);

            m_status.init();

            m_menu_show = new ToolStripMenuItem(Program.res.GetString("TRAY_Show"));
            m_menu_show.Image = Properties.Resources.BUTTON_Details;
            m_menu_show.Click += new EventHandler(m_menu_show_Click);
            m_menu.DropDownItems.Add(m_menu_show);

            m_menu_connect = new ToolStripMenuItem(Program.res.GetString("TRAY_Connect"));
            m_menu_connect.Image = Properties.Resources.BUTTON_Connect;
            m_menu_connect.Click += new EventHandler(m_menu_connect_Click);
            m_menu.DropDownItems.Add(m_menu_connect);

            m_menu_disconnect = new ToolStripMenuItem(Program.res.GetString("TRAY_Disconnect"));
            m_menu_disconnect.Image = Properties.Resources.BUTTON_Disconnect;
            m_menu_disconnect.Click += new EventHandler(m_menu_disconnect_Click);
            m_menu_disconnect.Visible = false;
            m_menu.DropDownItems.Add(m_menu_disconnect);

            m_menu_edit = new ToolStripMenuItem(Program.res.GetString("TRAY_Edit"));
            m_menu_edit.Image = Properties.Resources.BUTTON_Edit;
            m_menu_edit.Click += new EventHandler(m_menu_edit_Click);
            m_menu.DropDownItems.Add(m_menu_edit);

            m_menu.Image = Properties.Resources.STATE_Stopped;
        }

        /// <summary>
        /// shows the status window
        /// </summary>
        public void showStatus()
        {
            m_status.Show();
        }

        /// <summary>
        /// Show error detail in a message box
        /// </summary>
        public void showErrors()
        {
            MessageBox.Show(Program.res.GetString("BOX_Error_Information") +
                Environment.NewLine + m_error_message, "OpenVPN Manager",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// connect to the VPN <br />
        /// show message box on error
        /// </summary>
        public void connect()
        {
            try
            {
                m_vpn.connect();
            }
            catch (InvalidOperationException e)
            {
                /* 
                 * TODO it would be nicer if the message would hold less detail
                 * about the problem
                 */
                MessageBox.Show(Program.res.GetString("BOX_Error_Connect") +
                Environment.NewLine + e.Message, "OpenVPN Manager",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// disconnect from vpn <br />
        /// leave status window open
        /// </summary>
        public void disconnect()
        {
            disconnect(false);
        }

        /// <summary>
        /// disconnect from vpn
        /// </summary>
        /// <param name="closeForm">
        ///     close status window, this is important if you
        ///     change the configuration, because the status window will
        ///     still use the old configuration
        /// </param>
        public void disconnect(bool closeForm)
        {
            // disconnect if needed
            if(m_vpn != null)
                try
                {
                    m_vpn.disconnect();
                }
                catch (InvalidOperationException)
                {
                }

            if (m_frmpw != null)
                m_frmpw.Invoke(new closeDelegate(closeSubForm), m_frmpw);

            if (m_frmkey != null)
                m_frmkey.Invoke(new closeDelegate(closeSubForm), m_frmkey);

            if (m_frmlpw != null)
                m_frmlpw.Invoke(new closeDelegate(closeSubForm), m_frmlpw);

            // close the window if needed
            if (closeForm && m_status != null)
                m_status.Close();
        }

        private delegate void closeDelegate(Form f);
        private void closeSubForm(Form f) { f.Close(); }


        /// <summary>
        /// edit a configuration <br />
        /// this method simply starts notepad and opens the configuration file
        /// </summary>
        public void edit()
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
            disconnect();
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
            if (m_vpn != null)
                wasConnected = m_vpn.state == OVPNConnection.OVPNState.INITIALIZING ||
                m_vpn.state == OVPNConnection.OVPNState.RUNNING;

            // close the connection if needed, reload the configuration
            disconnect();
            init();

            // reconnect, if the user wants it
            if (wasConnected && m_error_message == null)
                if (MessageBox.Show(Program.res.GetString("BOX_Reconnect"),
                    "OpenVPN Manager", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                    connect();
        }

        /// <summary>
        /// OVPN changes it status.
        /// Invoke <c>stateChanged</c>.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        /// <seealso cref="stateChanged"/>
        private void m_vpn_stateChanged(object sender, EventArgs e)
        {
            // we are always in the wrong thread, so we invoke
            try
            {
                m_parent.Invoke(new EventHandler(stateChanged),
                    sender, e);
            }
            catch (ObjectDisposedException)
            {
            }
            catch (InvalidOperationException)
            {
                stateChanged(sender, e);
            }
        }

        /// <summary>
        /// OVPN changes it status.
        /// Show or hide elements.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void stateChanged(object sender, EventArgs e)
        {
            // set visiblity of menu items
            if(m_vpn.state == OVPNConnection.OVPNState.INITIALIZING)
            {
                m_menu_disconnect.Visible = false;
                m_menu_connect.Visible = false;
                m_menu.Image = Properties.Resources.STATE_Initializing;
            }
            else if(m_vpn.state == OVPNConnection.OVPNState.RUNNING)
            {
                m_menu_disconnect.Visible = true;
                m_menu_connect.Visible = false;
                m_menu.Image = Properties.Resources.STATE_Running;

                // show assigned ip if possible
                string text = Program.res.GetString("STATE_Connected");
                if (m_vpn.ip != null)
                    text += Environment.NewLine +
                        "IP: " + m_vpn.ip;

                m_parent.showPopup(name, text);
            }
            else if(m_vpn.state == OVPNConnection.OVPNState.STOPPED)
            {
                m_menu_disconnect.Visible = false;
                m_menu_connect.Visible = true;
                m_menu.Image = Properties.Resources.STATE_Stopped;
            }
            else if (m_vpn.state == OVPNConnection.OVPNState.STOPPING)
            {
                m_menu_disconnect.Visible = false;
                m_menu_connect.Visible = false;
                m_menu.Image = Properties.Resources.STATE_Stopping;
            }
            else if (m_vpn.state == OVPNConnection.OVPNState.ERROR)
            {
                m_menu_disconnect.Visible = false;
                m_menu_connect.Visible = true;
                m_menu.Image = Properties.Resources.STATE_Error;

                if (MessageBox.Show(Program.res.GetString("BOX_VPN_Error"),
                    "OpenVPN Manager", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button2)
                    == DialogResult.Yes)
                {
                    ProcessStartInfo pi = new ProcessStartInfo();
                    pi.Arguments = "\"" + m_tempLog + "\"";
                    pi.ErrorDialog = true;
                    pi.FileName = "notepad.exe";
                    pi.UseShellExecute = true;


                    Process.Start(pi);
                }
            }

            m_parent.stateChanged();
        }

        /// <summary>
        /// OVPN requests a password <br />
        /// generates and shows a form, answers via e
        /// </summary>
        /// <param name="sender">OVPN which requests the password</param>
        /// <param name="e">Information, what is needed</param>
        private void m_vpn_needPassword(object sender, OVPNNeedPasswordEventArgs e)
        {
            m_frmpw = new frmPasswd();
            e.password = m_frmpw.askPass(e.pwType, name);

            // if no password was entered, disconnect
            if (e.password == null && vpn.state == OVPNConnection.OVPNState.INITIALIZING)
                m_disconnectTimer.Start();
            m_frmpw = null;
        }

        /// <summary>
        /// OVPN requests a username and password <br />
        /// generates and shows a form, answers via e
        /// </summary>
        /// <param name="sender">OVPN which requests the username and password</param>
        /// <param name="e">Information, what is needed</param>
        private void m_vpn_needLoginAndPassword(object sender, OVPNNeedLoginAndPasswordEventArgs e)
        {
            m_frmlpw = new frmLoginAndPasswd();
            string[] loginfo = null;
            loginfo = m_frmlpw.askLoginAndPass(e.pwType, name);
            e.username = loginfo[0];
            e.password = loginfo[1];

            // if no password was entered, disconnect
            if ((e.password == null || e.username == null) && vpn.state == OVPNConnection.OVPNState.INITIALIZING)
                m_disconnectTimer.Start();

            m_frmlpw = null;
        }

        /// <summary>
        /// OVPN requests a SmardCard id <br />
        /// generates and shows a form, answers via e
        /// </summary>
        /// <param name="sender">OVPN which requests the id</param>
        /// <param name="e">Information, what was found</param>
        private void m_vpn_needCardID(object sender, OVPNNeedCardIDEventArgs e)
        {
            // if there is no id
            if(e.cardDetails.GetUpperBound(0) == -1)
            {
                if (MessageBox.Show(Program.res.GetString("BOX_NoKey"),
                    "OpenVPN Manager", MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Warning) == DialogResult.Retry)

                    e.selectedID = OVPNNeedCardIDEventArgs.RETRY;
                else
                {
                    e.selectedID = OVPNNeedCardIDEventArgs.NONE;
                    m_disconnectTimer.Start();
                    
                }
            }

            // if there is only one id, use it
            else if (e.cardDetails.GetUpperBound(0) == e.cardDetails.GetLowerBound(0))
                e.selectedID = e.cardDetails.GetLowerBound(0);
            else
            {
                // request key
                m_frmkey = new frmSelectPKCS11Key();
                int res = m_frmkey.selectKey(e.cardDetails, this.name);
                if (res == -1)
                {
                    e.selectedID = OVPNNeedCardIDEventArgs.NONE;
                    if (vpn.state == OVPNConnection.OVPNState.INITIALIZING)
                    {
                        m_disconnectTimer.Start();
                    }
                }
                else if (res == -2)
                    e.selectedID = OVPNNeedCardIDEventArgs.RETRY;
                else
                    e.selectedID = res;
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
            showErrors();
        }

        /// <summary>
        /// edit was selected in the context menu
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        void m_menu_edit_Click(object sender, EventArgs e)
        {
            edit();
        }

        /// <summary>
        /// disconnect was selected in the context menu
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void m_menu_disconnect_Click(object sender, EventArgs e)
        {
            // disconnect only, if we are connected
            if (m_vpn.state == OVPNConnection.OVPNState.INITIALIZING ||
                m_vpn.state == OVPNConnection.OVPNState.RUNNING)

                disconnect();
        }

        /// <summary>
        /// connect was selected in the context menu
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void m_menu_connect_Click(object sender, EventArgs e)
        {
            // connect only, if we are disconnected
            if (m_vpn.state == OVPNConnection.OVPNState.STOPPED)
                connect();
        }

        /// <summary>
        /// show was selected in the context menu
        /// </summary>
        /// <param name="sender">ignore</param>
        /// <param name="e">ignore</param>
        private void m_menu_show_Click(object sender, EventArgs e)
        {
            // show status window
            showStatus();
        }

        /// <summary>
        /// Determines, whether this configuration is connected.
        /// </summary>
        public bool running
        {
            get { return m_vpn != null && m_vpn.state != OVPNConnection.OVPNState.STOPPED; }
        }

        /// <summary>
        /// Generates a quickinfo-line which holds information about the configuration.
        /// </summary>
        public string quickInfo
        {
            get
            {
                if (m_vpn != null)
                    if (m_vpn.state == OVPNConnection.OVPNState.RUNNING)
                        if (m_vpn.ip != null)
                            return name + ": " + m_vpn.ip;
                        else
                            return name + ": " + Program.res.GetString("STATE_Connected");
                
                return null;
            }
        }
    }
}
