using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace OpenVPN
{
    /// <summary>
    /// holds information about a smartcard key
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "PKCS")]
    public class PKCS11Detail
    {
        #region variables
        /// <summary>
        /// internal card number of key
        /// </summary>
        private int m_nr;

        /// <summary>
        /// internal id of the key
        /// </summary>
        private string m_id;

        /// <summary>
        /// blob of pkcs15-helper
        /// </summary>
        private string m_blob;

        /// <summary>
        /// nice name of the key
        /// </summary>
        private string m_nicename;

        /// <summary>
        /// parts of id, decoded
        /// </summary>
        private string[] m_parts;
        #endregion

        /// <summary>
        /// decodes a string which is returned by pkcs-id-get
        /// </summary>
        /// <param name="x">encoded string</param>
        /// <returns>decoded string</returns>
        private static string decode(string x)
        {
            StringBuilder ret = new StringBuilder();
            string tmp = x;

            while (tmp.Length > 0)
            {
                // start of a string like \x20
                // this is a hex-encoded ascii-char
                if(tmp.StartsWith("\\x", StringComparison.OrdinalIgnoreCase))
                {
                    // remove \x
                    tmp = tmp.Remove(0, 2);
                    
                    // get the number (e.g. "20")
                    Char[] c = new Char[2];
                    tmp.CopyTo(0, c, 0, 2);

                    // convert the number into a char
                    // Utf32 should be no problem here,
                    // because the first 265 codepoints are the same
                    ret.Append(
                        Char.ConvertFromUtf32(
                        Int16.Parse(tmp.Substring(0, 2), 
                        NumberStyles.HexNumber,
                        CultureInfo.InvariantCulture.NumberFormat)
                        ));

                    // remove the number (e.g. "20")
                    tmp = tmp.Remove(0, 2);
                }
                else
                {
                    // append the char
                    ret.Append(tmp[0]);
                    tmp = tmp.Remove(0, 1);
                }
            }

            return ret.ToString();
        }

        /// <summary>
        /// generate the object
        /// </summary>
        /// <param name="nr">external number</param>
        /// <param name="id">
        /// internal id, used by OpenVPN to identify the objected
        /// </param>
        /// <param name="blob">blob</param>
        internal PKCS11Detail(int nr, string id, string blob)
        {
            m_nr = nr;
            m_id = id;
            m_blob = blob;

            m_parts = id.Split(new Char[] { '/' });
            for(int i = 0; i < m_parts.GetUpperBound(0); ++i)
                m_parts[i] = decode(m_parts[i]);

            m_nicename = decode(m_parts[3]) + " [" + decode(m_parts[4]) + "]";
        }

        /// <summary>
        /// get the external number used by OVPN to identify this object
        /// </summary>
        public int Number
        {
            get { return m_nr; }
        }

        /// <summary>
        /// get the internal id used by OVPN to determine this object
        /// </summary>
        public string Id
        {
            get { return m_id; }
        }

        /// <summary>
        /// get a nice name for this object
        /// </summary>
        public string NiceName
        {
            get { return m_nicename; }
        }

        /// <summary>
        /// get the blob of this object
        /// </summary>
        public string Blob
        {
            get { return m_blob; }
        }

        /// <summary>
        /// returns the decrypted parts of the id
        /// </summary>
        public ReadOnlyCollection<string> IdParts
        {
            get { return new ReadOnlyCollection<string>(m_parts); }
        }
    }
}
