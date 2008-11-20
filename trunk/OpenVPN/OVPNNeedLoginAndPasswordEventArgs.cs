using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVPN
{
    /// <summary>
    /// information about the required/entered username and password
    /// </summary>
    public class OVPNNeedLoginAndPasswordEventArgs : EventArgs
    {

        #region variables
        /// <summary>
        /// type of password
        /// </summary>
        private string m_pwType;

        /// <summary>
        /// username entered by the user
        /// </summary>
        private string m_un;

        /// <summary>
        /// password entered by the user
        /// </summary>
        private string m_pw;
        #endregion

        internal OVPNNeedLoginAndPasswordEventArgs(string pwType)
        {
            m_pwType = pwType;
            m_pw = null;
            m_un = null;
        }

        /// <summary>
        /// type of password
        /// </summary>
        public string pwType
        {
            get { return m_pwType; }
        }

        /// <summary>
        /// selected username
        /// </summary>
        public string username
        {
            get { return m_un; }
            set { m_un = value; }
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
