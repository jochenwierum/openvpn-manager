using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OpenVPN
{
    // http://openvpn.net/index.php/documentation/miscellaneous/management-interface.html
    /// <summary>
    /// Class which parses received data from the management interface.
    /// </summary>
    internal class OVPNManagementParser
    {
        #region variables
        /// <summary>
        /// Object which manages logs.
        /// </summary>
        private OVPNLogManager m_logs;

        /// <summary>
        /// Management logic which answers the requests.
        /// </summary>
        private OVPNManagementLogic m_ol;

        /// <summary>
        /// Buffer which holds received lines.
        /// </summary>
        private StringBuilder m_received = new StringBuilder();
        #endregion

        /// <summary>
        /// Creates a new management parser.
        /// </summary>
        /// <param name="oc">reference to the network communicator</param>
        /// <param name="ol">reference to the management logic</param>
        /// <param name="logs">reference to the log interface</param>
        internal OVPNManagementParser(OVPNCommunicator oc, OVPNManagementLogic ol, OVPNLogManager logs)
        {
            // copy data
            m_logs = logs;
            m_ol = ol;

            // receive new data
            oc.gotLine += new OVPNCommunicator.GotLineEvent(oc_gotLine);
        }

        /// <summary>
        /// a line was received, this method parses it and calls methods
        /// in the management logic.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">information about the received line</param>
        /// <remarks>
        /// This method is not thread save!
        /// But there is no need to call it from more than one thread.
        /// Only the thread which holds the management connection calls this event.
        /// </remarks>
        private void oc_gotLine(object sender, GotLineEventArgs e)
        {
            // drop a line
            m_logs.logDebugLine(10, "Got: " + e.line);

            string s = e.line;

            // some lines start with a ">" (see link above)
            // they come asynchron and are parsed imediately
            if (s.StartsWith(">"))
            {
                // extract the type and the message
                string type = s.Substring(1, s.IndexOf(":")-1);
                string msg = s.Substring(type.Length + 2);

                string[] infos = null;
                AsyncEventDetail.EventType et = AsyncEventDetail.EventType.UNKNOWN;

                // set the type
                switch (type)
                {
                    case "ECHO": et = AsyncEventDetail.EventType.ECHO; break;
                    case "FATAL": et = AsyncEventDetail.EventType.FATAL; break;
                    case "HOLD": et = AsyncEventDetail.EventType.HOLD; break;
                    case "INFO": et = AsyncEventDetail.EventType.INFO; break;
                    case "LOG": et = AsyncEventDetail.EventType.LOG; break;
                    case "STATE": et = AsyncEventDetail.EventType.STATE; break;

                    case "NEED-STR":
                        et = AsyncEventDetail.EventType.NEEDSTR;

                        string tmp = msg.Substring(msg.IndexOf('\'') + 1);
                        infos = new string[] {tmp.Substring(0,tmp.IndexOf('\''))};
                        break;

                    case "PASSWORD": 
                        et = AsyncEventDetail.EventType.PASSWORD;

                        string tmp2 = msg.Substring(msg.IndexOf('\'') + 1);
                        infos = new string[] { tmp2.Substring(0, tmp2.IndexOf('\'')) };
                        break;
                }

                // if it was an important event, send a signal
                if (et != AsyncEventDetail.EventType.UNKNOWN)
                {
                    m_ol.got_asyncEvent(new AsyncEventDetail(et, msg, infos));
                    return;
                }
            }

            // save the line
            m_received.Append(s + Environment.NewLine);
            s = m_received.ToString();

            // if the message is complete, raise an event and remove it
            if (s.StartsWith("SUCCESS: ") || s.StartsWith("ERROR: ") || 
                s.StartsWith(">") || s.EndsWith("END" + Environment.NewLine))
            {
                m_received.Remove(0, m_received.Length);
                m_ol.cb_syncEvent(s);
            }
        }

        /// <summary>
        /// Clear the safed lines.
        /// </summary>
        public void reset()
        {
            m_received.Remove(0, m_received.Length);
        }

        /// <summary>
        /// Parse an PKCS11ID-COUNT answer.
        /// </summary>
        /// <param name="s">the received line</param>
        /// <returns>number of keys present, -1 on errors</returns>
        public int getPKCS11IDCount(string s) {
            if (s.StartsWith(">PKCS11ID-COUNT:"))
            {
                s = s.Substring(16);
                return int.Parse(s);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Extract a PKCS11Detail from a given answer.
        /// </summary>
        /// <param name="s">string which the server sent</param>
        /// <returns>a PKCS11Detail structure which holds the extracted information</returns>
        public PKCS11Detail getPKCS11ID(string s) {

            // get the essential parts
            string[] parts = s.Split(new Char[] { ',' });

            int nr;
            string id;
            string blob;

            // try to get the number, id and blob
            try
            {
                if (parts[0].StartsWith(">PKCS11ID-ENTRY:'"))
                    nr = int.Parse(parts[0].Substring(17, parts[0].Length - 18));
                else
                    return null;

                if (parts[1].StartsWith(" ID:'"))
                    id = parts[1].Substring(5, parts[1].Length - 6);
                else
                    return null;

                if (parts[2].StartsWith(" BLOB:'"))
                    blob = parts[2].Substring(7, parts[2].Length - 10);
                else
                    return null;

                // return the extracted data
                return new PKCS11Detail(nr, id, blob);
            }
            catch (FormatException)
            {
                // the format had a problem;
                return null;
            }
        }

        /// <summary>
        /// encode an parameter for an answer
        /// (this is needed if it countains spaces)
        /// </summary>
        /// <param name="s">the parameter</param>
        /// <returns>the encoded parameter</returns>
        public string encodeMsg(string s)
        {
            return "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }
    }
}
