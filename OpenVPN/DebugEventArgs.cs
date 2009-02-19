using System;

namespace OpenVPN
{
    /// <summary>
    /// describes a debug event
    /// </summary>
    internal class DebugEventArgs : EventArgs
    {
        #region variables
        /// <summary>
        /// represents the verbose level of the log event
        /// </summary>
        private int m_level;

        /// <summary>
        /// represents the message of the log event
        /// </summary>
        private string m_msg;
        #endregion

        /// <summary>
        /// creates a new object
        /// </summary>
        /// <param name="level">verbose level of the message</param>
        /// <param name="msg">text of the message</param>
        internal DebugEventArgs(int level, string msg)
        {
            m_level = level;
            m_msg = msg;
        }

        /// <summary>
        /// get the verbose level
        /// </summary>
        public int level
        {
            get { return m_level; }
        }

        /// <summary>
        /// get the message
        /// </summary>
        public string msg
        {
            get { return m_msg; }
        }
    }
}
