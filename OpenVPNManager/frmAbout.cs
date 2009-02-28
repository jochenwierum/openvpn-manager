using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace OpenVPNManager
{
    /// <summary>
    /// The about box.
    /// </summary>
    public partial class FrmAbout : Form
    {
        /// <summary>
        /// Initializes the about box.
        /// </summary>
        public FrmAbout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the form is loaded.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void frmAbout_Load(object sender, EventArgs e)
        {
            lblName.Text = Application.ProductName + " " + 
                Application.ProductVersion;
            lblName.Left = (this.Width - lblName.Width) / 2;
        }

        /// <summary>
        /// Called when the user clicks the "close" button.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // don't unload!
            this.Hide();
        }

        /// <summary>
        /// Opens a given URL in the default webbrowser.
        /// </summary>
        /// <param name="url">The url to open</param>
        private static void openUrl(string url)
        {
            if (url == null)
                return;

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = url;
            psi.UseShellExecute = true;

            try
            {
                Process.Start(psi);
            }
            catch (ApplicationException)
            { 
            }
        }

        /// <summary>
        /// The label is clicked.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openUrl("http://www.everaldo.com/crystal/");
        }

        /// <summary>
        /// The label is clicked.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openUrl("http://openvpn.jowisoftware.de/");
        }

        /// <summary>
        /// Shortcut is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAbout_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                e.Handled = true;
                switch (e.KeyCode)
                {
                    case Keys.C:
                        btnClose_Click(null, null);
                        break;
                    case Keys.U:
                        btnUpdateCheck_Click(null, null);
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// User clicks on "update".
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnUpdateCheck_Click(object sender, EventArgs e)
        {
            // check if an error is possible
            Update u = new Update(false, this);
            if (u.hadErrors)
                return;

            // update available?
            if (u.checkUpdate())
            {

                // open url, if the user wants
                if (RTLMessageBox.Show(this,
                    Program.res.GetString("BOX_UpdateInformation")
                    .Replace("%s", u.getVersion()),
                    MessageBoxButtons.YesNoCancel,  
                    MessageBoxDefaultButton.Button1,
                    MessageBoxIcon.Information)
                    == DialogResult.Yes)

                    openUrl(u.getUpdateUrl());
            }

            // no update found
            else 
                RTLMessageBox.Show(this,
                    Program.res.GetString("BOX_UpdateNone"),
                    MessageBoxIcon.Information);
        }
    }
}
