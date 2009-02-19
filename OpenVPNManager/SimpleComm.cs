using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;

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
        /// Delegate to a ReceivedLines event.
        /// </summary>
        /// <param name="sender">The SimpleCom object.</param>
        /// <param name="e">Information about the event.</param>
        public delegate void ReceivedLinesDelegate(object sender, ReceivedLinesEventArgs e);

        /// <summary>
        /// Some lines were received.
        /// </summary>
        /// <param name="sender">The SimpleCom object.</param>
        /// <param name="e">Information about the event.</param>
        public event ReceivedLinesDelegate ReceivedLines;

        /// <summary>
        /// The server.
        /// </summary>
        private TcpListener m_server;

        /// <summary>
        /// The port we listen/send to.
        /// </summary>
        private int m_port;

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
            m_server.Start(3);
            m_server.BeginAcceptTcpClient(new AsyncCallback(ReadSocket), m_server);
        }

        /// <summary>
        /// A client wants to connect, accept it, read the text
        /// and wait for the next connection.
        /// </summary>
        /// <param name="iar"></param>
        private void ReadSocket(IAsyncResult iar)
        {
            try
            {
                TcpClient tc = ((TcpListener)iar.AsyncState).EndAcceptTcpClient(iar);
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

            try
            {
                ((TcpListener)iar.AsyncState).BeginAcceptTcpClient(new AsyncCallback(ReadSocket), iar.AsyncState);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        // Stop waiting for new connections
        public void stopServer()
        {
            m_server.Stop();   
        }
    }
}
