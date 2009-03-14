using System;
using System.IO;
using System.Windows.Forms;

namespace OpenVPNManager
{
    /// <summary>
    /// Manages settings.
    /// Note that - for various reasons - all openvpn processes should be
    /// closed when you edit those values.
    /// </summary>
    public partial class FrmSettings : Form
    {
        string m_error;

        #region constructor
        /// <summary>
        /// Initializes the form.
        /// </summary>
        public FrmSettings()
        {
            InitializeComponent();
            numDbgLevel.Value = Properties.Settings.Default.debugLevel;
            chkAutostart.Checked = helper.doesAutostart();
            cmbUpdate.SelectedIndex = Properties.Settings.Default.searchUpdate;

            refreshServiceFields();
        }

        private void refreshServiceFields()
        {
            if (helper.serviceKeyExists())
            {
                txtOVPNServiceConf.Text = helper.locateOpenVPNServiceDir();

                string ext = helper.locateOpenVPNServiceFileExt();
                if (ext.Length > 0)
                {
                    txtOVPNServiceExt.Text = "*." + ext;
                }
                else
                {
                    txtOVPNServiceExt.Text = "";
                }

                if (helper.canUseService())
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
                txtOVPNFile.Text = ofd.FileName;
                Properties.Settings.Default.Save();
            }
            refreshServiceFields();
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
            refreshServiceFields();
        }

        /// <summary>
        /// Detect link was clicked.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void llDetect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Detect();
            refreshServiceFields();
        }

        /// <summary>
        /// Try to detect openvpn binary and configuration directory.
        /// </summary>
        public void Detect()
        {
            // reset pathes
            txtOVPNConf.Text = "";
            txtOVPNFile.Text = "";

            // locate vpn
            string vpnbin = helper.locateOpenVPN();

            // vpn was not found, abort
            if (vpnbin == null)
                return;
            // vpn was found, save path
            else
                txtOVPNFile.Text = vpnbin;

            // save settings
            Properties.Settings.Default.Save();

            // try to locate configuration
            string vpnconf = helper.locateOpenVPNConfigDir(vpnbin);

            // vpn config was not found, abort
            if (vpnconf == null)
                return;
            // vpn config was found, save path
            else
                txtOVPNConf.Text = vpnconf;

            // save settings
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
                helper.installAutostart();
            else
                helper.removeAutostart();
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
        }

        private void btnClearOVPNDir_Click(object sender, EventArgs e)
        {
            txtOVPNConf.Text = "";
            Properties.Settings.Default.Save();
        }
    }
}
