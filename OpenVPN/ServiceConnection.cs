using System;
using System.IO;
using OpenVPN.States;
using System.Threading;

namespace OpenVPN
{
    /// <summary>
    /// Provides access to OpenVPN.
    /// </summary>
    public class ServiceConnection : Connection
    {
        #region constructors/destructors
        /// <summary>
        /// Initializes a new OVPN Object.
        /// </summary>
        /// <param name="config">Path to configuration file</param>
        public ServiceConnection(string config)
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
        /// <seealso cref="Connection.Logs"/>
        public ServiceConnection(string config,
            EventHandler<LogEventArgs> earlyLogEvent, int earlyLogLevel)
        {
            if (config == null)
                throw new ArgumentNullException(config, "Config file is null");
            if (!new FileInfo(config).Exists)
                throw new FileNotFoundException(config, "Config file \"" + config + "\" does not exist");

            ConfigParser cf = new ConfigParser(config);


            //management 127.0.0.1 11194
            foreach (string directive in new String[]{ 
                "management-query-passwords", "management-hold",
                "management-signal", "management-forget-disconnect",
                "pkcs11-id-management", "management"}) {

                if(!cf.DirectiveExists(directive))
                    throw new ArgumentException("The directive '" + directive
                        + "' is needed in '" + config + "'");
            }

            int port;
            string[] args = cf.GetValue("management");
            if(args.GetUpperBound(0) != 2)
                throw new ArgumentException("The directive 'management'"
                            + " is invalid in '" + config + "'");

            if(!int.TryParse(args[2], out port))
                throw new ArgumentException("The port '" + args[0]
                        + "' is invalid in '" + config + "'");

            this.init(args[1], port, earlyLogEvent, earlyLogLevel);
        }

        /*
        /// <summary>
        /// Destructor. Terminates a remaining connection.
        /// </summary>
        ~ServiceConnection()
        {
        }*/

        #endregion

        /// <summary>
        /// Connects with the configured parameters.
        /// </summary>
        /// <seealso cref="Disconnect"/>
        public override void Connect()
        {
            CheckState(VPNConnectionState.Initializing);
            State.ChangeState(VPNConnectionState.Initializing);
            var del = new helper.Function<bool>(ConnectLogic);
            del.BeginInvoke(null, null);
        }

        /// <summary>
        /// Disconnects from the OpenVPN Service.
        /// </summary>
        /// <seealso cref="Connect"/>
        public override void Disconnect()
        {
            StateSnapshot ss = State.CreateSnapshot();
            if (ss.ConnectionState == VPNConnectionState.Stopped ||
                State.ConnectionState == VPNConnectionState.Error)
            {
                State.ChangeState(VPNConnectionState.Stopped);
                return;
            }
            State.ChangeState(VPNConnectionState.Stopping);

            Logic.sendRestart();
            Logic.sendDisconnect();

            var del = new helper.Action(killtimer);
            del.BeginInvoke(null, null);
        }

        /// <summary>
        /// Kill the connection after 30 secons unless it is closed
        /// </summary>
        private void killtimer()
        {
            while (Logic.isConnected())
                Thread.Sleep(100);
            DisconnectLogic();
            State.ChangeState(VPNConnectionState.Stopped);
        }
    }
}