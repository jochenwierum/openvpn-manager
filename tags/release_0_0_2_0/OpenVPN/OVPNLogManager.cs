using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVPN
{
    /// <summary>
    /// Collects information about logged events.
    /// </summary>
    public class OVPNLogManager
    {
        #region events

        /// <summary>
        /// Delegate to a function which logs the information provided by OVPNLogEventArgs.
        /// </summary>
        /// <param name="sender">The LogManager which sends the event</param>
        /// <param name="e">Holds the text and the type of the log event</param>
        public delegate void LogEventDelegate(object sender, OVPNLogEventArgs e);

        /// <summary>
        /// A text to log was received.
        /// </summary>
        public event LogEventDelegate LogEvent;
        #endregion

        #region variables
        /// <summary>
        /// The Parent.
        /// </summary>
        private OVPN m_ovpn;

        /// <summary>
        /// Debug level. Log messages of the type DEBUG with a 
        /// level higher than <c>m_debugLevel</c> are ignored.
        /// </summary>
        private int m_debugLevel;
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new OVPNLogManager object.
        /// </summary>
        /// <param name="parent">the parent</param>
        internal OVPNLogManager(OVPN parent)
        {
            m_ovpn = parent;
        }
        #endregion

        #region properties

        /// <summary>
        /// The debug level. 
        /// Debug messages of a lower level (lower means a higher number) are ignored.
        /// </summary>
        public int debugLevel
        {
            get
            {
                return m_debugLevel;
            }

            set
            {
                m_debugLevel = value;
            }
        }
        #endregion

        /// <summary>
        /// Drops a debug line.
        /// </summary>
        /// <param name="level">the debug level of the message</param>
        /// <param name="msg">the message itself</param>
        /// <seealso cref="debugLevel"/>
        internal void logDebugLine(int level, string msg)
        {
            // only log the message if the debug level is high enough
            if (m_debugLevel >= level)
                LogEvent(this, new OVPNLogEventArgs(OVPNLogEventArgs.LogType.DEBUG,
                    "[" + level + "] " + msg));
        }

        /// <summary>
        /// Drops a line to log.
        /// </summary>
        /// <param name="type">The type of message</param>
        /// <param name="msg">The text to log</param>
        internal void logLine(OVPNLogEventArgs.LogType type, string msg)
        {
            try
            {
                if (m_ovpn.noevents) return;
                LogEvent(this, new OVPNLogEventArgs(type, msg));
            }
            catch (NullReferenceException)
            {
            }
        }
    }
}
