using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Forms;
using OpenVPNUtils;

namespace OpenVPNManager
{
    /// <summary>
    /// provides a form which gives the user the possibility to 
    /// select a smart card key
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "PKCS")]
    public partial class FrmSelectPKCS11Key : Form
    {
        #region variables
        /// <summary>
        /// the list of available keys
        /// </summary>
        private ReadOnlyCollection<PKCS11Detail> m_keys;
        #endregion

        #region constructor
        /// <summary>
        /// default constructor, does nothing
        /// </summary>
        public FrmSelectPKCS11Key()
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// Shows the form and waits for a selection.
        /// </summary>
        /// <param name="keylist">list of keys available</param>
        /// <param name="name">name of the config</param>
        /// <returns>number of the selected key, -1 on abort</returns>
        public int SelectKey(ReadOnlyCollection<PKCS11Detail> keylist, string name)
        {
            lstKeys.Items.Clear();
            lblAsk.Text = name;
            m_keys = keylist;

            foreach(PKCS11Detail d in keylist)
                lstKeys.Items.Add(d.NiceName);

            if (lstKeys.Items.Count > 0)
                lstKeys.SelectedIndex = 0;

            switch(this.ShowDialog())
            {
                case DialogResult.OK:
                    return lstKeys.SelectedIndex;
                case DialogResult.Retry:
                    return -2;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// A key was selected, display more information.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void lstKeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            StringBuilder text = new StringBuilder();
            foreach (var item in m_keys[lstKeys.SelectedIndex].IdParts)
	        {
		        text.Append(item);
                text.Append(Environment.NewLine);
	        }
            lblKeyDetail.Text = text.ToString();
        }

        /// <summary>
        /// Returns the number of the item under the mouse cursor.
        /// </summary>
        /// <param name="ypos">y Position of the mouse</param>
        /// <returns>the number of the item under the mouse cursor</returns>
        private int getItemAtPos(int ypos)
        {
            int item = lstKeys.TopIndex;
            int y = 0;

            // get the height of all items which are displayed
            while (item < lstKeys.Items.Count)
            {
                int h = lstKeys.GetItemHeight(item);
                
                // it this our item?
                if (ypos >= y && ypos < y + h)
                    return item;

                item++;
                y += h;
            }

            // no item was clicked (e.g. list is empty)
            return -1;
        }

        /// <summary>
        /// The list was double clicked.
        /// Close the form and return the double clicked item.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">holds the mouse position</param>
        private void lstKeys_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int item = getItemAtPos(e.Y);
            if (item != -1)
            {
                lstKeys.SelectedIndex = item;
                DialogResult = DialogResult.OK;
                Hide();
            }
        }
    }
}
