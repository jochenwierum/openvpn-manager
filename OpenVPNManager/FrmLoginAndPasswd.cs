using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace OpenVPNManager
{
    /// <summary>
    /// provides a formular which asks for a username and password
    /// </summary>

    [type: SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", Scope = "type",
            Target = "OpenVPNManager.FrmLoginAndPasswd", MessageId = "Login")]
    public partial class FrmLoginAndPasswd : Form
    {

        #region constructor
        /// <summary>
        /// generates the form
        /// </summary>
        public FrmLoginAndPasswd()
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
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public string[] AskLoginAndPass(string pwTitle, string config)
        {
            // set labels
            lblAsk.Text = pwTitle;
            lblName.Text = config;

            ProfileSettings settings = Settings.current.getProfile(config);
            chkRememberName.Checked = settings.storeUserName;
            txtUsername.Text = settings.userName;
            // show form, return
            if (this.ShowDialog() != DialogResult.OK)
                return null;
            else
            {
                settings.storeUserName = chkRememberName.Checked;
                if (settings.storeUserName)
                    settings.userName = txtUsername.Text;
                Settings.current.Save();
                return new string[]{txtUsername.Text,txtPasswd.Text};
            }
        }

        private void txt_EnterSelectAll(object sender, System.EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void FrmLoginAndPasswd_Shown(object sender, System.EventArgs e)
        {

            if (chkRememberName.Checked)
                txtPasswd.Focus();
        }
    }
}
