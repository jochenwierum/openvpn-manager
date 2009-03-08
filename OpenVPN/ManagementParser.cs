using System;
using System.Globalization;
using System.Text;

namespace OpenVPN
{
    // http://openvpn.net/index.php/documentation/miscellaneous/management-interface.html
    /// <summary>
    /// Class which parses received data from the management interface.
    /// </summary>
    internal class ManagementParser
    {
        #region variables
        /// <summary>
        /// Management logic which answers the requests.
        /// </summary>
        private ManagementLogic m_logic;

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
        internal ManagementParser(Communicator oc, ManagementLogic ol)
        {
            m_logic = ol;
            oc.gotLine += new helper.Action<object,GotLineEventArgs>(oc_gotLine);
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
            string s = e.line;

            // some lines start with a ">" (see link above)
            // they come asynchron and are parsed imediately
            if (s.StartsWith(">", StringComparison.OrdinalIgnoreCase))
            {
                string type = s.Substring(1, s.IndexOf(":", 
                    StringComparison.OrdinalIgnoreCase) - 1);
                string msg = s.Substring(type.Length + 2);

                string[] infos = null;
                AsyncEventDetail.EventType et = AsyncEventDetail.EventType.UNKNOWN;

                switch (type)
                {
                    case "ECHO": et = AsyncEventDetail.EventType.ECHO; break;
                    case "FATAL": et = AsyncEventDetail.EventType.FATAL; break;
                    case "HOLD": et = AsyncEventDetail.EventType.HOLD; break;
                    case "INFO": et = AsyncEventDetail.EventType.INFO; break;
                    case "LOG": et = AsyncEventDetail.EventType.LOG; break;

                    case "NEED-STR":
                        et = AsyncEventDetail.EventType.NEEDSTR;

                        string tmp = msg.Substring(msg.IndexOf('\'') + 1);
                        infos = new string[] {tmp.Substring(0,tmp.IndexOf('\''))};
                        break;

                    case "STATE":
                        et = AsyncEventDetail.EventType.STATE;
                        infos = msg.Split(new char[] { ',' });
                        break;

                    case "PASSWORD": 
                        et = AsyncEventDetail.EventType.PASSWORD;
                        // Several messages format are possible
                        // * first is a request for a passwd
                        //   >PASSWORD:Need 'Auth' username/password
                        // or
                        //   >PASSWORD:Need 'Private Key' password
                        //
                        // * second is a notification
                        //   >PASSWORD:Verification Failed: 'Auth'
                        // or
                        //   >PASSWORD:Verification Failed: 'Private Key'
                                        
                        // Let's first determine the PASSWORD message type and thus format
                        // "Need" or "Verification"
                        if(msg.StartsWith("Need", 
                            StringComparison.OrdinalIgnoreCase))
                        {
                            string tmp2 = msg.Substring(msg.IndexOf('\'') + 1);
                            string loginProfile = tmp2.Substring(0, tmp2.IndexOf('\'')); // 'Auth' or 'Private Key' or ...
                            string loginInfo = tmp2.Substring(tmp2.IndexOf('\'') + 2); // "password" or "username/password"
                            infos = new string[] { loginProfile, loginInfo, "Need" };
                        }
                        // "Verification Failed"
                        else if(msg.StartsWith("Verification Failed:", 
                                StringComparison.OrdinalIgnoreCase))
                        {
                            string tmp2 = msg.Substring(msg.IndexOf('\'') + 1);
                            string loginProfile = tmp2.Substring(0, tmp2.IndexOf('\'')); // 'Auth' or 'Private Key' or ...
                            infos = new string[] { loginProfile, null, "Verification" };
                        }
                        break;
                }

                if (et != AsyncEventDetail.EventType.UNKNOWN)
                {
                    m_logic.got_asyncEvent(new AsyncEventDetail(et, msg, infos));
                    return;
                }
            }

            m_received.Append(s + Environment.NewLine);
            s = m_received.ToString();

            if (s.StartsWith("SUCCESS: ", StringComparison.OrdinalIgnoreCase)
                || s.StartsWith("ERROR: ", StringComparison.OrdinalIgnoreCase)
                || s.StartsWith(">", StringComparison.OrdinalIgnoreCase) || 
                s.EndsWith("END" + Environment.NewLine, StringComparison.OrdinalIgnoreCase))
            {
                m_received.Remove(0, m_received.Length);
                m_logic.cb_syncEvent(s);
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
        public static int getPKCS11IDCount(string s) {
            if (s.StartsWith(">PKCS11ID-COUNT:", StringComparison.OrdinalIgnoreCase))
            {
                s = s.Substring(16);
                return int.Parse(s, CultureInfo.InvariantCulture.NumberFormat);
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
        public static PKCS11Detail getPKCS11ID(string s) {

            // get the essential parts
            string[] parts = s.Split(new Char[] { ',' });

            int nr;
            string id;
            string blob;

            // try to get the number, id and blob
            try
            {
                if (parts[0].StartsWith(">PKCS11ID-ENTRY:'", 
                    StringComparison.OrdinalIgnoreCase))
                    nr = int.Parse(parts[0].Substring(17, parts[0].Length - 18),
                        CultureInfo.InvariantCulture.NumberFormat);
                else
                    return null;

                if (parts[1].StartsWith(" ID:'", 
                    StringComparison.OrdinalIgnoreCase))
                    id = parts[1].Substring(5, parts[1].Length - 6);
                else
                    return null;

                if (parts[2].StartsWith(" BLOB:'", 
                    StringComparison.OrdinalIgnoreCase))
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
        public static string encodeMsg(string s)
        {
            return "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }
    }
}
