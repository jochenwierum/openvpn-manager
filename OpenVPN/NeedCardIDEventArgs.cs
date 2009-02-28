using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace OpenVPN
{
    /// <summary>
    /// holds details about available keys and the selected one
    /// </summary>
    /// 
    public class NeedCardIdEventArgs : EventArgs
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
        public const int None = -1;

        /// <summary>
        /// number of key, if OVPN should refresh ask for keys again;
        /// this is useful if the user pluggs in another smartcard and
        /// presses something like "retry"
        /// </summary>
        public const int Retry = -2;
        #endregion

        #region constructor
        /// <summary>
        /// generates an event object
        /// </summary>
        /// <param name="details">array of available keys</param>
        internal NeedCardIdEventArgs(PKCS11Detail[] details)
        {
            // save list, select no default key
            m_details = details;
            m_selected = None;
        }
        #endregion

        /// <summary>
        /// the selected key <br />
        /// if no key is selected, the value is OVPNNeedCardIDEventArgs.NONE
        /// </summary>
        public int SelectedId
        {
            get { return m_selected; }

            set
            {
                // was the given value valid?
                if (value != None && value != Retry &&
                    (value < m_details.GetLowerBound(0) || 
                    value > m_details.GetUpperBound(0)))
                    throw new ArgumentException("Invalid ID");

                m_selected = value;
            }
        }

        /// <summary>
        /// readonly property, holds all key details
        /// </summary>
        public ReadOnlyCollection<PKCS11Detail> CardDetails
        {
            get { return new ReadOnlyCollection<PKCS11Detail>(m_details); }
        }
    }
}
