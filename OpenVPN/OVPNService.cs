using System;
using System.Diagnostics;

namespace OpenVPN
{
    /// <summary>
    /// controls a openvpn binary
    /// </summary>
    internal class OVPNService
    {
        /*#region enums
        /// <summary>
        /// possible state of a service
        /// </summary>
        public enum OVPNServiceState { 
            STOPPING,
            STOPPED,
            RUNNING
        };
        #endregion*/

        #region variables

        /// <summary>
        /// internal number of the object
        /// </summary>
        private int m_objid;

        /*/// <summary>
        /// state of the object
        /// </summary>
        private OVPNServiceState m_state = OVPNServiceState.STOPPED;*/

        /*/// <summary>
        /// eventhandle used to stop OpenVPN
        /// </summary>
        private EventWaitHandle m_ewh;*/

        /// <summary>
        /// information about the OpenVPN binary process start
        /// </summary>
        private ProcessStartInfo m_psi = new ProcessStartInfo();

        /// <summary>
        /// information about the OpenVPN binary process
        /// </summary>
        private Process m_process;

        /// <summary>
        /// log manager which is used to write log lines to
        /// </summary>
        private OVPNLogManager m_logs;

        /// <summary>
        /// number of objects created so far
        /// </summary>
        private static int objcount = 0;

        private bool running;
        #endregion

        #region events
        /// <summary>
        /// fired when process closes
        /// </summary>
        public event EventHandler serviceExited;
        #endregion

        /// <summary>
        /// Initialize a new OpenVPN service.
        /// </summary>
        /// <param name="binfile">path to openvpn</param>
        /// <param name="configfile">path to openvpn config</param>
        /// <param name="dir">directory where config lies</param>
        /// <param name="logs">provider to write logs to</param>
        /// <param name="host">The host to connect to (e.g. 127.0.0.1)</param>
        /// <param name="port">The port to connect to</param>
        /// <param name="logfile">file to write OpenVPN log to</param>
        public OVPNService(string binfile, string configfile, 
            string dir, OVPNLogManager logs, string host, int port,
            string logfile) 
        {
            m_objid = ++objcount;
            m_logs = logs;
            running = false;

            m_psi.FileName = binfile;
            m_psi.WorkingDirectory = dir;
            m_psi.WindowStyle = ProcessWindowStyle.Hidden;
            m_psi.UseShellExecute = true;
            m_psi.Verb = "runas";
            m_psi.CreateNoWindow = true;
            m_psi.Arguments =
                "--log \"" + logfile + "\"" +
                " --config \"" + configfile + "\"" +
                " --management " + host + " " + port.ToString() +
                " --management-query-passwords" +
                " --management-hold" +
                " --management-signal" +
                " --management-forget-disconnect" +
                " --pkcs11-id-management";
        }

        /// <summary>
        /// Start the OpenVPN binary.
        /// </summary>
        public void start() 
        {
            m_logs.logDebugLine(1, "Starting OpenVPN");
            m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "Starting OpenVPN...");

            m_process = new Process();
            m_process.StartInfo = m_psi;
            m_process.Exited += new EventHandler(this.exited_event);
            m_process.EnableRaisingEvents = true;
            m_process.Start();

            m_logs.logDebugLine(1, "Started");
            m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "OpenVPN is running");

            running = true;
            //m_state = OVPNServiceState.RUNNING;
        }

        /// <summary>
        /// Kills remaining process after 3 seconds.
        /// </summary>
        public void kill()
        {
            if (!running) return;
            /*if (m_state != OVPNServiceState.STOPPING)
                return;*/

            m_logs.logDebugLine(2, "Forcing OpenVPN to terminate");

            try
            {
                m_process.Kill();
            }
            catch (InvalidOperationException e)
            {
                m_logs.logDebugLine(1, "Could not stop openvpn: " + e.Message);
            }
        }

        public bool hasExited
        {
            get { return m_process.HasExited; }
        }

        /// <summary>
        /// Process exited, reset everything important.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="args">ignored</param>
        private void exited_event(object sender, EventArgs args)
        {
            running = false;
            m_logs.logDebugLine(2, "OpenVPN stopped");
            m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "OpenVPN stopped");
            serviceExited(this, new EventArgs());
        }
    }
}
