using System;

namespace OpenVPN
{
    /// <summary>
    /// information about the required/entered password
    /// </summary>
    public class NeedPasswordEventArgs : EventArgs
    {
        /// <summary>
        /// type of password
        /// </summary>
        private string m_pwType;

        /// <summary>
        /// gernerates new EventArguments
        /// </summary>
        /// <param name="pwType">type of needed password</param>
        internal NeedPasswordEventArgs(string pwType)
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
        /// selected password
        /// </summary>
        public string Password
        {
            get;
            set;
        }
    }
}
