using System;
using System.IO;
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

        private object lockvar = new Object();
        #endregion

        #region constructors/destructors
        /// <summary>
        /// Initializes a new OVPN Object.
        /// Also set a LogEventDelegate so that the first log lines are reveived.
        /// </summary>
        /// <param name="bin">Path to openvpn binary</param>
        /// <param name="config">Path to configuration file</param>
        /// <param name="earlyLogEvent">Delegate to a event processor</param>
        /// <param name="earlyLogLevel">Log level</param>
        /// <param name="logfile">File to write OpenVPN log message to</param>
        /// <param name="smartCardSupport">Enable SmartCard support</param>
        /// <seealso cref="Connection.Logs"/>
        public UserSpaceConnection(string bin, string config, string logfile,
            EventHandler<LogEventArgs> earlyLogEvent, int earlyLogLevel, bool smartCardSupport)
        {
            if (bin == null || bin.Length == 0)
                throw new ArgumentNullException(bin, "OpenVPN Binary is not valid/selected");
            if (config == null || config.Length == 0)
                throw new ArgumentNullException(config, "Config file is not valid/selected");
            if (!new FileInfo(bin).Exists)
                throw new FileNotFoundException(bin,
                    "Binary \"" + bin + "\" does not exist");
            if (!new FileInfo(config).Exists)
                throw new FileNotFoundException(config,
                    "Config file \"" + config + "\" does not exist");

            this.Init("127.0.0.1", 11195 + obj_count++, earlyLogEvent, earlyLogLevel, true);
            m_ovpnService = new UserSpaceService(bin, config,
                Path.GetDirectoryName(config), Logs, base.Host, base.Port,
                logfile, smartCardSupport);

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
                    State.ChangeState(VPNConnectionState.Error);
                    IP = null;
                }
            }
            catch (InvalidOperationException)
            {
            }
        }
        #endregion

        private int m_connectState;
        private bool m_abort;

        /// <summary>
        /// Connects with the configured parameters.
        /// </summary>
        /// <seealso cref="Disconnect"/>
        public override void Connect()
        {
            CheckState(VPNConnectionState.Initializing);
            State.ChangeState(VPNConnectionState.Initializing);

            m_connectState = 1;
            m_abort = false;

            m_ovpnService.Start();
            if (!m_ovpnService.isRunning)
            {
                State.ChangeState(VPNConnectionState.Error);
                IP = null;
                return;
            }

            helper.Function<bool> cld = new helper.Function<bool>(ConnectLogic);
            m_connectState = 2;
            cld.BeginInvoke(connectComplete, cld);
        }

        private void connectComplete(IAsyncResult result)
        {
            StateSnapshot ss;
            helper.Function<bool> cld;
            bool abort;
            int connectionState;

            try
            {
                Monitor.Enter(lockvar);
                ss = State.CreateSnapshot();
                cld = (helper.Function<bool>)result.AsyncState;
                abort = m_abort;
                connectionState = m_connectState;

                if (cld.EndInvoke(result))
                    m_connectState = 3;
            }
            finally
            {
                Monitor.Exit(lockvar);
            }


            if (abort && ss.ConnectionState == VPNConnectionState.Stopping)
            {
                m_abort = false;
                Logs.logDebugLine(2, "Connection is marked as aborded");
                switch (connectionState)
                {
                    case 1: // service not startet
                        Logs.logDebugLine(2, "No action required");
                        break;
                    case 2: // service startet, not connected via tcp
                        Logs.logDebugLine(2, "Killing serivce");
                        m_ovpnService.kill();
                        break;
                    case 3: // service startet and connected via tcp
                        Logs.logDebugLine(2, "Calling disconnect");
                        Disconnect();
                        break;
                    default:
                        Logs.logDebugLine(1, "Connection state is invalid (" +
                            connectionState + "). Ignoring disconnect event.");
                        break;
                }
                State.ChangeState(VPNConnectionState.Stopped);
            }
        }

        /// <summary>
        /// Disconnects from the OpenVPN Service.
        /// </summary>
        /// <seealso cref="Connect"/>
        public override void Disconnect()
        {
            StateSnapshot ss;

            try
            {
                Monitor.Enter(lockvar);
                if (State.ConnectionState == VPNConnectionState.Stopped) return;

                if (State.ConnectionState == VPNConnectionState.Error)
                {
                    State.ChangeState(VPNConnectionState.Stopped);
                    return;
                }

                ss = State.ChangeState(VPNConnectionState.Stopping);
                m_abort = true;


                if (ss.ConnectionState == VPNConnectionState.Running ||
                    (ss.ConnectionState == VPNConnectionState.Initializing &&
                    m_connectState == 3))
                {
                    Logic.sendQuit();
                    Thread t = new Thread(new ThreadStart(killtimer));
                    t.Name = "async disconnect thread";
                    t.Start();
                    m_abort = false;
                }
            }
            finally
            {
                Monitor.Exit(lockvar);
            }
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
        private void Dispose(bool disposing)
        {
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