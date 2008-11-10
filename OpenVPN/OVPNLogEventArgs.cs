using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVPN
{
    /// <summary>
    /// Information about a LogEvent.
    /// </summary>
    public class OVPNLogEventArgs : EventArgs
    {
        #region enums

        /// <summary>
        /// The type of the log.
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// OpenVPN prints a message to stderr.
            /// </summary>
            STDERR,

            /// <summary>
            /// OpenVPN prints a message to stdout.
            /// </summary>
            STDOUT,

            /// <summary>
            /// The management wants to say something.
            /// </summary>
            MGNMT,

            /// <summary>
            /// A "normal" message is logged by OpenVPN via Management Interface.
            /// </summary>
            LOG,

            /// <summary>
            /// A debug message is sent. This is primary for internal usage.
            /// </summary>
            DEBUG
        }
        #endregion

        /// <summary>
        /// The type of Message.
        /// </summary>
        private LogType m_type;

        /// <summary>
        /// The real message.
        /// </summary>
        private string m_msg;

        /// <summary>
        /// Creates a new OVPNLogEventArgs object.
        /// </summary>
        /// <param name="type">type of message</param>
        /// <param name="msg">text of message</param>
        internal OVPNLogEventArgs(LogType type, string msg)
        {
            m_type = type;
            m_msg = msg;
        }

        /// <summary>
        /// The log type.
        /// </summary>
        public LogType type
        {
            get { return m_type; }
        }

        /// <summary>
        /// The message.
        /// </summary>
        public string message
        {
            get { return m_msg; }
        }
    }
}
