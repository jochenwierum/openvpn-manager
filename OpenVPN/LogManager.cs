using System;
#if DEBUG
using System.Diagnostics;
#endif

namespace OpenVPN
{
    /// <summary>
    /// Collects information about logged events.
    /// </summary>
    public class LogManager
    {
        /// <summary>
        /// A text to log was received.
        /// </summary>
        public event EventHandler<LogEventArgs> LogEvent;

        /// <summary>
        /// The parent object.
        /// </summary>
        private Connection m_ovpn;

        #region constructor
        /// <summary>
        /// Initializes a new OVPNLogManager object.
        /// </summary>
        /// <param name="parent">the parent</param>
        internal LogManager(Connection parent)
        {
            m_ovpn = parent;
        }
        #endregion

        #region properties

        /// <summary>
        /// The debug level. 
        /// Debug messages of a lower level (lower means a higher number) are ignored.
        /// </summary>
        public int DebugLevel
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Drops a debug line.
        /// </summary>
        /// <param name="level">the debug level of the message</param>
        /// <param name="msg">the message itself</param>
        /// <seealso cref="DebugLevel"/>
        internal void logDebugLine(int level, string msg)
        {
#if DEBUG
            Debug.WriteLine(level + ": " + msg);
#endif
            // only log the message if the debug level is high enough
            if (DebugLevel >= level)
                LogEvent(this, new LogEventArgs(LogType.Debug,
                    "[" + level + "] " + msg, 0));
        }

        /// <summary>
        /// Drops a line to log.
        /// </summary>
        /// <param name="type">The type of message</param>
        /// <param name="msg">The text to log</param>
        internal void logLine(LogType type, string msg)
        {
            logLine(type, msg, 0);
        }

        internal void logLine(LogType type, string msg, long time)
        {
#if DEBUG
            Debug.WriteLine("[" + type.ToString() + ", " + time + "]: " + msg);
#endif
            if(LogEvent != null && !m_ovpn.NoEvents)
                LogEvent(this, new LogEventArgs(type, msg, time));
        }
    }
}
