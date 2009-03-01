using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Collections.ObjectModel;

namespace OpenVPN
{
    /// <summary>
    /// state of the VPN
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VPN")]
    public enum VPNConnectionState
    {
        /// <summary>
        /// OpenVPN is starting up.
        /// </summary>
        Initializing,

        /// <summary>
        /// OpenVPN is up and running.
        /// </summary>
        Running,

        /// <summary>
        /// OpenVPN is shutting down.
        /// </summary>
        Stopping,

        /// <summary>
        /// OpenVPN has stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// OpenVPN had an error while communicating with OpenVPN.
        /// </summary>
        Error
    }

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
        /// State of the whole VPN Object.
        /// </summary>
        private VPNConnectionState m_state;

        /// <summary>
        /// Dont raise events anymore, used on dispose().
        /// </summary>
        private bool m_noevents;

        /// <summary>
        /// internal state of openvpn
        /// </summary>
        private string[] m_vpnstate = new string[] {"", "", "", ""};

        /// <summary>
        /// Saves the IP of the VPN-endpoint.
        /// </summary>
        private string m_ip;
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
        /// Asks for a Username and Password.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public event EventHandler<NeedLoginAndPasswordEventArgs> NeedLoginAndPassword;

        /// <summary>
        /// Signals, that the state has changed.
        /// </summary>
        public event EventHandler ConnectionStateChanged;

        /// <summary>
        /// Signals, that the state of vpn has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VPN")]
        public event EventHandler VPNStateChanged;
        #endregion

        #region constructors/destructors

        /// <summary>
        /// Initializes a new OVPN Object.
        /// </summary>
        /// <param name="earlyLogEvent">Delegate to a event processor</param>
        /// <param name="earlyLogLevel">Log level</param>
        /// <seealso cref="Logs"/>
        protected Connection(EventHandler<LogEventArgs> earlyLogEvent, 
            int earlyLogLevel)
            : this("127.0.0.1", 11194, earlyLogEvent, earlyLogLevel)
        {
        }

        /// <summary>
        /// Initializes a new OVPN Object.
        /// </summary>
        /// <param name="host">Host to connect to which holds the management interface</param>
        /// <param name="port">Port to connect to</param>
        /// <param name="earlyLogEvent">Delegate to a event processor</param>
        /// <param name="earlyLogLevel">Log level</param>
        /// <seealso cref="Logs"/>
        protected Connection(string host, int port,
            EventHandler<LogEventArgs> earlyLogEvent,
                int earlyLogLevel)
        {
            this.Host = host;
            this.Port = port;

            m_logs = new LogManager(this);
            m_logs.DebugLevel = earlyLogLevel;
            if (earlyLogEvent != null)
                m_logs.LogEvent += earlyLogEvent;

            m_ovpnMLogic = new ManagementLogic(this, host, port, m_logs);
            changeState(VPNConnectionState.Stopped);
        }
        #endregion

        #region propertys

        /// <summary>
        /// Host of the management interface
        /// </summary>
        public string Host
        {
            get;
            protected set;
        }

        /// <summary>
        /// Port of the used management interface
        /// </summary>
        public int Port
        {
            get;
            protected set;
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
        public VPNConnectionState State
        {
            get { return m_state; }
        }

        /// <summary>
        /// The IP of this endpoint, or null if unknown/unset
        /// </summary>
        public string IP
        {
            get { return m_ip; }
        }

        /// <summary>
        /// gets the last state that was reported by the opvnvpn process
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VPN")]
        public ReadOnlyCollection<String> VPNState
        {
            get
            {
                return new ReadOnlyCollection<String>(m_vpnstate);
            }
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
        /// Checks wheather the requested state can be reached. If not, the methods throws an error.
        /// </summary>
        /// <param name="newState"></param>
        protected void CheckState(VPNConnectionState newState)
        {
            switch (newState)
            {
                case VPNConnectionState.Initializing:
                    if (m_state != VPNConnectionState.Stopped &&
                        m_state != VPNConnectionState.Error)
                        throw new InvalidOperationException("Already connected");

                    break;
                case VPNConnectionState.Stopping:
                    if (m_state == VPNConnectionState.Initializing)
                        throw new InvalidOperationException("Can't disconnect and connect at the same time");
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
                    System.Threading.Thread.Sleep(500); // TODO: 500
                    m_ovpnMLogic.connect();
                    
                    return true;
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    m_logs.logDebugLine(1, "Could not establish connection " +
                        "to management interface:" + ex.Message);
                    if (i != 5)
                    {
                        m_logs.logDebugLine(1, "Trying again in a second");
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
            m_logs.logDebugLine(1, "Could not establish connection, abording");
            m_state = VPNConnectionState.Running;
            Disconnect();

            while (m_state != VPNConnectionState.Stopped)
                Thread.Sleep(200);

            changeState(VPNConnectionState.Error);
            return false;
        }

        /// <summary>
        /// disconnect the management logic
        /// </summary>
        protected void DisconnectLogic()
        {
            m_ovpnMLogic.disconnect();
        }

        #endregion

        #region internal methods

        /// <summary>
        /// change the state of the class
        /// </summary>
        /// <param name="newstate">new state</param>
        internal void changeState(VPNConnectionState newstate)
        {
            if (m_state != newstate)
            {
                m_state = newstate;
            
                if(ConnectionStateChanged != null && !NoEvents)
                    ConnectionStateChanged(this, new EventArgs());
            }
        }

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

        internal void changeVPNState(string[] p)
        {
            Array.Copy(p, m_vpnstate, 4);

            if (p[1] == "CONNECTED" || p[1] == "ASSIGN_IP")
            {
                m_ip = p[3];

                if (p[1] == "CONNECTED")
                    changeState(VPNConnectionState.Running);
            }
            else if (p[1] == "EXITING")
            {
                m_ip = null;
            }
            
            if(VPNStateChanged != null && !NoEvents)
                VPNStateChanged(this, new EventArgs());
        }

        internal void error()
        {
            m_state = VPNConnectionState.Error;
            DisconnectLogic();
            m_state = VPNConnectionState.Stopped;
            changeState(VPNConnectionState.Error);
        }
        #endregion

        #region IDisposable Members

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
                if (State != VPNConnectionState.Stopped)
                    Disconnect();

                if (disposing)
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