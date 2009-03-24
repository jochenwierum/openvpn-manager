using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace OpenVPNManager
{
    /// <summary>
    /// Simple class which allows to send an array of strings
    /// to the main instance of the program.
    /// </summary>
    class SimpleComm
    {
        /// <summary>
        /// Holds information about the received sting array.
        /// </summary>
        public class ReceivedLinesEventArgs : EventArgs
        {
            /// <summary>
            /// The lines.
            /// </summary>
            private string[] m_str;

            /// <summary>
            /// Creates a new instance, saves the strings.
            /// </summary>
            /// <param name="str">strings to save.</param>
            public ReceivedLinesEventArgs(string[] str)
            {
                m_str = str;
            }

            /// <summary>
            /// The received lines.
            /// </summary>
            public string[] lines
            {
                get { return m_str; }
            }
        }

        /// <summary>
        /// Some lines were received.
        /// </summary>
        /// <param name="sender">The SimpleCom object.</param>
        /// <param name="e">Information about the event.</param>
        public event EventHandler<ReceivedLinesEventArgs> ReceivedLines;

        /// <summary>
        /// The server.
        /// </summary>
        private TcpListener m_server;

        /// <summary>
        /// The port we listen/send to.
        /// </summary>
        private int m_port;

        /// <summary>
        /// is there a server running?
        /// </summary>
        private bool m_running;

        /// <summary>
        /// Create a new SimpleCom object.
        /// </summary>
        /// <param name="port">The port to use.</param>
        public SimpleComm(int port)
        {
            m_port = port;
        }

        /// <summary>
        /// Send some lines to the server.
        /// </summary>
        /// <param name="message">The lines to send.</param>
        /// <returns>True on success, false on failure.</returns>
        public bool client(string[] message)
        {
            try
            {
                TcpClient tc = new TcpClient();
                tc.Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, m_port));

                StreamWriter sw = new StreamWriter(tc.GetStream());
                foreach (string m in message)
                    sw.WriteLine(m);
                sw.Close();

                tc.Close();
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
            catch (ApplicationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Start a server, throw events if some lines are received.
        /// </summary>
        public void startServer()
        {
            m_server = new TcpListener(System.Net.IPAddress.Loopback, m_port);
            m_server.Start();
            m_server.BeginAcceptTcpClient(new AsyncCallback(ReadSocket), null);
            m_running = true;
        }

        /// <summary>
        /// A client wants to connect, accept it, read the text
        /// and wait for the next connection.
        /// </summary>
        /// <param name="iar">AsyncResult object</param>
        private void ReadSocket(IAsyncResult iar)
        {
            try
            {
                TcpClient tc = m_server.EndAcceptTcpClient(iar);
                StreamReader sr = new StreamReader(tc.GetStream());
                List<string> commands = new List<string>();

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (ReceivedLines != null)
                        commands.Add(line);
                }

                ReceivedLines(this, new ReceivedLinesEventArgs(commands.ToArray()));

                sr.Close();
                tc.Close();
            }

            // there three are okay
            catch (IOException)
            {
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            if (m_running)
            {
                try
                {
                    m_server.BeginAcceptTcpClient(new AsyncCallback(ReadSocket), null);
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        // Stop waiting for new connections
        public void stopServer()
        {
            m_running = false;
            m_server.Stop();   
        }
    }
}
