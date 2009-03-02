using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using OpenVPN.States;

namespace OpenVPN
{
    /// <summary>
    /// Provides access to OpenVPN.
    /// </summary>
    public class UserSpaceConnection : Connection, IDisposable
    {

        #region variables
        /// <summary>
        /// The OpenVPN binary service.
        /// </summary>
        private UserSpaceService m_ovpnService;

        /// <summary>
        /// Counts, how many objects have been created.
        /// </summary>
        private static int obj_count;
        #endregion

        #region constructors/destructors
        /// <summary>
        /// Initializes a new OVPN Object.
        /// </summary>
        /// <param name="bin">Path to openvpn binary</param>
        /// <param name="config">Path to configuration file</param>
        /// <param name="logfile">File to write OpenVPN log messages to</param>
        public UserSpaceConnection(string bin, string config, string logfile)
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
        /// <seealso cref="Connection.Logs"/>
        public UserSpaceConnection(string bin, string config, string logfile,
            EventHandler<LogEventArgs> earlyLogEvent, int earlyLogLevel) :
            base("127.0.0.1", 11195 + obj_count++, earlyLogEvent, earlyLogLevel)
        {
            if (bin == null)
                throw new ArgumentNullException(bin, "Binary is null");
            if (config == null)
                throw new ArgumentNullException(config, "Config file is null");
            if (!new FileInfo(bin).Exists)
                throw new FileNotFoundException(bin, 
                    "Binary \"" + bin + "\" does not exist");
            if (!new FileInfo(config).Exists)
                throw new FileNotFoundException(config, 
                    "Config file \"" + config + "\" does not exist");

            m_ovpnService = new UserSpaceService(bin, config,
                Path.GetDirectoryName(config), Logs, base.Host, base.Port, logfile);

            m_ovpnService.serviceExited += new EventHandler(m_ovpnService_serviceExited);
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
                if (State.ConnectionState != VPNConnectionState.Stopping 
                    && State.ConnectionState != VPNConnectionState.Stopped)
                {
                    Disconnect();
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
        /// <seealso cref="Disconnect"/>
        public override void Connect()
        {
            CheckState(VPNConnectionState.Initializing);
            State.ChangeState(VPNConnectionState.Initializing);
            
            m_ovpnService.Start();
            if (!m_ovpnService.isRunning)
            {
                State.ChangeState(VPNConnectionState.Error);
                return;
            }

            Thread t = new Thread(new ThreadStart(connectThread));
            t.Name = "async connect thread";
            t.Start();
        }

        /// <summary>
        /// Just a wrapper function which calls connectLogic but returns void.
        /// </summary>
        private void connectThread()
        {
            ConnectLogic();
        }

        /// <summary>
        /// Disconnects from the OpenVPN Service.
        /// </summary>
        /// <seealso cref="Connect"/>
        public override void Disconnect()
        {
            CheckState(VPNConnectionState.Stopping);
            if (State.ConnectionState == VPNConnectionState.Stopped)
            {
                return;
            }
            State.ChangeState(VPNConnectionState.Stopping);

            Logic.sendQuit();
            Thread t = new Thread(new ThreadStart(killtimer));
            t.Name = "async disconnect thread";
            t.Start();
        }

        /// <summary>
        /// Kill the connection after 30 secons unless it is closed
        /// </summary>
        private void killtimer()
        {
            while (Logic.isConnected())
                Thread.Sleep(100);
            DisconnectLogic();

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

            State.ChangeState(VPNConnectionState.Stopped);
        }

        #region IDisposable Members

        private bool disposed;
        /// <summary>
        /// Destructor. Terminates a remaining connection.
        /// </summary>
            
        ~UserSpaceConnection()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose this object.
        /// </summary>
        /// <param name="disposing">true if called from Dispose(), false if called from destructor</param>
        private void Dispose(bool disposing) {
            if (!this.disposed)
            {
                base.Dispose();
                if (disposing)
                {
                    m_ovpnService.Dispose();
                }

                m_ovpnService = null;
                disposed = true;
            }
        }

        #endregion
    }
}