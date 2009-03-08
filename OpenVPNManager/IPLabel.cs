using System;
using System.Windows.Forms;

namespace OpenVPNManager
{
    public class IPLabel : LinkLabel
    {
        /// <summary>
        /// holds the context menu
        /// </summary>
        private ContextMenuStrip contextMenu;

        /// <summary>
        /// "copy ip" menu item
        /// </summary>
        private ToolStripMenuItem copyIPToolStripMenuItem;

        /*/// <summary>
        /// "copy ip and subnet in extended format" menu item
        /// </summary>
        private ToolStripMenuItem copyIPAndSubnetToolStripMenuItem;*/

        /*/// <summary>
        /// "copy ip and subnet" menu item
        /// </summary>
        private ToolStripMenuItem copyIPAndSubnetshortToolStripMenuItem;*/

        /// <summary>
        /// the ip to display
        /// </summary>
        private string ip;

        /// <summary>
        /// Constructor
        /// </summary>
        public IPLabel() : base()
        {

            /*copyIPAndSubnetshortToolStripMenuItem = new ToolStripMenuItem(
                Program.res.GetString("DIALOG_IP_CopyAll"));
            copyIPAndSubnetshortToolStripMenuItem.Click += 
                new System.EventHandler(copyIPAndSubnetshortToolStripMenuItem_Click);*/

            copyIPToolStripMenuItem = new ToolStripMenuItem(
                Program.res.GetString("DIALOG_IP_Copy"));
            copyIPToolStripMenuItem.Click += 
                new System.EventHandler(copyIPToolStripMenuItem_Click);

            /*copyIPAndSubnetToolStripMenuItem = new ToolStripMenuItem(
                Program.res.GetString("DIALOG_IP_CopySN"));
            copyIPAndSubnetToolStripMenuItem.Click += 
                new System.EventHandler(copyIPAndSubnetToolStripMenuItem_Click);*/

            contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripItem[] {
                //copyIPAndSubnetshortToolStripMenuItem,
                copyIPToolStripMenuItem,
                //copyIPAndSubnetToolStripMenuItem
            });

            ContextMenuStrip = contextMenu;
            VisitedLinkColor = System.Drawing.Color.Blue;
            ActiveLinkColor = System.Drawing.Color.Blue;
            Text = "";
            Visible = false;
            LinkClicked += new LinkLabelLinkClickedEventHandler(IPLabel_LinkClicked);
        }

        /// <summary>
        /// Link was clicked, show menu
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        void IPLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            contextMenu.Show(this, 0, Height);
        }

        /// <summary>
        /// Refresh IP. If the IP is null, the control becomes invisible.
        /// </summary>
        /// <param name="ip">the ip in form w.x.y.z/sn or null</param>
        public void SetIP(string ip)
        {
            Visible = (ip != null);
            Text = ip;
            this.ip = ip;
        }

        /// <summary>
        /// Copys the value as it is into the clipboard.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void copyIPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(ip, TextDataFormat.Text);
        }

        /*/// <summary>
        /// Copys only the ip into the clipboard.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void copyIPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string iponly = ip.Substring(0, ip.IndexOf('/'));
            Clipboard.Clear();
            Clipboard.SetText(iponly, TextDataFormat.Text);
        }

        /// <summary>
        /// Generates a subnetmask from given bits.
        /// E.g. 24 becomes "255.255.255.0".
        /// </summary>
        /// <param name="bits">Number of subnet bits</param>
        /// <returns>Subnet mask in readable form</returns>
        private string getSubnet(int bits)
        {
            uint subnet = 0;
            string result = "";

            for (int i = 31; i > 31 - bits; --i)
                subnet = subnet | (uint)Math.Pow(2, i);

            for (int i = 0; i < 4; ++i)
            {
                result = (subnet % 256) + "." + result;
                subnet /= 256;
            }

            return result.Substring(0, result.Length - 1);
        }

        /// <summary>
        /// Copys IP an Subnet in separate lines.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void copyIPAndSubnetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string iponly = ip.Substring(0, ip.IndexOf('/'));
            int subnet = int.Parse(ip.Substring(iponly.Length + 1));
            Clipboard.Clear();
            Clipboard.SetText("IP: " + iponly + Environment.NewLine +
                "Subnet: " + getSubnet(subnet), TextDataFormat.Text);
        }*/
    }
}
