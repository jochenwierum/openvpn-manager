using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVPN
{
    /// <summary>
    /// holds data about an AsyncEvent
    /// </summary>
    internal class AsyncEventDetail
    {
        #region enums
        /// <summary>
        /// Type of event.
        /// </summary>
        public enum EventType
        {
            /// <summary>
            /// Just a message for the user.
            /// </summary>
            ECHO,

            /// <summary>
            /// A faral error.
            /// </summary>
            FATAL,

            /// <summary>
            /// A "hold" message. OpenVPN continues, 
            /// if the hold state is released.
            /// </summary>
            HOLD,

            /// <summary>
            /// An Info, e.g. a version number.
            /// </summary>
            INFO,

            /// <summary>
            /// A log line.
            /// </summary>
            LOG,

            /// <summary>
            /// A password is requested.
            /// </summary>
            PASSWORD,

            /// <summary>
            /// Unknown, but reserved.
            /// </summary>
            STATE,

            /// <summary>
            /// A string (e.g. a username) is needed.
            /// </summary>
            NEEDSTR,

            /// <summary>
            /// The type is unknown.
            /// </summary>
            UNKNOWN,
        }
        #endregion

        #region variables
        
        /// <summary>
        /// Type of event.
        /// </summary>
        private EventType m_type;

        /// <summary>
        /// Data of the event.
        /// </summary>
        private string m_msg;

        /// <summary>
        /// Additional information.
        /// </summary>
        private string[] m_infos;
        #endregion

        /// <summary>
        /// Creates the object,
        /// </summary>
        /// <param name="t">type of event</param>
        /// <param name="msg">message line</param>
        /// <param name="infos">optional additional data</param>
        internal AsyncEventDetail(EventType t, string msg, string[] infos)
        {
            m_type = t;
            m_msg = msg;
            m_infos = infos;
        }

        /// <summary>
        /// The sent data.
        /// </summary>
        public string message
        {
            get { return m_msg; }
        }

        /// <summary>
        /// The event type.
        /// </summary>
        public EventType eventType
        {
            get { return m_type; }
        }

        /// <summary>
        /// Additional information.
        /// </summary>
        /// <returns>array of preparsed information</returns>
        public string[] getInfos()
        {
            return m_infos;
        }
    }
}
