using System;
using System.IO;
using System.Windows.Forms;
using OpenVPNUtils;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Diagnostics;
using Microsoft.Win32;
using System.Security;

namespace OpenVPNManager
{
    /// <summary>
    /// Manages settings.
    /// Note that - for various reasons - all openvpn processes should be
    /// closed when you edit those values.
    /// </summary>
    public partial class FrmSettings : Form
    {
        [CLSCompliant(false)]
        [DllImport("user32")]
        public static extern UInt32 SendMessage
            (IntPtr hWnd, UInt32 msg, UInt32 wParam, UInt32 lParam);

        internal const int BCM_FIRST = 0x1600; //Normal button
        internal const int BCM_SETSHIELD = (BCM_FIRST + 0x000C); //Elevated button

        string m_error;
        
        #region constructor
        /// <summary>
        /// Initializes the form.
        /// </summary>
        public FrmSettings()
        {
            InitializeComponent();
            numDbgLevel.Value = Properties.Settings.Default.debugLevel;
            chkAutostart.Checked = Helper.DoesAutostart();
            cmbUpdate.SelectedIndex = Properties.Settings.Default.searchUpdate;
            chkRememberPosition.Checked = Properties.Settings.Default.mainFormSavePosition;

            RefreshServiceFields();
            AddShieldToButton(InstallServiceButton);
            AddShieldToButton(UpdateServiceButton);
            AddShieldToButton(RemoveServiceButton);
            UpdateButtonStatus();
        }

        private void UpdateButtonStatus()
        {
            bool installed = ServiceIsInstalled();
            InstallServiceButton.Enabled = !installed;
            UpdateServiceButton.Enabled = installed;
            RemoveServiceButton.Enabled = installed;
        }

        private void RefreshServiceFields()
        {
            txtOVPNManagereServiceConf.Text = UtilsHelper.FixedConfigDir;

            if (Helper.ServiceKeyExists())
            {
                txtOVPNServiceConf.Text = UtilsHelper.LocateOpenVPNServiceDir();

                string ext = Helper.LocateOpenVPNServiceFileExt();
                if (ext.Length > 0)
                {
                    txtOVPNServiceExt.Text = "*." + ext;
                }
                else
                {
                    txtOVPNServiceExt.Text = "";
                }

                if (Helper.CanUseService())
                {
                    lblServiceEnabled.Text = Program.res.GetString("DIALOG_Enabled");
                    llWhy.Visible = false;
                }
                else
                {
                    m_error = Program.res.GetString("BOX_Service_Same_Path");
                    lblServiceEnabled.Text = Program.res.GetString("DIALOG_Disabled");
                    llWhy.Visible = true;
                }

            }
            else
            {
                m_error = Program.res.GetString("BOX_Service_Not_Installed");
                lblServiceEnabled.Text = Program.res.GetString("DIALOG_Disabled");
                llWhy.Visible = true;
            }
        }
        #endregion

        /// <summary>
        /// User wants to browse for openvpn, show dialog, save result.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnBrowseOVPNFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = false;
            ofd.CheckFileExists = true;
            ofd.ShowHelp = false;
            ofd.ShowReadOnly = false;
            ofd.Title = Program.res.GetString("DIALOG_Title_Open_OpenVPN");
            ofd.FileName = txtOVPNFile.Text;
            ofd.Filter = Program.res.GetString("DIALOG_Filter_Application") +
                " (*.exe)|*.exe|" +
                Program.res.GetString("DIALOG_Filter_Allfiles") +
                " (*.*)|*.*";

            try
            {
                ofd.InitialDirectory = Path.GetDirectoryName(txtOVPNFile.Text);
            }
            catch(ArgumentException)
            {
            }

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string oldBin = txtOVPNFile.Text;
                txtOVPNFile.Text = ofd.FileName;
                WarnServiceChange(oldBin, ofd.FileName);
                Properties.Settings.Default.Save();
            }
            RefreshServiceFields();
        }

        private void WarnServiceChange(string oldBin, string p)
        {
            if (ServiceIsInstalled() && oldBin != p)
            {
                RTLMessageBox.Show(Program.res.GetString("BOX_UpdateService"),
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// User wants to browse for directory, show dialog, save result.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnBrowseOVPNDir_Click(object sender, EventArgs e)
        {
            // Initalize dialog.
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = Program.res.GetString("DIALOG_Title_Folder");
            fbd.SelectedPath = txtOVPNConf.Text;
            fbd.ShowNewFolderButton = false;

            // show dialog, save result
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtOVPNConf.Text = fbd.SelectedPath;
                Properties.Settings.Default.Save();
            }
            RefreshServiceFields();
        }

        /// <summary>
        /// Detect link was clicked.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void llDetect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Detect();
            RefreshServiceFields();
        }

        /// <summary>
        /// Try to detect openvpn binary and configuration directory.
        /// </summary>
        public void Detect()
        {
            // reset pathes
            string oldVPNBin = Properties.Settings.Default.vpnbin;
            Properties.Settings.Default.vpnbin = "";
            Properties.Settings.Default.vpnconf = "";
            Properties.Settings.Default.Save();

            // locate vpn
            string vpnbin = UtilsHelper.LocateOpenVPN();
            if (vpnbin == null)
            {
                return;
            }
            else
            {
                Properties.Settings.Default.vpnbin = vpnbin;
                WarnServiceChange(vpnbin, oldVPNBin);
            }

            Properties.Settings.Default.Save();

            // try to locate configuration
            string vpnconf = UtilsHelper.LocateOpenVPNConfigDir(vpnbin);

            // vpn config was not found, abort
            if (vpnconf == null)
                return;
            // vpn config was found, save path
            else
                Properties.Settings.Default.vpnconf = vpnconf;

            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// user wants to close the formular
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Save all settings which don't save automatically :-)
        /// </summary>
        private void Save()
        {
            Properties.Settings.Default.debugLevel =
                Decimal.ToInt32(numDbgLevel.Value);
            Properties.Settings.Default.searchUpdate = 
                cmbUpdate.SelectedIndex;
            Properties.Settings.Default.mainFormSavePosition = chkRememberPosition.Checked;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Close the form.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void FrmSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Called, when the user swiches the autostart feature,
        /// installs or removes the command.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void chkAutostart_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutostart.Checked)
                Helper.InstallAutostart();
            else
                Helper.RemoveAutostart();
        }

        private void FrmSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                e.Handled = true;
                switch (e.KeyCode)
                {
                    case Keys.C:
                        btnClose_Click(null, null);
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }
        }

        private void llWhy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RTLMessageBox.Show(this,
                m_error, MessageBoxIcon.Information);
        }

        private void llHowChange_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RTLMessageBox.Show(this,
                Program.res.GetString("BOX_Service_How_Change"),
                MessageBoxIcon.Information);
        }

        private void btnClearOVPNFile_Click(object sender, EventArgs e)
        {
            txtOVPNFile.Text = "";
            Properties.Settings.Default.Save();
            RefreshServiceFields();
        }

        private void btnClearOVPNDir_Click(object sender, EventArgs e)
        {
            txtOVPNConf.Text = "";
            Properties.Settings.Default.Save();
            RefreshServiceFields();
        }

        static internal bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id);
            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static internal void AddShieldToButton(Button b)
        {
            if (!IsAdmin())
            {
                b.FlatStyle = FlatStyle.System;
                SendMessage(b.Handle, BCM_SETSHIELD, 0, 0xFFFFFFFF);
            }
        }

        private void InstallServiceButton_Click(object sender, EventArgs e)
        {
            RunServiceInstall("install");
            UpdateButtonStatus();
        }

        private void UpdateServiceButton_Click(object sender, EventArgs e)
        {
            RunServiceInstall("reinstall");
        }

        private void RemoveServiceButton_Click(object sender, EventArgs e)
        {
            RunServiceInstall("uninstall");
            UpdateButtonStatus();
        }

        private void RunServiceInstall(string action)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = GetServiceExecutable();
            
            if (Environment.OSVersion.Version.Major >= 6)
                startInfo.Verb = "runas";

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;

            startInfo.Arguments = action + " \"" + txtOVPNFile.Text + "\"";

            try
            {
                Process p = Process.Start(startInfo);
                p.WaitForExit();
            }
            catch (System.ComponentModel.Win32Exception)
            {
            }
        }

        private string GetServiceExecutable()
        {
            string basePath = Path.GetDirectoryName(Application.ExecutablePath);
            return basePath + Path.DirectorySeparatorChar + "OpenVPNManagerService.exe";
        }

        private static bool ServiceIsInstalled()
        {
            bool exists = false;

            try
            {
                RegistryKey k = Registry.LocalMachine.OpenSubKey(
                    @"SYSTEM\CurrentControlSet\services\OpenVPNManager");

                if (k != null)
                {
                    exists = true;
                    k.Close();
                }
            }
            catch (SecurityException)
            { }

            return exists;
        }
    }
}
