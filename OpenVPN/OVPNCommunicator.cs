using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace OpenVPN
{
    internal class OVPNCommunicator
    {
        #region Variables
        /// <summary>
        /// Host to communicate with (normally '127.0.0.1').
        /// </summary>
        private string m_host;

        /// <summary>
        /// Port to communicate with.
        /// </summary>
        private int m_port;

        /// <summary>
        /// TCP Client which is connected to <c>m_host:m_port</c>.
        /// Used to send and Receive Data to/from the OpenVPN Management Interface.
        /// </summary>
        private TcpClient m_tcpC;

        /// <summary>
        /// Streamreader used to read from <c>m_tcpC</c>.
        /// </summary>
        private StreamReader m_sread;

        /// <summary>
        /// Streamwriter used to write to <c>m_tcpC</c>.
        /// </summary>
        private StreamWriter m_swrite;

        /// <summary>
        /// Thread to read from <c>m_sread</c> asynchronly.
        /// </summary>
        private Thread m_reader;
        
        /// <summary>
        /// Parent object.
        /// </summary>
        private OVPNConnection m_ovpn;

        /// <summary>
        /// Log manager.
        /// </summary>
        private OVPNLogManager m_logs;

        private bool m_connected;

        #endregion
        #region events

        /// <summary>
        /// Signalizes a received text line.
        /// </summary>
        /// <param name="sender">OVPNCommunicator which received the line.</param>
        /// <param name="args">GotLineEventArgs which holds the received line.</param>
        public delegate void GotLineEvent(object sender, GotLineEventArgs args);

        /// <summary>
        /// OVPNCommunicator received a line.
        /// </summary>
        public event GotLineEvent gotLine;

        /// <summary>
        /// Server closed the connection.
        /// </summary>
        public event EventHandler connectionClosed;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new OVPNCommunicator object.
        /// </summary>
        /// <param name="host">Host to connect to (127.0.0.1)</param>
        /// <param name="port">Port to connect to</param>
        /// <param name="logs">Log manager</param>
        /// <param name="ovpn">Parent object</param>
        public OVPNCommunicator(string host, int port,
            OVPNLogManager logs, OVPNConnection ovpn) 
        {
            m_host = host;
            m_port = port;
            m_logs = logs;
            m_ovpn = ovpn;
            m_tcpC = new TcpClient();
        }
        #endregion

        /// <summary>
        /// Connects to the management interface.
        /// </summary>
        public void connect()
        {
            m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "Connecting to management interface");
            m_logs.logDebugLine(1, "Connecting to management interface");

            try
            {
                m_tcpC.Connect(m_host, m_port);
            }
            catch (SocketException e)
            {
                m_logs.logDebugLine(1, "Connection failed: " + e.Message);
                throw; // new ApplicationException("Could not connect to socket: " + e.Message);
            }
            
            m_sread = new StreamReader(m_tcpC.GetStream());
            m_swrite = new StreamWriter(m_tcpC.GetStream());
            m_reader = new Thread(new ThreadStart(readerThread));
            m_reader.Name = "management interface reader thread";
            m_reader.Start();
            m_connected = true;
        }

        /// <summary>
        /// Reads lines from the connection, fires events.
        /// </summary>
        private void readerThread()
        {
            try
            {
                // read until...
                while (true)
                {
                    // read a line
                    string line = m_sread.ReadLine();
                    if (line == null)
                        throw new IOException("Got null");

                    // log line, fire event
                    m_logs.logDebugLine(5, "Got: \"" + line + "\"");
                    gotLine(this, new GotLineEventArgs(line));
                }
            }

            // thread was aborted (this happens on disconnection)
            catch (ThreadAbortException)
            {
                m_logs.logDebugLine(2, "readerThread died: ThreadAbortException");
            }

            // ioexception (this can happen on disconnection, too)
            catch (IOException e)
            {
                m_logs.logDebugLine(2, "readerThread died: IOException: " + e.Message);
            }

            m_logs.logDebugLine(1, "Connection closed by server");
            m_connected = false;
            
            if(connectionClosed != null)
                connectionClosed(this, new EventArgs());
        }

        /// <summary>
        /// Sends a string to the management interface.
        /// </summary>
        /// <param name="s">The string to send</param>
        public void send(string s)
        {
            // can we send?
            if(m_connected)
            {
                m_logs.logDebugLine(5, "Sending \"" + s + "\"");
                m_swrite.WriteLine(s);
                m_swrite.Flush();
            }

            // log an error
            else
                m_logs.logDebugLine(5, "Trying to send, but disconnected or null: \"" + s + "\"");
        }

        /// <summary>
        /// Determines whether we are connected.
        /// </summary>
        /// <returns>true if the socket is connected, false otherwise</returns>
        public bool isConnected()
        {
            return m_connected;
        }

        /// <summary>
        /// Disconnects from the management interface.
        /// We send "signal SIGTERM".
        /// OpenVPN will then close the connection.
        /// All readers will terminate by itself.
        /// </summary>
        public void quit() 
        {
            m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "Sending signal to quit");
            m_logs.logDebugLine(1, "Sending OpenVPN signal SIGTERM");

            if (m_swrite != null)
                send("signal SIGTERM");
        }

        public void restart()
        {
            m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "Sending signal to restart");
            m_logs.logDebugLine(1, "Sending OpenVPN signal SIGHUP");

            if (m_swrite != null)
                send("signal SIGHUP");
        }

        public void logout()
        {
            m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "Logging out");
            
            if(m_swrite != null)
                send("exit");
        }

        /// <summary>
        /// Close all readers, writers, stop thread, close connection imediatelly.
        /// </summary>
        public void disconnect()
        {
            m_logs.logLine(OVPNLogEventArgs.LogType.MGNMT, "Disconnecting from management interface");
            m_logs.logDebugLine(1, "Disconnecting from management interface");
            
            if(m_reader != null)
                m_reader.Abort();

            if(m_sread != null)
                m_sread.Close();

            if(m_swrite != null)
                m_swrite.Close();

            m_tcpC.Close();
            m_tcpC = new TcpClient();
        }
    }
}