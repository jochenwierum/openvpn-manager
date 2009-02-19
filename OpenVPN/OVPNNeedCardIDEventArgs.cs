using System;

namespace OpenVPN
{
    /// <summary>
    /// holds details about available keys and the selected one
    /// </summary>
    public class OVPNNeedCardIDEventArgs : EventArgs
    {
        #region variables
        /// <summary>
        /// array of keys
        /// </summary>
        private PKCS11Detail[] m_details;

        /// <summary>
        /// number of the selected key or NONE
        /// </summary>
        private int m_selected;
        #endregion

        #region constants
        /// <summary>
        /// number of key, if no key is selected
        /// </summary>
        public const int NONE = -1;

        /// <summary>
        /// number of key, if OVPN should refresh ask for keys again;
        /// this is useful if the user pluggs in another smartcard and
        /// presses something like "retry"
        /// </summary>
        public const int RETRY = -2;
        #endregion

        #region constructor
        /// <summary>
        /// generates an event object
        /// </summary>
        /// <param name="details">array of available keys</param>
        internal OVPNNeedCardIDEventArgs(PKCS11Detail[] details)
        {
            // save list, select no default key
            m_details = details;
            m_selected = NONE;
        }
        #endregion

        /// <summary>
        /// the selected key <br />
        /// if no key is selected, the value is OVPNNeedCardIDEventArgs.NONE
        /// </summary>
        public int selectedID
        {
            get { return m_selected; }

            set
            {
                // was the given value valid?
                if (value != NONE && value != RETRY &&
                    (value < m_details.GetLowerBound(0) || 
                    value > m_details.GetUpperBound(0)))
                    throw new ArgumentException("Invalid ID");

                m_selected = value;
            }
        }

        /// <summary>
        /// readonly property, holds all key details
        /// </summary>
        public PKCS11Detail[] cardDetails
        {
            get { return m_details; }
        }
    }
}
