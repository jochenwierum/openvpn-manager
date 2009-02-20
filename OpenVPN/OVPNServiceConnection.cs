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
    public class OVPNServiceConnection : OVPNConnection
    {

        #region variables
        #endregion

        #region constructors/destructors
        /// <summary>
        /// Initializes a new OVPN Object.
        /// </summary>
        /// <param name="config">Path to configuration file</param>
        public OVPNServiceConnection(string config)
            : this(config, null, 1)
        {
        }

        /// <summary>
        /// Initializes a new OVPN Object.
        /// Also set a LogEventDelegate so that the first log lines are reveived.
        /// </summary>
        /// <param name="config">Path to configuration file</param>
        /// <param name="earlyLogEvent">Delegate to a event processor</param>
        /// <param name="earlyLogLevel">Log level</param>
        /// <seealso cref="OVPNConnection.logs"/>
        public OVPNServiceConnection(string config,
            OVPNLogManager.LogEventDelegate earlyLogEvent, int earlyLogLevel)
            : base(earlyLogEvent, earlyLogLevel)
        {
            if (config == null)
                throw new ArgumentNullException("Config file is null");
            if (!new FileInfo(config).Exists)
                throw new FileNotFoundException("Config file \"" + config + "\" does not exist");

            OVPNConfigFile cf = new OVPNConfigFile(config);


            //management 127.0.0.1 11194
            foreach (string directive in new String[]{ 
                "management-query-passwords", "management-hold",
                "management-signal", "management-forget-disconnect",
                "pkcs11-id-management", "management"}) {

                if(cf.exists(directive))
                    throw new ArgumentException("The directive '" + directive
                        + "' is needed in '" + config + "'");
            }

            int port;
            string[] args = cf.get("management");
            if(args.GetUpperBound(0) != 2)
                throw new ArgumentException("The directive 'management'"
                            + " is invalid in '" + config + "'");

            if(!int.TryParse(args[1], out port))
                throw new ArgumentException("The port '" + args[0]
                        + "' is invalid in '" + config + "'");

            this.port = port;
            this.host = args[0];
        }

        /// <summary>
        /// Destructor. Terminates a remaining connection.
        /// </summary>
        ~OVPNServiceConnection()
        {
            if (state != OVPNState.STOPPED)
                disconnect();
        }

        #endregion

        /// <summary>
        /// Connects with the configured parameters.
        /// </summary>
        /// <seealso cref="disconnect"/>
        public override void connect()
        {
            checkState(OVPNState.INITIALIZING);
            changeState(OVPNState.INITIALIZING);
            (new Thread(new ThreadStart(connectThread))).Start();
        }

        /// <summary>
        /// Just a wrapper function which calls connectLogic but returns void.
        /// </summary>
        private void connectThread()
        {
            connectLogic();
        }

        /// <summary>
        /// Disconnects from the OpenVPN Service.
        /// </summary>
        /// <seealso cref="connect"/>
        public override void disconnect()
        {
            checkState(OVPNState.STOPPING);
            if (state == OVPNState.STOPPED)
            {
                return;
            }
            changeState(OVPNState.STOPPING);

            logic.sendRestart();
            logic.sendDisconnect();
            (new Thread(new ThreadStart(killtimer))).Start();
        }

        /// <summary>
        /// Kill the connection after 30 secons unless it is closed
        /// </summary>
        private void killtimer()
        {
            while (logic.isConnected())
                Thread.Sleep(100);
            disconnectLogic();
            changeState(OVPNConnection.OVPNState.STOPPED);
        }
    }
}