using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using OpenVPN.States;

namespace OpenVPNManager
{
    /// <summary>
    /// Holds all VPN configurations, represents the status.
    /// </summary>
    public partial class FrmGlobalStatus : Form
    {

        #region variables

        /// <summary>
        /// Represents a list of available vpn configuration.
        /// </summary>
        private List<VPNConfig> m_configs = new List<VPNConfig>();

        /// <summary>
        /// Shall we quit or minimize?
        /// </summary>
        private bool m_quit;

        /// <summary>
        /// Holds the about form.
        /// </summary>
        private FrmAbout m_about = new FrmAbout();

        /// <summary>
        /// Holds information about connections, which are resumed later.
        /// </summary>
        private List<VPNConfig> m_resumeList = new List<VPNConfig>();

        /// <summary>
        /// Connection used tu controle this software via tcp (for commandline control)
        /// </summary>
        private SimpleComm m_simpleComm = new SimpleComm(4911);
        #endregion

        #region constructor
        /// <summary>
        /// Generate the form, load configs, show initial settings if needed.
        /// </summary>
        /// <param name="configs">Strings passed on the commandline</param>
        public FrmGlobalStatus(string[] commands)
        {
            InitializeComponent();
            ReadConfigs();

            niIcon.Icon = Properties.Resources.TRAY_Disconnected;

            bool checkupdate = false;
            TimeSpan ts = Properties.Settings.Default.lastUpdateCheck
                - DateTime.Now;

            if (Properties.Settings.Default.searchUpdate == 0 ||
               (ts.Days > 7 && Properties.Settings.Default.searchUpdate == 1) ||
               (ts.Days > 30 && Properties.Settings.Default.searchUpdate == 2))
            {
                checkupdate = true;
                Properties.Settings.Default.lastUpdateCheck =
                    DateTime.Now;
                Properties.Settings.Default.Save();
            }

            if (checkupdate)
            {
                Update u = new Update(true, this);
                if (u.checkUpdate())
                {
                    niIcon.ShowBalloonTip(5000,
                        Program.res.GetString("QUICKINFO_Update"),
                        Program.res.GetString("QUICKINFO_Update_More"),
                        ToolTipIcon.Info);
                }
            }

            m_simpleComm.ReceivedLines += new 
                EventHandler<SimpleComm.ReceivedLinesEventArgs>(
                m_simpleComm_ReceivedLines);

            parseCommandLine(commands);

            if(Properties.Settings.Default.allowRemoteControl)
                m_simpleComm.startServer();

            // if this is the first start: show settings
            if (Properties.Settings.Default.firstStart)
            {
                Properties.Settings.Default.firstStart = false;
                Properties.Settings.Default.Save();
                ShowSettings(true);
            }
        }
        #endregion

        /// <summary>
        /// Our Simplecom got some parameters.
        /// </summary>
        /// <param name="sender">The sending SimplCcom, ignored.</param>
        /// <param name="e">Event arguments, holds the received parameters.</param>
        private void m_simpleComm_ReceivedLines(object sender, SimpleComm.ReceivedLinesEventArgs e)
        {
            // Change thread, if we must
            if (this.InvokeRequired)
                this.Invoke(new helper.Action<string[]>(parseCommandLine), new Object[]{ e.lines });
            else
                parseCommandLine(e.lines);
        }

        /// <summary>
        /// Executes the given commandline.
        /// </summary>
        /// <param name="commands">Array of commands</param>
        private void parseCommandLine(string[] commands)
        {
            List<string> args = new List<string>(commands);

            int i = 0;
            bool found = false;
            string names;

            while (i < args.Count)
            {
                switch (args[i].ToUpperInvariant())
                {
                    // -connect "vpn name"
                    case "-CONNECT":
                        if (i == args.Count - 1)
                        {
                            RTLMessageBox.Show(this, String.Format(
                                CultureInfo.InvariantCulture,
                                Program.res.GetString(
                                "ARGS_Missing_Parameter"),
                                args[i]), MessageBoxIcon.Error);
                            return;
                        }

                        found = false;
                        names = "";

                        foreach (VPNConfig c in m_configs)
                        {
                            names += c.Name + "\n";
                            if (c.Name.Equals(args[i + 1],
                                StringComparison.OrdinalIgnoreCase))
                            {
                                c.Connect();
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            RTLMessageBox.Show(this, String.Format(
                                CultureInfo.InvariantCulture, 
                                Program.res.GetString(
                                    "ARGS_Invalid_Parameter"), 
                                args[i], args[i + 1], names),
                                MessageBoxIcon.Error);
                        }

                        ++i;
                        break;

                    case "-DISCONNECT":
                        if (i == args.Count - 1)
                        {
                            RTLMessageBox.Show(this, String.Format(
                                CultureInfo.InvariantCulture,
                                Program.res.GetString(
                                    "ARGS_Missing_Parameter"),
                                args[i]), MessageBoxIcon.Error);
                            return;
                        }

                        found = false;
                        names = "";

                        foreach (VPNConfig c in m_configs)
                        {
                            names += c.Name + "\n";
                            if (c.Name.Equals(args[i + 1],
                                StringComparison.OrdinalIgnoreCase))
                            {
                                c.Disconnect();
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            RTLMessageBox.Show(this, String.Format(
                                CultureInfo.InvariantCulture,
                                Program.res.GetString(
                                    "ARGS_Invalid_Parameter"), 
                                args[i], args[i + 1], names),
                                MessageBoxIcon.Error);
                        }

                        ++i;

                        break;

                    case "-QUIT":
                    case "-EXIT":
                        m_quit = true;
                        this.Close();
                        break;

                    default:
                        RTLMessageBox.Show(this, String.Format(
                            CultureInfo.InvariantCulture,
                            Program.res.GetString(
                                "ARGS_Unknown_Parameter"),
                            args[i]), MessageBoxIcon.Error);
                        break;
                }

                ++i;
            }
        }
        
        /// <summary>
        /// Unloads all configs, remove controls.
        /// </summary>
        public void UnloadConfigs()
        {
            pnlStatus.Controls.Clear();

            foreach (VPNConfig vpnc in m_configs)
                vpnc.Disconnect(true);

            // disconnect all configs, remove menu items
            while (m_configs.Count > 0)
            {
                while (
                    m_configs[0].VPNConnection.State.CreateSnapshot().ConnectionState
                        != VPNConnectionState.Stopped)
                {
                    System.Threading.Thread.Sleep(200);
                }
                contextMenu.Items.Remove(m_configs[0].Menuitem);
                m_configs.Remove(m_configs[0]);
            }

            toolStripSeparator2.Visible = false;
        }

        /// <summary>
        /// Read all configs, initialize/add controls, etc.
        /// </summary>
        public void ReadConfigs()
        {
            // unload config first, if needed
            UnloadConfigs();

            // find config files
            string[] configs =
                helper.locateOpenVPNConfigs(Properties.Settings.Default.vpnconf);

            // insert configs in context menu and panel
            int atIndex = 2;
            if (configs != null)
            {
                toolStripSeparator2.Visible = true;

                foreach (string cfile in configs)
                {
                    try
                    {
                        VPNConfig c = VPNConfig.CreateUserspaceConnection(
                            Properties.Settings.Default.vpnbin,
                            cfile, Properties.Settings.Default.debugLevel,
                            this);

                        m_configs.Add(c);
                        contextMenu.Items.Insert(atIndex++, c.Menuitem);
                        pnlStatus.Controls.Add(c.InfoBox);
                    }
                    catch (ArgumentException e)
                    {
                        RTLMessageBox.Show(this,
                            Program.res.GetString("BOX_Config_Error") +
                            Environment.NewLine + cfile + ": " +
                            e.Message, MessageBoxIcon.Exclamation);
                    }
                }
            }

            if (helper.canUseService())
            {
                configs = helper.locateOpenVPNServiceConfigs();

                if (configs != null)
                {
                    toolStripSeparator2.Visible = true;

                    foreach (string cfile in configs)
                    {
                        try
                        {
                            VPNConfig c = VPNConfig.CreateServiceConnection(
                                cfile, Properties.Settings.Default.debugLevel,
                                this);

                            m_configs.Add(c);
                            contextMenu.Items.Insert(atIndex++, c.Menuitem);
                            pnlStatus.Controls.Add(c.InfoBox);
                        }
                        catch (ArgumentException e)
                        {
                            RTLMessageBox.Show(this,
                                Program.res.GetString("BOX_Config_Error") +
                                Environment.NewLine + cfile + ": " +
                                e.Message, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Show settings, if wanted, detect defaults.
        /// </summary>
        /// <param name="Detect"></param>
        public void ShowSettings(bool detect)
        {
            foreach (VPNConfig c in m_configs)
            {
                if (c.Running)
                {
                    if (RTLMessageBox.Show(this,
                        Program.res.GetString("BOX_Settings_Close"),
                        MessageBoxButtons.YesNoCancel, 
                        MessageBoxDefaultButton.Button2,
                        MessageBoxIcon.Exclamation) == DialogResult.Yes)

                        break;
                    else
                        return;
                }
            }

            // remember visible-state, hide everything, unload everything
            bool reShow = Visible;
            niIcon.Visible = false;
            Hide();
            if (Properties.Settings.Default.allowRemoteControl)
                m_simpleComm.stopServer();
            UnloadConfigs();

            // show settings, detect settings
            FrmSettings m_settingsDialog = new FrmSettings();
            if (detect)
                m_settingsDialog.Detect();

            m_settingsDialog.ShowDialog();

            // reread settings, show icon, show form if needed
            ReadConfigs();
            if (Properties.Settings.Default.allowRemoteControl)
                m_simpleComm.startServer();
            niIcon.Visible = true;

            if (reShow)
                Show();
        }

        /// <summary>
        /// Show popup to user.
        /// </summary>
        /// <param name="title">title of the popup</param>
        /// <param name="message">message of the popup</param>
        public void ShowPopup(string title, string message)
        {
            niIcon.ShowBalloonTip(2000, title, message, ToolTipIcon.Info);
        }

        /// <summary>
        /// User wants to close the form, just hide it.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // just hide
            Hide();
        }

        /// <summary>
        /// User wants to quit, exit application.
        /// </summary>
        /// <param name="sender">ignore</param>
        /// <param name="e">ignore</param>
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // yeah, we want to quit
            m_quit = true;

            Close();
            Application.Exit();
        }

        /// <summary>
        /// User wants to show settings.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettings(false);
        }

        /// <summary>
        /// User doubleclicked notify icon: show/hide icon.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void niIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Visible)
                Hide();
            else
            {
                Show();

                // If we were minimized...
                WindowState = FormWindowState.Normal;            
            }
        }

        /// <summary>
        /// Form is about to be closed, unload or hide it.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">information about the action</param>
        private void FrmGlobalStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            // did the user clicked on "x"? just hide the form
            if (!m_quit && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                return;
            }

            UnloadConfigs();
            if (Properties.Settings.Default.allowRemoteControl)
                m_simpleComm.stopServer();
            Application.Exit();
        }

        /// <summary>
        /// Form is resized (minimized).
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void FrmGlobalStatus_Resize(object sender, EventArgs e)
        {
            // if the form is minimized, hide it instead
            if (this.WindowState == FormWindowState.Minimized)
                Hide();
        }

        /// <summary>
        /// User selected status, show this form.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
        }

        /// <summary>
        /// User clicked about, show the form.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnAbout_Click(object sender, EventArgs e)
        {
            m_about.Show();
        }

        /// <summary>
        /// User clicked about, show the form.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_about.Show();
        }

        /// <summary>
        /// User wants to quit.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnQuit_Click(object sender, EventArgs e)
        {
            m_quit = true;
            Close();
        }

        /// <summary>
        /// User wants to show settings.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShowSettings(false);
        }

        /// <summary>
        /// This method refreshs the Quickinfo (Tooltip) of the NotifyIcon.
        /// It should display all VPN-IPs, but at the moment, it only
        /// displays the program name.
        /// </summary>
        private void refreshQuickInfo()
        {
            niIcon.Text = "OpenVPN Manager";
            // TODO: fix this

            /*
            StringBuilder text = new StringBuilder();
            foreach(VPNConfig c in m_configs)
            {
                if(c.quickInfo != null)
                    text.AppendLine(c.quickInfo);
            }

            String s = "OpenVPN Manager";
            if (text.Length > 0)
                niIcon.Text = "OpenVPN Manager:" + Environment.NewLine +
                    text.ToString();
            else
                niIcon.Text = s;*/
        }

        /// <summary>
        /// Formular ist shown after it is loaded.
        /// If the user wants to start minimized, minimize now.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        /// <remarks>
        ///     There ist no way to start the form without showing it,
        ///     because if you don't do it, the invokes go bad.
        /// </remarks>
        private void FrmGlobalStatus_Shown(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.startMinimized)
                Hide();
        }

        /// <summary>
        /// A key was pressed. Initialize a button click.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">the pressed key</param>
        private void FrmGlobalStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                e.Handled = true;
                switch (e.KeyCode)
                {
                    case Keys.C:
                        btnClose_Click(null, null);
                        break;
                    case Keys.Q:
                        btnQuit_Click(null, null);
                        break;
                    case Keys.S:
                        btnSettings_Click(null, null);
                        break;
                    case Keys.A:
                        btnAbout_Click(null, null);
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// If the status panel is resized, resize all its clients.
        /// </summary>
        /// <param name="sender">the panel</param>
        /// <param name="e">ignored</param>
        private void pnlStatus_Resize(object sender, EventArgs e)
        {
            Panel p = (Panel) sender;
            foreach(UserControl c in p.Controls)
                c.Width = p.ClientSize.Width - 
                    p.Margin.Horizontal;
            pnlStatus.ResumeLayout(true);
        }

        /// <summary>
        /// Called if the state of a OpenVPN Config has changed.
        /// Redraw the Icon, etc.
        /// </summary>
        public void StateChanged() { 
            int c = 0, w = 0;
            foreach (VPNConfig conf in m_configs)
                if (conf.VPNConnection != null)
                    if (conf.VPNConnection.State.CreateSnapshot().ConnectionState
                        == VPNConnectionState.Running)
                        c++;
                    else if (conf.VPNConnection.State.CreateSnapshot().ConnectionState
                        == VPNConnectionState.Initializing)
                        w++;

            try
            {
                if (c > 0 && w == 0)
                    niIcon.Icon = Properties.Resources.TRAY_Connected;
                else if (c == 0 && w > 0)
                    niIcon.Icon = Properties.Resources.TRAY_Connecting;
                else if (c == 0 && w == 0)
                    niIcon.Icon = Properties.Resources.TRAY_Disconnected;
                else
                    niIcon.Icon = Properties.Resources.TRAY_Multiple;
            }
            catch (NullReferenceException)
            { 
            }

            refreshQuickInfo();
        }

        /// <summary>
        /// Resize of the form has finished, resize the status panel a last time.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">passed to pnlStatus</param>
        private void FrmGlobalStatus_ResizeEnd(object sender, EventArgs e)
        {
            pnlStatus_Resize(pnlStatus, e);
        }

        /// <summary>
        /// Closes all connection.
        /// This should be called before a System is hibernated.
        /// </summary>
        public void CloseAll()
        {
            if (Properties.Settings.Default.allowRemoteControl)
                m_simpleComm.stopServer();
            m_resumeList.Clear();

            foreach (VPNConfig c in m_configs)
            {
                if(c.Running)
                {
                    m_resumeList.Add(c);
                    c.Disconnect();
                }
            }
        }

        /// <summary>
        /// Resumes all closed connection.
        /// This should be called after the System is restarted after a hibernation.
        /// </summary>
        public void ResumeAll()
        {
            foreach (VPNConfig c in m_resumeList)
            {
                c.Connect();
            }
            m_resumeList.Clear();
            if (Properties.Settings.Default.allowRemoteControl)
                m_simpleComm.startServer();
        }
    }
}
