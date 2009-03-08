using System;

namespace OpenVPN.States
{
    /// <summary>
    /// Holds a threadsafe snapshot of the new state
    /// </summary>
    public class StateChangedEventArgs : EventArgs
    {
        private StateSnapshot m_state;

        /// <summary>
        /// Generates the new object.
        /// </summary>
        /// <param name="newState">The snapshot to return</param>
        internal StateChangedEventArgs(StateSnapshot newState)
        {
            m_state = newState;
        }

        /// <summary>
        /// A Snapshot of the new state.
        /// </summary>
        public StateSnapshot NewState {
            get { return m_state; } 
        }
    }
}
