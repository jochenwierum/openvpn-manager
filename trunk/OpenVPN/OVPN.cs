using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

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
            STOPPED
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
        public OVPN(string bin, string config)
        {
            init(bin, config, null, 1); 
        }

        /// <summary>
        /// Initializes a new OVPN Object.
        /// Also set a LogEventDelegate so that the first log lines are reveived.
        /// </summary>
        /// <param name="bin">Path to openvpn binary</param>
        /// <param name="config">Path to configuration file</param>
        /// <param name="earlyLogEvent">Delegate to a event processor</param>
        /// <param name="earlyLogLevel">Log level</param>
        /// <seealso cref="logs"/>
        public OVPN(string bin, string config, 
            OVPNLogManager.LogEventDelegate earlyLogEvent,
            int earlyLogLevel)
        {
            init(bin, config, earlyLogEvent, earlyLogLevel);
        }

        /// <summary>
        /// Initializes a new OVPN Object.
        /// Also set a LogEventDelegate so that the first log lines are reveived.
        /// </summary>
        /// <param name="bin">Path to openvpn binary</param>
        /// <param name="config">Path to configuration file</param>
        /// <param name="earlyLogEvent">Delegate to a event processor</param>
        /// <param name="earlyLogLevel">Log level</param>
        private void init(string bin, string config,
            OVPNLogManager.LogEventDelegate earlyLogEvent,
            int earlyLogLevel)
        {

            // Check parameters
            if (bin == null)
                throw new ArgumentNullException("Binary is null");
            if (config == null)
                throw new ArgumentNullException("Config file is null");

            // Check for files
            if (!new FileInfo(bin).Exists)
                throw new FileNotFoundException("Binary \"" + bin + "\" does not exist");
            if (!new FileInfo(config).Exists)
                throw new FileNotFoundException("Config file \"" + config + "\" does not exist");

            // Initialize logging
            m_logs = new OVPNLogManager(this);
            if (earlyLogEvent != null)
                m_logs.LogEvent += earlyLogEvent;
            if (earlyLogLevel >= 0)
                m_logs.debugLevel = earlyLogLevel;

            // Management-Interface parameters
            string host = "127.0.0.1";
            int port = 11195 + obj_count++;

            // Initialize the service and the logic
            m_ovpnService = new OVPNService(bin, config,
                Path.GetDirectoryName(config), m_logs, host, port);
            m_ovpnMLogic = new OVPNManagementLogic(this, host, port, m_logs);

            // Process service messages
            m_ovpnService.gotStderrLine += new OVPNService.GotLineEvent(m_os_gotStderrLine);
            m_ovpnService.gotStdoutLine += new OVPNService.GotLineEvent(m_os_gotStdoutLine);
            m_ovpnService.serviceExited += new EventHandler(m_ovpnService_serviceExited);

            // Set initial state
            changeState(OVPNState.STOPPED);
        }

        /// <summary>
        /// If we dispose, stop at least openvpn.
        /// </summary>
        ~OVPN()
        {
            if (m_ovpnService != null)
                m_ovpnService.stop();
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
                disconnect();
            }
            catch (ApplicationException)
            {
            }
            changeState(OVPNState.STOPPED);
        }

        /// <summary>
        /// Connects with the configured parameters.
        /// </summary>
        /// <seealso cref="disconnect"/>
        public void connect()
        {
            if (m_state != OVPNState.STOPPED)
                throw new ApplicationException("Already running");

            m_ovpnMLogic.reset();
            m_ovpnService.start();
            changeState(OVPNState.INITIALIZING);
        }

        /// <summary>
        /// Disconnects from the OpenVPN Service.
        /// </summary>
        /// <seealso cref="connect"/>
        public void disconnect()
        {
            if (m_state == OVPNState.STOPPED) return;
            if (m_state == OVPNState.STOPPING)
                throw new ApplicationException("Already stopping");

            changeState(OVPNState.STOPPING);
            m_ip = null;
            if (m_ovpnService != null)
                m_ovpnService.stop();
            else
                changeState(OVPNState.STOPPED);
        }

        /// <summary>
        /// A line was received on stdout.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="args">information about the received line</param>
        private void m_os_gotStdoutLine(object sender, GotLineEventArgs args)
        {
            // log output
            m_logs.logLine(OVPNLogEventArgs.LogType.STDOUT, args.line);

            // the first line after a successful start (I...)
            if (args.line.Contains("OpenVPN") &&
                args.line.Contains("built on"))

                // connect to the management interface
                try
                {
                    System.Threading.Thread.Sleep(2000);
                    m_ovpnMLogic.connect();
                }
                catch (ApplicationException ex)
                {
                    m_logs.logDebugLine(1, "Could not establish connection " +
                        "to management interface:" + ex.Message);
                    disconnect();
                }

            // does it say: we are ready?
            else if (args.line.Contains("Initialization Sequence Completed"))

                // change the state
                changeState(OVPNState.RUNNING);

            // does it say: we got an ip?
            else if (args.line.Contains("Notified TAP-Win32 driver to set a DHCP IP"))
                m_ip = getIP(args.line);
        }

        /// <summary>
        /// Extracts IP and Subnet from a String.
        /// </summary>
        /// <param name="l">The String to parse</param>
        /// <returns>IP and Subnet in the Form "192.168.0.1/24"</returns>
        private string getIP(string l)
        {
            Regex r = new Regex(
                @"((?:[0-9]{1,3}\.){3}[0-9]{1,3})/((?:[0-9]{1,3}\.){3}[0-9]{1,3})"
                );

            Match m = r.Match(l);

            if (m.Success)
                return m.Groups[1].Value + "/" + getSubnet(m.Groups[2].Value);
            return null;
        }

        /// <summary>
        /// Extracts a Subnet from a String.
        /// </summary>
        /// <param name="s">The String to parse</param>
        /// <returns>Length of Subnet-Bits</returns>
        private int getSubnet(string s)
        {
            string[] parts = s.Split(new char[] { '.' });
            int subnet = 0;

            foreach (string part in parts)
            {
                subnet += Convert.ToString(Byte.Parse(part), 2)
                    .Replace("0", "").Length;
            }

            return subnet;
        }

        /// <summary>
        /// a line was received on stdout
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="args">information about the received line</param>
        private void m_os_gotStderrLine(object sender, GotLineEventArgs args)
        {
            // we just log it
            m_logs.logLine(OVPNLogEventArgs.LogType.STDERR, args.line);
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
            // only if the state is new...
            if (m_state != newstate)
            {
                // set it
                m_state = newstate;
                try
                {
                    // notify the listeners
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

            // initialize the event arguments
            OVPNNeedCardIDEventArgs args = 
                new OVPNNeedCardIDEventArgs(pkcs11d.ToArray());

            try
            {
                m_logs.logDebugLine(1, "Asking user for Token");

                // raise the event
                needCardID(this, args);
            }
            catch (NullReferenceException)
            { 
            }

            // return the answer
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

            // prepare the event data
            OVPNNeedPasswordEventArgs args =
                new OVPNNeedPasswordEventArgs(pwType);

            try
            {
                m_logs.logDebugLine(1, "Asking user for password \"" + pwType + "\"");

                // raise the event
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
            string[] info = null;
            if (noevents) return null;

            // prepare the event data
            OVPNNeedLoginAndPasswordEventArgs args =
                new OVPNNeedLoginAndPasswordEventArgs(pwType);

            try
            {
                m_logs.logDebugLine(1, "Asking user for username and password \"" + pwType + "\"");

                // raise the event
                needLoginAndPassword(this, args);
            }
            catch (NullReferenceException)
            {
                return null;
            }
            info = new string[] {args.username, args.password};
            return info;
        }
    }
}
