using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVPN
{
    /// <summary>
    /// information about the required/entered password
    /// </summary>
    public class OVPNNeedPasswordEventArgs : EventArgs
    {
        #region variables
        /// <summary>
        /// type of password
        /// </summary>
        private string m_pwType;

        /// <summary>
        /// password enterd by the user
        /// </summary>
        private string m_pw;
        #endregion

        /// <summary>
        /// gernerates new EventArguments
        /// </summary>
        /// <param name="pwType">type of needed password</param>
        internal OVPNNeedPasswordEventArgs(string pwType)
        {
            m_pwType = pwType;
            m_pw = null;
        }

        /// <summary>
        /// type of password
        /// </summary>
        public string pwType
        {
            get { return m_pwType; }
        }

        /// <summary>
        /// selected password
        /// </summary>
        public string password
        {
            get { return m_pw; }
            set { m_pw = value; }
        }
    }
}
