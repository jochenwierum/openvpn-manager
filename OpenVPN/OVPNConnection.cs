using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace OpenVPN
{
    /// <summary>
    /// Provides access to OpenVPN.
    /// </summary>
    public abstract class OVPNConnection
    {

        #region variables
        /// <summary>
        /// The management logic used to communicate with OpenVPN.
        /// </summary>
        private OVPNManagementLogic m_ovpnMLogic;

        /// <summary>
        /// The log manager.
        /// </summary>
        private OVPNLogManager m_logs;

        /// <summary>
        /// State of the whole VPN Object.
        /// </summary>
        private OVPNState m_state;

        /// <summary>
        /// Dont raise events anymore, used on dispose().
        /// </summary>
        private bool m_noevents = false;

        /// <summary>
        /// internal state of openvpn
        /// </summary>
        private string[] m_vpnstate = new string[] {"", "", "", ""};

        /// <summary>
        /// Port to connect to
        /// </summary>
        private int m_port;

        /// <summary>
        /// Host to connect to (normally 127.0.0.1)
        /// </summary>
        private string m_host;
        #endregion

        #region enums
        /// <summary>
        /// state of the VPN
        /// </summary>
        public enum OVPNState
        {
            /// <summary>
            /// OpenVPN is starting up.
            /// </summary>
            INITIALIZING,

            /// <summary>
            /// OpenVPN is up and running.
            /// </summary>
            RUNNING,

            /// <summary>
            /// OpenVPN is shutting down.
            /// </summary>
            STOPPING,

            /// <summary>
            /// OpenVPN has stopped.
            /// </summary>
            STOPPED,

            /// <summary>
            /// OpenVPN had an error while communicating with OpenVPN.
            /// </summary>
            ERROR
        }
        #endregion

        #region events
        /// <summary>
        /// Delegate to a method which selects a SmartCard.
        /// </summary>
        /// <param name="sender">The OVPN objects which asks.</param>
        /// <param name="e">An instance of OVPNNeedCardIDEventArgs which holds all found SmartCards.</param>
        public delegate void NeedCardIDEventDelegate(object sender, OVPNNeedCardIDEventArgs e);

        /// <summary>
        /// Asks for a SmartCard ID to use.
        /// </summary>
        public event NeedCardIDEventDelegate needCardID;

        /// <summary>
        /// Delegate to a method which sets a password.
        /// </summary>
        /// <param name="sender">The OVPN object which asks.</param>
        /// <param name="e">
        ///     An instance of OVPNNeedPasswordEventArgs
        ///     which holds information about the required password
        /// </param>
        public delegate void NeedPasswordEventDelegate(object sender, OVPNNeedPasswordEventArgs e);

        /// <summary>
        /// Asks for a Password.
        /// </summary>
        public event NeedPasswordEventDelegate needPassword;
        
        /// <summary>
        /// Delegate to a method which sets a username and a password.
        /// </summary>
        /// <param name="sender">The OVPN object which asks.</param>
        /// <param name="e">
        ///     An instance of OVPNNeedLoginAndPasswordEventArgs
        ///     which holds information about the required password
        /// </param>
        public delegate void NeedLoginAndPasswordEventDelegate(object sender, OVPNNeedLoginAndPasswordEventArgs e);

        /// <summary>
        /// Asks for a Username and Password.
        /// </summary>
        public event NeedLoginAndPasswordEventDelegate needLoginAndPassword;

        /// <summary>
        /// Signals, that the state has changed.
        /// </summary>
        public event EventHandler stateChanged;

        /// <summary>
        /// Signals, that the state of vpn has changed.
        /// </summary>
        public event EventHandler vpnStateChanged;

        /// <summary>
        /// Saves the IP of the VPN-endpoint.
        /// </summary>
        private string m_ip = null;
        #endregion

        #region constructors/destructors

        /// <summary>
        /// Initializes a new OVPN Object.
        /// </summary>
        /// <param name="earlyLogEvent">Delegate to a event processor</param>
        /// <param name="earlyLogLevel">Log level</param>
        /// <seealso cref="logs"/>
        public OVPNConnection(OVPNLogManager.LogEventDelegate earlyLogEvent, int earlyLogLevel)
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
        /// <seealso cref="logs"/>
        public OVPNConnection(string host, int port, OVPNLogManager.LogEventDelegate earlyLogEvent, int earlyLogLevel)
        {
            m_host = host;
            m_port = port;

            m_logs = new OVPNLogManager(this);
            m_logs.debugLevel = earlyLogLevel;
            if (earlyLogEvent != null)
                m_logs.LogEvent += earlyLogEvent;

            m_ovpnMLogic = new OVPNManagementLogic(this, host, port, m_logs);
            changeState(OVPNState.STOPPED);
        }

        /// <summary>
        /// Called when the object is unloaded.
        /// </summary>
        ~OVPNConnection()
        {
            m_noevents = true;
            if(m_state != OVPNState.STOPPED)
                disconnect();
        }
        #endregion

        #region propertys

        /// <summary>
        /// Host of the management interface
        /// </summary>
        public string host
        {
            get { return m_host; }
            protected set { m_host = value; }
        }

        /// <summary>
        /// Port of the used management interface
        /// </summary>
        public int port
        {
            get { return m_port; }
            protected set { m_port = value; }
        }

        /// <summary>
        /// Get the LogManager.
        /// </summary>
        public OVPNLogManager logs
        {
            get { return m_logs; }
        }

        /// <summary>
        /// Get the internal state.
        /// </summary>
        public OVPNState state
        {
            get { return m_state; }
        }

        /// <summary>
        /// The IP of this endpoint, or null if unknown/unset
        /// </summary>
        public string ip
        {
            get { return m_ip; }
        }

        /// <summary>
        /// gets the last state that was reported by the opvnvpn process
        /// </summary>
        public string[] vpnState
        {
            get
            {
                return m_vpnstate;
            }
        }

        /// <summary>
        /// Suppress events. This is used when disposing.
        /// </summary>
        internal bool noevents
        {
            get { return m_noevents; }
        }

        internal OVPNManagementLogic logic
        {
            get { return m_ovpnMLogic; }
        }
        #endregion

        #region abstract methods
        /// <summary>
        /// Connects with the configured parameters.
        /// </summary>
        public abstract void connect();

        /// <summary>
        /// Closes the connection
        /// </summary>
        public abstract void disconnect();
        #endregion

        #region protected methods

        /// <summary>
        /// Checks wheather the requested state can be reached. If not, the methods throws an error.
        /// </summary>
        /// <param name="newState"></param>
        protected void checkState(OVPNState newState)
        {
            switch (newState)
            {
                case OVPNState.INITIALIZING:
                    if (m_state != OVPNState.STOPPED &&
                        m_state != OVPNState.ERROR)
                        throw new InvalidOperationException("Already connected");

                    break;
                case OVPNState.STOPPING:
                    if (m_state == OVPNState.INITIALIZING)
                        throw new InvalidOperationException("Can't disconnect and connect at the same time");
                    break;
            }
        }

        /// <summary>
        /// Connect the management logic to OpenVPN
        /// </summary>
        protected bool connectLogic()
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
            m_state = OVPNState.RUNNING;
            disconnect();

            while (m_state != OVPNState.STOPPED)
                Thread.Sleep(200);

            changeState(OVPNState.ERROR);
            return false;
        }

        /// <summary>
        /// disconnect the management logic
        /// </summary>
        protected void disconnectLogic()
        {
            m_ovpnMLogic.disconnect();
        }

        #endregion

        #region internal methods

        /// <summary>
        /// change the state of the class
        /// </summary>
        /// <param name="newstate">new state</param>
        internal void changeState(OVPNState newstate)
        {
            if (m_state != newstate)
            {
                m_state = newstate;
            
                if(stateChanged != null && !noevents)
                    stateChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// we need a key id, raise an event to fetch it
        /// </summary>
        /// <param name="pkcs11d">available keys</param>
        /// <returns>id of the key to use</returns>
        internal int getKeyID(List<PKCS11Detail> pkcs11d)
        {
            if (noevents) 
                return OVPNNeedCardIDEventArgs.NONE;

            m_logs.logDebugLine(1, "Asking user for PKCS11 Token");
            OVPNNeedCardIDEventArgs args = 
                new OVPNNeedCardIDEventArgs(pkcs11d.ToArray());

            if(needCardID != null)
                needCardID(this, args);
            return args.selectedID;
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
            if (noevents) return null;

            m_logs.logDebugLine(1, "Asking user for password \"" + pwType + "\"");
            OVPNNeedPasswordEventArgs args =
                new OVPNNeedPasswordEventArgs(pwType);

            if(needPassword != null)
                needPassword(this, args);
            else
                return null;
            return args.password;
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
            if (noevents) return null;

            m_logs.logDebugLine(1, "Asking user for username and password \"" + pwType + "\"");
            OVPNNeedLoginAndPasswordEventArgs args =
                new OVPNNeedLoginAndPasswordEventArgs(pwType);

            if(needLoginAndPassword != null)
                needLoginAndPassword(this, args);
            else
                return null;

            return new string[] { args.username, args.password };
        }

        internal void changeVPNState(string[] p)
        {
            Array.Copy(p, m_vpnstate, 4);

            if (p[1] == "CONNECTED" || p[1] == "ASSIGN_IP")
            {
                m_ip = p[3];

                if (p[1] == "CONNECTED")
                    changeState(OVPNState.RUNNING);
            }
            else if (p[1] == "EXITING")
            {
                m_ip = null;
            }
            
            if(vpnStateChanged != null && !noevents)
                vpnStateChanged(this, new EventArgs());
        }

        internal void error()
        {
            m_state = OVPNState.ERROR;
            disconnectLogic();
            m_state = OVPNState.STOPPED;
            changeState(OVPNState.ERROR);
        }
        #endregion
    }
}