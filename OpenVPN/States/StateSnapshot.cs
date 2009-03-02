using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace OpenVPN.States
{
    /// <summary>
    /// Represents a snapshot of the connection/OpenVPN state.
    /// </summary>
    public class StateSnapshot
    {
        /// <summary>
        /// The global state.
        /// </summary>
        public VPNConnectionState ConnectionState { get; internal set; }

        /// <summary>
        /// Can the state be changed?
        /// </summary>
        public bool IsStoppable { get; internal set; }

        /// <summary>
        /// The VPN state.
        /// </summary>
        public ReadOnlyCollection<String> VPNState { get; internal set; }
    }
}
