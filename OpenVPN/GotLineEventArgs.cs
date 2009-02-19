using System;

namespace OpenVPN
{
    /// <summary>
    /// event which describes a received text line
    /// </summary>
    internal class GotLineEventArgs : EventArgs
    {
        #region variables
        /// <summary>
        /// revceived line
        /// </summary>
        private string m_line;
        #endregion

        /// <summary>
        /// initializes a new object, saves the received line
        /// </summary>
        /// <param name="line">the received line</param>
        internal GotLineEventArgs(string line)
        {
            m_line = line;
        }

        /// <summary>
        /// get the received text line;
        /// null is transformed to an empty string
        /// </summary>
        public string line
        {
            get
            {
                if (m_line == null)
                    return "";
                return m_line;
            }
        }
    }
}
