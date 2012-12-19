using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using OpenVPNUtils.States;

namespace OpenVPNUtils
{
    /// <summary>
    /// Provides access to OpenVPN.
    /// </summary>
    public abstract class Connection : IDisposable
    {
        #region variables
        /// <summary>
        /// The management logic used to communicate with OpenVPN.
        /// </summary>
        private ManagementLogic m_ovpnMLogic;

        /// <summary>
        /// The log manager.
        /// </summary>
        private LogManager m_logs;

        /// <summary>
        /// Dont raise events anymore, used on dispose().
        /// </summary>
        private bool m_noevents;

        /// <summary>
        /// The connection and vpn state.
        /// </summary>
        private State m_state;
        #endregion

        #region events
        /// <summary>
        /// Asks for a SmartCard ID to use.
        /// </summary>
        public event EventHandler<NeedCardIdEventArgs> NeedCardId;

        /// <summary>
        /// Asks for a Password.
        /// </summary>
        public event EventHandler<NeedPasswordEventArgs> NeedPassword;

        /// <summary>
        /// Holds the logfile path of the OpenVPN Process
        /// </summary>
        protected string m_logFile;

        /// <summary>
        /// Asks for a Username and Password.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public event EventHandler<NeedLoginAndPasswordEventArgs> NeedLoginAndPassword;
        #endregion

        #region initialization
        /// <summary>
        /// Initializes a new OVPN Object.
        /// </summary>
        /// <param name="host">Host to connect to which holds the management interface</param>
        /// <param name="port">Port to connect to</param>
        /// <param name="earlyLogEvent">Delegate to a event processor</param>
        /// <param name="earlyLogLevel">Log level</param>
        /// <param name="receiveOldLogs">Should old log lines be received?</param>
        /// <seealso cref="Logs"/>
        protected void Init(string host, int port,
            EventHandler<LogEventArgs> earlyLogEvent,
                int earlyLogLevel, bool receiveOldLogs)
        {
            this.Host = host;
            this.Port = port;
            m_state = new State(this);

            m_logs = new LogManager(this);
            m_logs.DebugLevel = earlyLogLevel;
            if (earlyLogEvent != null)
                m_logs.LogEvent += earlyLogEvent;

            m_ovpnMLogic = new ManagementLogic(this, host, port, m_logs, receiveOldLogs);
            m_state.ChangeState(VPNConnectionState.Stopped);
        }
        #endregion

        #region propertys

        /// <summary>
        /// Host of the management interface
        /// </summary>
        public string Host
        {
            get;
            private set;
        }

        /// <summary>
        /// Port of the used management interface
        /// </summary>
        public int Port
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the LogManager.
        /// </summary>
        public LogManager Logs
        {
            get { return m_logs; }
        }

        /// <summary>
        /// Get the internal state.
        /// </summary>
        public State State
        {
            get { return m_state; }
        }

        /// <summary>
        /// The IP of this endpoint, or null if unknown/unset
        /// </summary>
        public string IP
        {
            get;
            internal set;
        }

        /// <summary>
        /// Suppress events. This is used when disposing.
        /// </summary>
        internal bool NoEvents
        {
            get { return m_noevents; }
        }

        internal ManagementLogic Logic
        {
            get { return m_ovpnMLogic; }
        }
        #endregion

        #region abstract methods
        /// <summary>
        /// Connects with the configured parameters.
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// Closes the connection
        /// </summary>
        public abstract void Disconnect();
        #endregion

        #region protected methods

        /// <summary>
        /// Reads the used log files in a given vpn config file
        /// </summary>
        /// <param name="config">the file to read</param>
        /// <returns>the log file, <code>null</code> if none is specified</returns>
        protected string getLogFile(string config)
        {
            ConfigParser parser = new ConfigParser(config);
            string[] logFiles = parser.GetValue("log");
            if (logFiles == null)
            {
                return null;
            }
            else
            {
                return logFiles[1];
            }
        }

        /// <summary>
        /// Checks wheather the requested state can be reached. If not, the methods throws an error.
        /// </summary>
        /// <param name="newState"></param>
        protected void CheckState(VPNConnectionState newState)
        {
            switch (newState)
            {
                case VPNConnectionState.Initializing:
                    if (m_state.ConnectionState != VPNConnectionState.Stopped &&
                        m_state.ConnectionState != VPNConnectionState.Error)
                        throw new InvalidOperationException("Already connected");

                    break;
            }
        }

        /// <summary>
        /// Connect the management logic to OpenVPN
        /// </summary>
        protected bool ConnectLogic()
        {
            m_ovpnMLogic.reset();
            for(int i = 0; i < 8; ++i) {
                try
                {
                    System.Threading.Thread.Sleep(500);
                    m_ovpnMLogic.connect();
                    
                    return true;
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    m_logs.logDebugLine(1, "Could not establish connection " +
                        "to management interface:" + ex.Message);

                    if(m_state.ConnectionState != VPNConnectionState.Initializing)
                        return false;

                    if (i != 8)
                    {
                        m_logs.logDebugLine(1, "Trying again in a second");
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }

            if (m_state.ConnectionState != VPNConnectionState.Initializing)
                return false;

            m_logs.logDebugLine(1, "Could not establish connection, abording");
            m_state.ConnectionState = VPNConnectionState.Running;
            Disconnect();

            while (m_state.ConnectionState != VPNConnectionState.Stopped)
                Thread.Sleep(200);

            m_state.ChangeState(VPNConnectionState.Error);
            return false;
        }

        /// <summary>
        /// disconnect the management logic
        /// </summary>
        protected void DisconnectLogic()
        {
            m_ovpnMLogic.disconnect();
        }

        /// <summary>
        /// The log file of the process
        /// </summary>
        public string LogFile
        {
            get { return m_logFile; }
        }

        #endregion

        #region internal methods

        /// <summary>
        /// we need a key id, raise an event to fetch it
        /// </summary>
        /// <param name="pkcs11d">available keys</param>
        /// <returns>id of the key to use</returns>
        internal int getKeyID(List<PKCS11Detail> pkcs11d)
        {
            if (NoEvents) 
                return NeedCardIdEventArgs.None;

            m_logs.logDebugLine(1, "Asking user for PKCS11 Token");
            NeedCardIdEventArgs args = 
                new NeedCardIdEventArgs(pkcs11d.ToArray());

            if(NeedCardId != null)
                NeedCardId(this, args);
            return args.SelectedId;
        }

        /// <summary>
        /// We need a password, raise an event to fetch it.
        /// </summary>
        /// <param name="pwType">
        /// name of the password (e.g. 'private key')
        /// </param>
        /// <returns>the given password, null if none</returns>
        internal string getPW(string pwType)
        {
            if (NoEvents) return null;

            m_logs.logDebugLine(1, "Asking user for password \"" + pwType + "\"");
            NeedPasswordEventArgs args =
                new NeedPasswordEventArgs(pwType);

            if(NeedPassword != null)
                NeedPassword(this, args);
            else
                return null;
            return args.Password;
        }
        
        /// <summary>
        /// We need a username and a password, raise an event to fetch it.
        /// </summary>
        /// <param name="pwType">
        /// name of the username/password (e.g. 'Auth')
        /// </param>
        /// <returns>the given username and password, null if none</returns>
        internal string[] getLoginPass(string pwType)
        {
            if (NoEvents) return null;

            m_logs.logDebugLine(1, "Asking user for username and password \"" + pwType + "\"");
            NeedLoginAndPasswordEventArgs args =
                new NeedLoginAndPasswordEventArgs(pwType);

            if(NeedLoginAndPassword != null)
                NeedLoginAndPassword(this, args);
            else
                return null;

            return new string[] { args.UserName, args.Password };
        }

        internal void error()
        {
            m_state.ConnectionState = VPNConnectionState.Error;
            DisconnectLogic();
            m_state.ConnectionState = VPNConnectionState.Stopped;
            m_state.ChangeState(VPNConnectionState.Error);
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Is the object disposed?
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Destructor. Disposes the object.
        /// </summary>
        ~Connection()
        {
            m_noevents = true;
            Dispose(false);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor. Disposes the object.
        /// </summary>
        /// <param name="disposing">true if called from Dispose()</param>
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (m_state != null && m_state.ConnectionState != VPNConnectionState.Stopped)
                    Disconnect();

                if (disposing && m_ovpnMLogic != null)
                {
                    m_ovpnMLogic.Dispose();
                }
                m_logs = null;
                m_ovpnMLogic = null;
                disposed = true;
            }
        }

        #endregion
    }
}