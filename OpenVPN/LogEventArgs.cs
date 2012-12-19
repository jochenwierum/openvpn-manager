using System;

namespace OpenVPNUtils
{
    /// <summary>
    /// The type of the log.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// OpenVPN changed the internal state.l
        /// </summary>
        State,

        /// <summary>
        /// The management interface wants to say something.
        /// </summary>
        Management,

        /// <summary>
        /// A "normal" message is logged by OpenVPN via Management Interface.
        /// </summary>
        Log,

        /// <summary>
        /// A debug message is sent. This is primary for internal usage.
        /// </summary>
        Debug
    }

    /// <summary>
    /// Information about a LogEvent.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// The type of Message.
        /// </summary>
        private LogType m_type;

        /// <summary>
        /// The real message.
        /// </summary>
        private string m_msg;

        /// <summary>
        /// Timestamp.
        /// </summary>
        private DateTime m_time;

        /// <summary>
        /// Creates a new OVPNLogEventArgs object.
        /// </summary>
        /// <param name="type">type of message</param>
        /// <param name="msg">text of message</param>
        /// <param name="time">time of the message in unix time</param>
        internal LogEventArgs(LogType type, string msg, long time)
        {
            if(time == 0)
                m_time = (new DateTime(1070, 1, 1, 0, 0, 0)).AddSeconds(time);
            else
                m_time = DateTime.Now;
            m_type = type;
            m_msg = msg;
        }

        /// <summary>
        /// The log type.
        /// </summary>
        public LogType MessageType
        {
            get { return m_type; }
        }

        /// <summary>
        /// The message.
        /// </summary>
        public string Message
        {
            get { return m_msg; }
        }

        /// <summary>
        /// The time of the event.
        /// </summary>
        public DateTime Time
        {
            get { return m_time; }
        }
    }
}
