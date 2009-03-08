using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

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
        /// 
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VPN")]
        public ReadOnlyCollection<String> VPNState { get; internal set; }
    }
}
