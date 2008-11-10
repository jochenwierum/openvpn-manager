using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace OpenVPN
{
    /// <summary>
    /// controls a openvpn binary
    /// </summary>
    internal class OVPNService
    {
        #region enums
        /// <summary>
        /// possible state of a service
        /// </summary>
        public enum OVPNServiceState { 
            STOPPING,
            STOPPED,
            RUNNING
        };
        #endregion

        #region variables

        /// <summary>
        /// internal number of the object
        /// </summary>
        private int m_objid;

        /// <summary>
        /// state of the object
        /// </summary>
        private OVPNServiceState m_state = OVPNServiceState.STOPPED;

        /// <summary>
        /// eventhandle used to stop OpenVPN
        /// </summary>
        private EventWaitHandle m_ewh;

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
        #endregion

        #region events
        /// <summary>
        /// delegate which describes GotLine events
        /// </summary>
        /// <param name="sender">reference to OVPNService</param>
        /// <param name="args">information about the read line</param>
        public delegate void GotLineEvent(object sender, GotLineEventArgs args);

        /// <summary>
        /// OpenVPN wrote something to StdOut
        /// </summary>
        public event GotLineEvent gotStdoutLine;

        /// <summary>
        /// OpenVPN wrote something to StdErr
        /// </summary>
        public event GotLineEvent gotStderrLine;

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
        public OVPNService(string binfile, string configfile, 
            string dir, OVPNLogManager logs, string host, int port) 
        {
            // generate internal id
            m_objid = ++objcount;
            m_logs = logs;

            // generate event handler
            string evname = "openvpn_exit_event_" + m_objid;
            m_logs.logDebugLine(2, "Creating EventWaitHandle \"" + evname + "\"");
            m_ewh = new EventWaitHandle(false, EventResetMode.ManualReset, evname);

            // set startup data
            m_psi.FileName = binfile;
            m_psi.RedirectStandardOutput = true;
            m_psi.RedirectStandardError = true;
            m_psi.WorkingDirectory = dir;
            m_psi.UseShellExecute = false;
            m_psi.CreateNoWindow = true;
            m_psi.Arguments =
                "--config \"" + configfile + "\"" +
                " --service " + evname + " 0" +
                " --management " + host + " " + port.ToString() +
                " --management-query-passwords" +
                " --management-hold" +
                " --pkcs11-id-management";
        }

        /// <summary>
        /// Start the OpenVPN binary.
        /// </summary>
        public void start() 
        {
            // reset the event, drop some lines, start process
            m_ewh.Reset();
            m_logs.logDebugLine(1, "Starting OpenVPN");
            m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "Starting OpenVPN...");

            // set process data, start the process
            m_process = new Process();
            m_process.StartInfo = m_psi;
            m_process.OutputDataReceived += new DataReceivedEventHandler(this.stdout_event);
            m_process.ErrorDataReceived += new DataReceivedEventHandler(this.stderr_event);
            m_process.Exited += new EventHandler(this.exited_event);
            m_process.EnableRaisingEvents = true;
            m_process.Start();
            m_process.BeginErrorReadLine();
            m_process.BeginOutputReadLine();

            m_logs.logDebugLine(1, "Started");
            m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "OpenVPN is running");

            // set the state
            m_state = OVPNServiceState.RUNNING;
        }

        /// <summary>
        /// Stop the service.
        /// </summary>
        public void stop() 
        {
            m_logs.logDebugLine(2, "Trying to stop OpenVPN");

            // stop only if needed
            if (m_state != OVPNServiceState.STOPPED)
            {
                // send stop event
                m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "Stopping OpenVPN");
                m_state = OVPNServiceState.STOPPING;
                m_ewh.Set();
                m_logs.logDebugLine(2, "Signal Send");

                (new Thread(new ThreadStart(killtimer))).Start();
            }
            else
            {
                m_logs.logDebugLine(2, "Already stopped");
            }
        }

        /// <summary>
        /// Kills remaining process after 3 seconds.
        /// </summary>
        private void killtimer()
        {
            try
            {
                Thread.Sleep(3000);
            }
            catch (ApplicationException)
            {
            }
            
            if (m_state != OVPNServiceState.STOPPING)
                return;

            m_process.Kill();
        }

        /// <summary>
        /// Process exited, reset everything important.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="args">ignored</param>
        private void exited_event(object sender, EventArgs args)
        {
            // abort reading stderr, stdout, reset state
            m_logs.logDebugLine(2, "OpenVPN stopped");
            m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "OpenVPN stopped");
            m_process.CancelErrorRead();
            m_process.CancelOutputRead();
            m_state = OVPNServiceState.STOPPED;
            serviceExited(this, new EventArgs());
            
        }

        /// <summary>
        /// OpenVPN wrote something to StdOut, raise event.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="args">information about the read line</param>
        private void stdout_event(object sender, DataReceivedEventArgs args)
        {
            if (args.Data != null)
                gotStdoutLine(this, new GotLineEventArgs(args.Data));
        }

        /// <summary>
        /// OpenVPN wrote something to StdOut, raise event.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="args">information about the read line</param>
        private void stderr_event(object sender, DataReceivedEventArgs args)
        {
            if (args.Data != null)
                gotStderrLine(this, new GotLineEventArgs(args.Data));
        }
    }
}
