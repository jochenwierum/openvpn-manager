using System.Diagnostics.CodeAnalysis;

namespace OpenVPN.States
{
    /// <summary>
    /// state of the VPN
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VPN")]
    public enum VPNConnectionState
    {
        /// <summary>
        /// OpenVPN is starting up.
        /// </summary>
        Initializing,

        /// <summary>
        /// OpenVPN is up and running.
        /// </summary>
        Running,

        /// <summary>
        /// OpenVPN is shutting down.
        /// </summary>
        Stopping,

        /// <summary>
        /// OpenVPN has stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// OpenVPN had an error while communicating with OpenVPN.
        /// </summary>
        Error
    }
}