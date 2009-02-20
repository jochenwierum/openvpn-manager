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
    public class OVPNUserConnection : OVPNConnection
    {

        #region variables
        /// <summary>
        /// The OpenVPN binary service.
        /// </summary>
        private OVPNService m_ovpnService;

        /// <summary>
        /// Counts, how many objects have been created.
        /// </summary>
        private static int obj_count = 0;
        #endregion

        #region constructors/destructors
        /// <summary>
        /// Initializes a new OVPN Object.
        /// </summary>
        /// <param name="bin">Path to openvpn binary</param>
        /// <param name="config">Path to configuration file</param>
        /// <param name="logfile">File to write OpenVPN log messages to</param>
        public OVPNUserConnection(string bin, string config, string logfile)
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
        /// <seealso cref="OVPNConnection.logs"/>
        public OVPNUserConnection(string bin, string config, string logfile,
            OVPNLogManager.LogEventDelegate earlyLogEvent, int earlyLogLevel) :
            base("127.0.0.1", 11195 + obj_count++, earlyLogEvent, earlyLogLevel)
        {
            if (bin == null)
                throw new ArgumentNullException("Binary is null");
            if (config == null)
                throw new ArgumentNullException("Config file is null");
            if (!new FileInfo(bin).Exists)
                throw new FileNotFoundException("Binary \"" + bin + "\" does not exist");
            if (!new FileInfo(config).Exists)
                throw new FileNotFoundException("Config file \"" + config + "\" does not exist");

            m_ovpnService = new OVPNService(bin, config,
                Path.GetDirectoryName(config), logs, base.host, base.port, logfile);

            m_ovpnService.serviceExited += new EventHandler(m_ovpnService_serviceExited);
        }

        /// <summary>
        /// Destructor. Terminates a remaining connection.
        /// </summary>
        ~OVPNUserConnection()
        {
            if (state != OVPNState.STOPPED)
                disconnect();
        }

        #endregion

        #region eventhandler
        /// <summary>
        /// If the service exits, disconnect, so we got a propper state.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        void m_ovpnService_serviceExited(object sender, EventArgs e)
        {
            try
            {
                if (state != OVPNState.STOPPING && state != OVPNState.STOPPED)
                {
                    disconnect();
                }
            }
            catch (InvalidOperationException)
            {
            }
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
            
            m_ovpnService.start();
            if (!m_ovpnService.isRunning)
            {
                changeState(OVPNState.ERROR);
                return;
            }

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

            logic.sendQuit();
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

            // wait 30 seconds, stop if the service is down
            for (int i = 0; i < 60; ++i)
            {
                if (!m_ovpnService.isRunning)
                    break;
                Thread.Sleep(500);
            }
            
            if (m_ovpnService.isRunning)
            {
                m_ovpnService.kill();
            }

            changeState(OVPNConnection.OVPNState.STOPPED);
        }
    }
}