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
    public class OVPN
    {

        #region variables
        /// <summary>
        /// The OpenVPN binary service.
        /// </summary>
        private OVPNService m_ovpnService;

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
        /// Counts, how many objects have been created.
        /// </summary>
        private static int obj_count = 0;

        /// <summary>
        /// internal state of openvpn
        /// </summary>
        private string[] vpnstate = new string[] {"", "", "", ""};
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
        /// <param name="bin">Path to openvpn binary</param>
        /// <param name="config">Path to configuration file</param>
        /// <param name="logfile">File to write OpenVPN log messages to</param>
        public OVPN(string bin, string config, string logfile)
            : this(bin, config, logfile, null, 1)
        {
        }

        /// <summary>
        /// Initializes a new OVPN Object.
        /// Also set a LogEventDelegate so that the first log lines are reveived.
        /// </summary>
        /// <param name="bin">Path to openvpn binary</param>
        /// <param name="config">Path to configuration file</param>
        /// <param name="earlyLogEvent">Delegate to a event processor</param>
        /// <param name="earlyLogLevel">Log level</param>
        /// <param name="logfile">File to write OpenVPN log message to</param>
        /// <seealso cref="logs"/>
        public OVPN(string bin, string config, string logfile,
            OVPNLogManager.LogEventDelegate earlyLogEvent,
            int earlyLogLevel)
        {
            if (bin == null)
                throw new ArgumentNullException("Binary is null");
            if (config == null)
                throw new ArgumentNullException("Config file is null");
            if (!new FileInfo(bin).Exists)
                throw new FileNotFoundException("Binary \"" + bin + "\" does not exist");
            if (!new FileInfo(config).Exists)
                throw new FileNotFoundException("Config file \"" + config + "\" does not exist");

            string host = "127.0.0.1";
            int port = 11195 + obj_count++;

            // Initialize logging
            m_logs = new OVPNLogManager(this);
            m_logs.debugLevel = earlyLogLevel;
            if (earlyLogEvent != null)
                m_logs.LogEvent += earlyLogEvent;

            // Initialize the service and the logic
            m_ovpnService = new OVPNService(bin, config,
                Path.GetDirectoryName(config), m_logs, host, port, logfile);
            m_ovpnMLogic = new OVPNManagementLogic(this, host, port, m_logs);

            m_ovpnService.serviceExited += new EventHandler(m_ovpnService_serviceExited);

            changeState(OVPNState.STOPPED);
        }

        /// <summary>
        /// Called when the object is unloaded.
        /// </summary>
        ~OVPN()
        {
            quit();
        }
        #endregion

        #region propertys

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
        /// Suppress events. This is used when disposing.
        /// </summary>
        internal bool noevents
        {
            get { return m_noevents; }
            set { m_noevents = value; }
        }
        #endregion

        /// <summary>
        /// If the service exits, disconnect, so we got a propper state.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        void m_ovpnService_serviceExited(object sender, EventArgs e)
        {
            try
            {
                if (m_state != OVPNState.STOPPING && m_state != OVPNState.STOPPED)
                {
                    quit();
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Connects with the configured parameters.
        /// </summary>
        /// <seealso cref="quit"/>
        public void start()
        {
            if (m_state != OVPNState.STOPPED && m_state != OVPNState.ERROR)
                throw new InvalidOperationException("Already running");

            m_ovpnMLogic.reset();
            changeState(OVPNState.INITIALIZING);
            
            m_ovpnService.start();
            if (!m_ovpnService.isRunning)
            {
                changeState(OVPNState.ERROR);
                return;
            }

            (new Thread(new ThreadStart(starttimer))).Start();
        }

        /// <summary>
        /// Wait 3 seconds that OpenVPN is started up, then connect
        /// </summary>
        private void starttimer()
        {
            for(int i = 0; i < 5; ++i) {
                try
                {
                    System.Threading.Thread.Sleep(2000);
                    m_ovpnMLogic.connect();
                    return;
                }
                catch (ApplicationException ex)
                {
                    m_logs.logDebugLine(1, "Could not establish connection " +
                        "to management interface:" + ex.Message);
                    if(i != 5)
                        m_logs.logDebugLine(1, "Trying again in 2 seconds");
                }
            }
            m_logs.logDebugLine(1, "Could not establish connection, abording");
            quit();
            changeState(OVPNState.ERROR);
        }

        /// <summary>
        /// Disconnects from the OpenVPN Service.
        /// </summary>
        /// <seealso cref="start"/>
        public void quit()
        {
            if (m_state == OVPNState.STOPPED)
            {
                return;
            }

            if (m_state == OVPNState.ERROR)
            {
                changeState(OVPNState.STOPPED);
            }

            if (m_state == OVPNState.STOPPING)
            {
                throw new InvalidOperationException("Already stopping");
            }

            changeState(OVPNState.STOPPING);

            m_ip = null;
            if (m_ovpnService != null || m_ovpnService != null)
            {
                // disconnect from management interface, initialize shutdown
                m_ovpnMLogic.sendQuit();
                (new Thread(new ThreadStart(killtimer))).Start();
            }
            else
            {
                changeState(OVPNState.STOPPED);
            }
        }

        /// <summary>
        /// Kill the connection after 120 secons unless it is closed
        /// </summary>
        private void killtimer()
        {
            Thread.Sleep(200);
            m_ovpnMLogic.disconnect();

            // wait 120 seconds, stop if the service is down
            for (int i = 0; i < 240; ++i)
            {
                if (!m_ovpnService.isRunning)
                    break;
                Thread.Sleep(500);
            }
            
            if (!m_ovpnService.isRunning)
            {
                m_ovpnService.kill();
            }

            changeState(OVPN.OVPNState.STOPPED);
        }

        /// <summary>
        /// The IP of this endpoint, or null if unknown/unset
        /// </summary>
        public string ip
        {
            get { return m_ip; }
        }

        /// <summary>
        /// change the state of the class
        /// </summary>
        /// <param name="newstate">new state</param>
        internal void changeState(OVPNState newstate)
        {
            if (m_state != newstate)
            {
                m_state = newstate;
                try
                {
                    if (noevents)
                        return;

                    stateChanged(this, new EventArgs());
                }
                catch (NullReferenceException)
                {
                }
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

            try
            {
                needCardID(this, args);
            }
            catch (NullReferenceException)
            { 
            }

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

            try
            {
                needPassword(this, args);
            }
            catch (NullReferenceException)
            {
                return null;
            }
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

            try
            {
                needLoginAndPassword(this, args);
            }
            catch (NullReferenceException)
            {
                return null;
            }

            return new string[] { args.username, args.password };
        }

        internal void changeVPNState(string[] p)
        {
            Array.Copy(p, vpnstate, 4);

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
            
            try
            {
                if (noevents)
                    return;

                vpnStateChanged(this, new EventArgs());
            }
            catch (NullReferenceException)
            {
            }
        }

        /// <summary>
        /// gets the last state that was reported by the opvnvpn process
        /// </summary>
        public string[] vpnState
        {
            get
            {
                return vpnstate;
            }
        }
    }
}