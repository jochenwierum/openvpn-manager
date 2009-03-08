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
