using System;
using System.Diagnostics.CodeAnalysis;

namespace OpenVPN
{
    /// <summary>
    /// information about the required/entered username and password
    /// </summary>
    
    [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
    public class NeedLoginAndPasswordEventArgs : EventArgs
    {
        /// <summary>
        /// type of password
        /// </summary>
        private string m_pwType;

        internal NeedLoginAndPasswordEventArgs(string pwType)
        {
            m_pwType = pwType;
        }

        /// <summary>
        /// type of password
        /// </summary>
        public string PasswordType
        {
            get { return m_pwType; }
        }

        /// <summary>
        /// selected user name
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// selected password
        /// </summary>
        public string Password
        {
            get;
            set;
        }
    }
}
