using System.Collections.Generic;
using System.Threading;

namespace OpenVPN
{
    /// <summary>
    /// The logic which decides what to send to the management interface
    /// and what to ask the user for, and when to do it.
    /// </summary>
    internal class OVPNManagementLogic
    {
        #region enums
        /// <summary>
        /// Structure that tells what we are waiting for.
        /// </summary>
        public enum WaitState
        {
            /// <summary>
            /// We are waiting for nothing.
            /// </summary>
            NULL,

            /// <summary>
            /// We requested the number of found SmartCards.
            /// </summary>
            PKCS11_GET_COUNT,

            /// <summary>
            /// We requested a SmartCard key.
            /// </summary>
            PKCS11_GET_KEYS,

            /// <summary>
            /// We sent "log on".
            /// </summary>
            LOG_ON_ALL_1,

            /// <summary>
            /// After a "log on", we received a "ok".
            /// </summary>
            LOG_ON_ALL_2,

            /// <summary>
            /// After a "log on" and a "ok", we received the last log lines.
            /// </summary>
            LOG_ON,

            /// <summary>
            /// We requested a hold release.
            /// </summary>
            HOLD_RELEASE,

            /// <summary>
            /// We requested "state"
            /// </summary>
            STATE,

            /// <summary>
            /// We sent a signal
            /// </summary>
            SIGNAL
        }
        #endregion

        #region variables
        /// <summary>
        /// The parser used to "read" the answer.
        /// </summary>
        private OVPNManagementParser m_ovpnMParser;

        /// <summary>
        /// The communicator used to read and write from/to 
        /// the management interface of OpenVPN.
        /// </summary>
        private OVPNCommunicator m_ovpnComm;

        /// <summary>
        /// Log Manager used to log events.
        /// </summary>
        private OVPNLogManager m_logs;

        /// <summary>
        /// What are we waiting for at the moment?
        /// </summary>
        private WaitState m_state = WaitState.NULL;

        /// <summary>
        /// Asyncronous events to process.
        /// </summary>
        private List<AsyncEventDetail> m_todo = new List<AsyncEventDetail>();

        /// <summary>
        /// Parent.
        /// </summary>
        private OVPNConnection m_ovpn;

        /// <summary>
        /// Number of known SmartCards.
        /// </summary>
        private int m_pkcs11count;

        /// <summary>
        /// Details of the SmartCards.
        /// </summary>
        private List<PKCS11Detail> m_pkcs11details;
        #endregion

        #region constructor
        /// <summary>
        /// Creates a new ManagementLogic object.
        /// </summary>
        /// <param name="ovpn">parent</param>
        /// <param name="host">host to connect to (e.g. 127.0.0.1)</param>
        /// <param name="port">port to connect to</param>
        /// <param name="logs">LogManager to write the logs to</param>
        public OVPNManagementLogic(OVPNConnection ovpn, string host,
            int port, OVPNLogManager logs)
        {
            m_ovpn = ovpn;
            m_logs = logs;

            // initialize required components
            m_ovpnComm = new OVPNCommunicator(host, port, logs, ovpn);
            m_ovpnMParser = new OVPNManagementParser(m_ovpnComm, this, logs);
            m_pkcs11details = new List<PKCS11Detail>();
        }
        #endregion

        /// <summary>
        /// Connects to the management interface.
        /// </summary>
        public void connect()
        {
            m_ovpnComm.connect();
        }

        /// <summary>
        /// Disconnects from the managerment interface.
        /// </summary>
        public void sendQuit()
        {
            setLock(WaitState.SIGNAL);
            m_ovpnComm.quit();
            while (m_state == WaitState.SIGNAL && m_ovpnComm.isConnected())
                Thread.Sleep(200);
        }

        /// <summary>
        /// Closes the underlying connection.
        /// </summary>
        public void disconnect()
        {
            m_ovpnComm.disconnect();
        }

        /// <summary>
        /// Resets internal state.
        /// </summary>
        public void reset() {
            m_pkcs11count = 0;

            if(m_pkcs11details != null)
                m_pkcs11details.Clear();

            if(m_todo != null)
                m_todo.Clear();

            if (m_ovpnMParser != null)
                m_ovpnMParser.reset();

            m_state = WaitState.NULL;
        }

        /// <summary>
        /// Returns whether the management logic is connected
        /// </summary>
        /// <returns>true if the logic is connected, false otherwise</returns>
        public bool isConnected()
        {
            return m_ovpnComm.isConnected();
        }

        /// <summary>
        /// Acquires a lock. Waits, until the lock is possible.
        /// </summary>
        /// <param name="newState">state to set</param>
        private void setLock(WaitState newState)
        {
            setLock(newState, false);
        }

        /// <summary>
        /// Acquires a lock.
        /// </summary>
        /// <param name="newState">state to set</param>
        /// <param name="force">if true, don't wait for the lock but take it. This can be a risk!</param>
        private void setLock(WaitState newState, bool force)
        {
            if (force)
            {
                m_state = newState;
                return;
            }

            while (true)
            {
                try
                {
                    Monitor.Enter(this);
                    if (m_state == WaitState.NULL)
                    {
                        m_state = newState;
                        return;
                    }
                }
                finally
                {
                    Monitor.Exit(this);
                }

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Release the acquired lock.
        /// </summary>
        private void releaseLock()
        {
            try
            {
                Monitor.Enter(this);
                m_state = WaitState.NULL;
            }
            finally
            {
                Monitor.Exit(this);
            }

            if (m_todo.Count > 0)
            {
                AsyncEventDetail e;
                try{
                    Monitor.Enter(m_todo);
                    e = m_todo[0];
                    m_todo.Remove(e);
                }
                finally
                {
                    Monitor.Exit(m_todo);
                }

                got_asyncEvent(e);
            }
        }

        /// <summary>
        /// We got a synchronous message.
        /// </summary>
        /// <param name="msg">The message</param>
        public void cb_syncEvent(string msg)
        {
            // the reaction depends on what we are waiting for
            switch(m_state)
            {

                // the number of SmartCards
                case WaitState.PKCS11_GET_COUNT:
                    m_pkcs11count = m_ovpnMParser.getPKCS11IDCount(msg);
                    
                    if (m_pkcs11count == -1)
                    {
                        m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT,
                            "Could not determine the number of pkcs11-ids:\"" +
                            msg + "\"");
                        releaseLock();
                    }

                    else if (m_pkcs11count == 0)
                    {
                        m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT,
                            "No pkcs11-ids were found");

                        int id = m_ovpn.getKeyID(new List<PKCS11Detail>());
                        if (id == OVPNNeedCardIDEventArgs.RETRY)
                            m_ovpnComm.send("pkcs11-id-count");
                        else
                        {
                            releaseLock();
                        }
                    }
                    else
                    {
                        m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT,
                            "Got " + m_pkcs11count + " PKCS11-IDs");
                        m_pkcs11details.Clear();
                        releaseLock();

                        setLock(WaitState.PKCS11_GET_KEYS);
                        m_ovpnComm.send("pkcs11-id-get 0");
                    }

                    break;

                case WaitState.PKCS11_GET_KEYS:
                    PKCS11Detail d = m_ovpnMParser.getPKCS11ID(msg);

                    if (d != null)
                    {
                        m_pkcs11details.Add(d);

                        if (d.nr < m_pkcs11count - 1)
                            m_ovpnComm.send("pkcs11-id-get " + (d.nr + 1));

                        else
                        {
                            releaseLock();

                            int kid = m_ovpn.getKeyID(m_pkcs11details);

                            if (kid == OVPNNeedCardIDEventArgs.RETRY)
                            {
                                setLock(WaitState.PKCS11_GET_COUNT);
                                m_ovpnComm.send("pkcs11-id-count");
                            }
                            else if (kid != OVPNNeedCardIDEventArgs.NONE)
                                m_ovpnComm.send("needstr 'pkcs11-id-request' '" +
                                    m_pkcs11details[kid].id + "'");
                        }
                    }

                    // error in parsing
                    else
                    {
                        m_logs.logDebugLine(1, 
                            "Error while parsing pkcs11-id-get: \"" + 
                            msg + "\"");

                        releaseLock();
                    }
                    break;
                
                // logging was turned on, wait for log lines
                case WaitState.LOG_ON_ALL_1:
                    setLock(WaitState.LOG_ON_ALL_2, true);
                    break;

                // logging was turned on
                case WaitState.LOG_ON:
                case WaitState.LOG_ON_ALL_2:
                    releaseLock();

                    string[] m = msg.Split("\n".ToCharArray());
                    for (int i = 0; i < m.GetUpperBound(0) - 1; ++i)
                        m_logs.logLine(OVPNLogEventArgs.LogType.LOG, m[i]);

                    setLock(WaitState.STATE);
                    m_ovpnComm.send("state on");
                    break;

                // "state" was set
                case WaitState.STATE:
                    releaseLock();
                    setLock(WaitState.HOLD_RELEASE);
                    m_ovpnComm.send("hold release");

                    break;

                // hold relese was executed
                case WaitState.HOLD_RELEASE:
                    releaseLock();
                    break;

                // we sent a signal
                case WaitState.SIGNAL:
                    releaseLock();
                    break;

                // something else happened (this should not happen)
                // we release the lock
                default:
                    releaseLock();
                    break;
            }
        }

        /// <summary>
        /// We got an asynchronous event (e.g. a log message).
        /// </summary>
        /// <param name="aeDetail">details about the event</param>
        public void got_asyncEvent(AsyncEventDetail aeDetail)
        {
            m_logs.logDebugLine(4, "Extracted async event: " + 
                aeDetail.eventType.ToString() + ": " + aeDetail.message);

            // if we can't execute just queue it
            if (m_state != WaitState.NULL)
                try
                {
                    Monitor.Enter(m_todo);
                    m_todo.Add(aeDetail);
                    return;
                }
                finally
                {
                    Monitor.Exit(m_todo);
                }
            
            switch (aeDetail.eventType)
            {
                case AsyncEventDetail.EventType.NEEDSTR:

                    switch (aeDetail.getInfos()[0])
                    {

                        // A SmartCard ID is requested
                        case "pkcs11-id-request":
                            m_logs.logDebugLine(3, "Got Request for pkcs11-id");

                            setLock(WaitState.PKCS11_GET_COUNT);
                            m_ovpnComm.send("pkcs11-id-count");
                            break;
                    }
                        
                    break;

                // a password is requested
                case AsyncEventDetail.EventType.PASSWORD:
                    string pwType = aeDetail.getInfos()[0]; // "Auth" or "Private Key", or ...
                    string pwInfo = aeDetail.getInfos()[1]; // "password" or "username/password"
                    string pwMsg = aeDetail.getInfos()[2];  // "Need" or "Verification Failed"

                    if (pwMsg.Equals("Need"))
                    {
                        if (pwType.Equals("Auth") && pwInfo.Equals("username/password"))
                        {
                            // Ask for username/password
                            string[] loginInfo = m_ovpn.getLoginPass(pwType);
                            if (loginInfo != null)
                            {
                                string username = loginInfo[0];
                                string password = loginInfo[1];
                                if (username != null && pwType.Length > 0 &&
                                    password != null && password.Length > 0)
                                {
                                    m_ovpnComm.send("username '" + pwType + "' " +
                                            m_ovpnMParser.encodeMsg(username));
                                    m_ovpnComm.send("password '" + pwType + "' " +
                                            m_ovpnMParser.encodeMsg(password));
                                }
                            }

                        }
                        else
                        {
                            string pw = m_ovpn.getPW(pwType);
                            if (pw != null)
                            {
                                if (pw.Length > 0)
                                {
                                    m_ovpnComm.send("password '" + pwType + "' " +
                                        m_ovpnMParser.encodeMsg(pw));
                                }
                            }
                        }
                    }
                    else if (pwMsg.CompareTo("Verification Failed") == 0)
                    {
                        m_logs.logDebugLine(1, "Authentication Failed said remote server");
                    }
                    else
                    {
                        m_logs.logDebugLine(1, "Unknown 'PASSWORD' reply from remote server: " + pwMsg);
                    }

                    break;

                // a hold state is signalized
                case AsyncEventDetail.EventType.HOLD:
                    // it is released
                    if (aeDetail.message.Contains("Waiting"))
                    {
                        /* 
                         * enable logging
                         * (this is a trick to get all logs, because at the
                         * beginning, the hold state is set)
                         */
                        setLock(WaitState.LOG_ON_ALL_1);
                        m_ovpnComm.send("log on all");
                    }
                    break;

                case AsyncEventDetail.EventType.INFO:
                    break;

                // the internal state changed
                case AsyncEventDetail.EventType.STATE:
                    m_ovpn.changeVPNState(aeDetail.getInfos());
                    m_logs.logLine(OVPNLogEventArgs.LogType.STATE,
                        aeDetail.getInfos()[1]);
                    break;

                // we got a "log"
                case AsyncEventDetail.EventType.LOG:
                    string[] parts = aeDetail.message.Split(new char[] {','}, 3);
                    long time = 0;
                    long.TryParse(parts[0], out time);

                    m_logs.logLine(OVPNLogEventArgs.LogType.LOG,
                        parts[2], time);
                    break;
            }
        }
    }
}
