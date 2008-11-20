using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenVPNManager
{
    /// <summary>
    /// provides a formular which asks for a username and password
    /// </summary>
    public partial class frmLoginAndPasswd : Form
    {
        #region constructor
        /// <summary>
        /// generates the form
        /// </summary>
        public frmLoginAndPasswd()
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// Asks for a username and password.
        /// </summary>
        /// <param name="pwTitle">name of the password, e.g. 'Auth'</param>
        /// <param name="config">name of the config</param>
        /// <returns>the password or null if aborted</returns>
        public string[] askLoginAndPass(string pwTitle, string config)
        {
            // set labels
            lblAsk.Text = pwTitle;
            lblName.Text = config;

            // show form, return
            if (this.ShowDialog() != DialogResult.OK)
                return null;
            else
            {
                return new string[]{txtUsername.Text,txtPasswd.Text};
            }
        }
    }
}
