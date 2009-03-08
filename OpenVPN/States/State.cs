using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;


[module: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", 
    Scope = "namespace", Target = "OpenVPN.States", MessageId = "VPN")]
[module: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", 
    Scope = "namespace", Target = "OpenVPN.States")]
namespace OpenVPN.States
{
    /// <summary>
    /// Represents the state of the Connection an OpenVPN
    /// </summary>
    public class State
    {        
        /// <summary>
        /// Internal state of openvpn.
        /// </summary>
        private string[] m_vpnstate = new string[] { "", "", "", "" };

        /// <summary>
        /// Signals, that the state of vpn has changed.
        /// </summary>
        public event EventHandler<StateChangedEventArgs> StateChanged;

        /// <summary>
        /// The parent.
        /// </summary>
        private Connection m_connection;

        /// <summary>
        /// Initalizes the state
        /// </summary>
        /// <param name="parent">Parent Control</param>
        internal State(Connection parent)
        {
            m_connection = parent;
        }

        /// <summary>
        /// Is the connection stoppable at the moment?
        /// </summary>
        internal bool Stoppable
        {
            get;
            set;
        }

        /// <summary>
        /// State of the connection.
        /// </summary>
        internal VPNConnectionState ConnectionState
        {
            get;
            set;
        }


        /// <summary>
        /// Change the state of the connection.
        /// </summary>
        /// <param name="newstate">new state</param>
        internal StateSnapshot ChangeState(VPNConnectionState newstate)
        {
            StateSnapshot ev = null;
            StateSnapshot ret;
            lock (this)
            {
                ret = createSnapshotNoLock();
                if (ConnectionState != newstate)
                {
                    ConnectionState = newstate;
                    ev = createSnapshotNoLock();
                }
            }
            
            if (ev != null) 
                raiseEvents(ev);

            return ret;
        }

        /// <summary>
        /// Generates a snapshot without setting a lock
        /// </summary>
        /// <returns>a snapshot of the current state</returns>
        private StateSnapshot createSnapshotNoLock()
        {
            return new StateSnapshot() {
                ConnectionState = ConnectionState,
                IsStoppable = Stoppable,
                VPNState = new ReadOnlyCollection<string>(m_vpnstate)
            };
        }

        /// <summary>
        /// Generates a snapshot
        /// </summary>
        /// <returns>a snapshot of the current state</returns>
        public StateSnapshot CreateSnapshot()
        {
            StateSnapshot ret;
            lock (this)
            {
                ret = createSnapshotNoLock();
            }
            return ret;
        }

        internal void ChangeVPNState(string[] p)
        {
            StateSnapshot res;
            lock (this)
            {
                Array.Copy(p, m_vpnstate, 4);

                if (p[1] == "CONNECTED" || p[1] == "ASSIGN_IP")
                {
                    m_connection.IP = p[3];

                    if (p[1] == "CONNECTED")
                        ConnectionState = VPNConnectionState.Running;
                }
                else if (p[1] == "EXITING")
                {
                    m_connection.IP = null;
                }
                res = createSnapshotNoLock();
            }
            raiseEvents(res);
        }

        private void raiseEvents(StateSnapshot ea)
        {
            if (StateChanged != null && !m_connection.NoEvents)
                StateChanged(this, new StateChangedEventArgs(ea));
        }
    }
}
