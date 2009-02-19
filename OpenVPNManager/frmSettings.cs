using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace OpenVPNManager
{
    /// <summary>
    /// Manages settings.
    /// Note that - for various reasons - all openvpn processes should be
    /// closed when you edit those values.
    /// </summary>
    public partial class frmSettings : Form
    {
        #region constructor
        /// <summary>
        /// Initializes the form.
        /// </summary>
        public frmSettings()
        {
            InitializeComponent();
            numDbgLevel.Value = Properties.Settings.Default.debugLevel;
            chkAutostart.Checked = helper.doesAutostart();
            cmbUpdate.SelectedIndex = Properties.Settings.Default.searchUpdate;
        }
        #endregion

        /// <summary>
        /// User wants to browse for openvpn, show dialog, save result.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnBrowseOVPNFile_Click(object sender, EventArgs e)
        {
            // initialize the dialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = false;
            ofd.CheckFileExists = true;
            ofd.ShowHelp = false;
            ofd.ShowReadOnly = false;
            ofd.Title = Program.res.GetString("DIALOG_Title_Open_OpenVPN");
            ofd.FileName = txtOVPNFile.Text;

            // filter
            ofd.Filter = Program.res.GetString("DIALOG_Filter_Application") +
                " (*.exe)|*.exe|" +
                Program.res.GetString("DIALOG_Filter_Allfiles") +
                " (*.*)|*.*";

            // try to set initial directory
            try
            {
                ofd.InitialDirectory = Path.GetDirectoryName(txtOVPNFile.Text);
            }
            catch(ArgumentException)
            {
            }

            // save result
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtOVPNFile.Text = ofd.FileName;
                Properties.Settings.Default.Save();
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
        }

        /// <summary>
        /// Detect link was clicked.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void llDetect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            detect();
        }

        /// <summary>
        /// Try to detect openvpn binary and configuration directory.
        /// </summary>
        public void detect()
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
        private void frmSettings_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
