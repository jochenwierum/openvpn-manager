using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using OpenVPN;
using System.Diagnostics.CodeAnalysis;

namespace OpenVPNManager
{
    /// <summary>
    /// displays the state of the connection, shows log, etc.
    /// </summary>
    public partial class FrmStatus : Form
    {
        /// <summary>
        /// represents a colored listbox entry
        /// </summary>
        private class ColoredListBoxItem
        {            
            /// <summary>
            /// creates a new ColoredListBoxItem
            /// </summary>
            /// <param name="prefix">the prefix which will be used</param>
            /// <param name="text">the real message</param>
            /// <param name="color">the color of both</param>
            public ColoredListBoxItem(string prefix, string text,
                Color color)
            {
                Text = text;
                Prefix = prefix;
                TextColor = color;
            }

            /// <summary>
            /// the prefix of the text
            /// </summary>
            public string Prefix { get; private set; }

            /// <summary>
            /// the real message
            /// </summary>
            public string Text { get; private set; }

            /// <summary>
            /// the color of the message
            /// </summary>
            public Color TextColor { get; private set; }
        }

        /// <summary>
        /// the config which belongs to the control
        /// </summary>
        private VPNConfig m_config;

        /// <summary>
        /// creates a new form
        /// </summary>
        /// <param name="config">parent config</param>
        public FrmStatus(VPNConfig config)
        {
            InitializeComponent();
            m_config = config;
        }
        
        /// <summary>
        /// (re)initialize the form;
        /// this is needed if the vpn changes
        /// </summary>
        public void Init()
        {
            lstLog.Items.Clear();

            m_config.VPNConnection.ConnectionStateChanged += new EventHandler(m_vpn_stateChanged);
            m_config.VPNConnection.VPNStateChanged += new EventHandler(vpn_vpnStateChanged);
            m_vpn_stateChanged(null, null);

            this.Text = "OpenVPN Manager [ " + m_config.Name + " ]";
            btnEdit.Enabled = !m_config.IsService;
        }

        /// <summary>
        /// vpn state changed. Assign the labels.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        void vpn_vpnStateChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(new EventHandler(vpn_vpnStateChanged), sender, e);
                }
                catch (ObjectDisposedException)
                { }
                return;
            }

            string text = m_config.VPNConnection.VPNState[1];
            text = Program.res.GetString("VPNSTATE_" + text);
            if (text != null)
            {
                if (text.StartsWith("VPNSTATE_", 
                    StringComparison.OrdinalIgnoreCase))
                {
                    text = m_config.VPNConnection.VPNState[1];
                }

                lblVPNState.Text = text;
            }
            else
            {
                lblVPNState.Text = "";
            }

            llIP.SetIP(m_config.VPNConnection.IP);
            lblVPNState.Left = llIP.Left - lblVPNState.Width - 16;
        }

        /// <summary>
        /// user wants to (dis)connect
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // connect only if we are disconnected, clear the list
            if (m_config.VPNConnection.State == VPNConnectionState.Stopped
                || m_config.VPNConnection.State == VPNConnectionState.Error)
            {
                lstLog.Items.Clear();
                m_config.Connect();
            }

            // disconnect only if we are connected
            else if (m_config.VPNConnection.State == VPNConnectionState.Initializing ||
                m_config.VPNConnection.State == VPNConnectionState.Running)
            {
                m_config.Disconnect();
            }
        }

        /// <summary>
        /// vpn state has changed, refresh all buttons/texts
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void m_vpn_stateChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(new EventHandler(m_vpn_stateChanged), sender, e);
                }
                catch (ObjectDisposedException)
                { }
                return;
            }

            llIP.SetIP(m_config.VPNConnection.IP);
            
            if(m_config.VPNConnection.State == VPNConnectionState.Initializing) {
                lstLog.Items.Clear();
                lblState.Text = Program.res.GetString("STATE_Initializing");
                pbStatus.Image = Properties.Resources.STATE_Initializing;
                toolTip.SetToolTip(btnConnect, 
                    Program.res.GetString("QUICKINFO_Disconnect"));
                btnConnect.Image = Properties.Resources.BUTTON_Disconnect;
                btnConnect.Enabled = false;
            } else if(m_config.VPNConnection.State == VPNConnectionState.Running) {
                lblState.Text = Program.res.GetString("STATE_Connected");
                pbStatus.Image = Properties.Resources.STATE_Running;
                toolTip.SetToolTip(btnConnect,
                    Program.res.GetString("QUICKINFO_Disconnect"));
                btnConnect.Image = Properties.Resources.BUTTON_Disconnect;
                btnConnect.Enabled = true;
            } else if(m_config.VPNConnection.State == VPNConnectionState.Stopped) {
                lblState.Text = Program.res.GetString("STATE_Stopped");
                pbStatus.Image = Properties.Resources.STATE_Stopped;
                toolTip.SetToolTip(btnConnect,
                    Program.res.GetString("QUICKINFO_Connect"));
                btnConnect.Image = Properties.Resources.BUTTON_Connect;
                btnConnect.Enabled = true;
                lblVPNState.Text = "";
            } else if(m_config.VPNConnection.State == VPNConnectionState.Stopping) {
                lblState.Text = Program.res.GetString("STATE_Stopping");
                pbStatus.Image = Properties.Resources.STATE_Stopping;
                toolTip.SetToolTip(btnConnect,
                    Program.res.GetString("QUICKINFO_Connect"));
                btnConnect.Image = Properties.Resources.BUTTON_Connect;
                btnConnect.Enabled = false;
            } else if(m_config.VPNConnection.State == VPNConnectionState.Error) {
                lblState.Text = Program.res.GetString("STATE_Error");
                pbStatus.Image = Properties.Resources.STATE_Error;
                toolTip.SetToolTip(btnConnect,
                    Program.res.GetString("QUICKINFO_Connect"));
                btnConnect.Image = Properties.Resources.BUTTON_Connect;
                btnConnect.Enabled = true;
                lblVPNState.Text = "";
            }
        }

        /// <summary>
        /// Delegate to addLog.
        /// </summary>
        /// <param name="p">type of log event</param>
        /// <param name="m">the message</param>
        private delegate void addLogDelegate(LogType p, string m);

        /// <summary>
        /// Add a log entry.
        /// </summary>
        /// <param name="prefix">type of log event</param>
        /// <param name="text">the message</param>
        public void AddLog(LogType prefix, string text)
        {
            if (lstLog.InvokeRequired)
            {
                try
                {
                    lstLog.BeginInvoke(new addLogDelegate(AddLog), prefix, text);
                }
                catch (ObjectDisposedException)
                {
                }
                return;
            }

            Color rowColor;
            switch (prefix)
            {
                case LogType.Management:
                    rowColor = Color.Green;
                    break;

                case LogType.Log:
                    rowColor = Color.DarkBlue;
                    break;

                case LogType.Debug:
                    rowColor = Color.Brown;
                    break;

                default: // e.g. State
                    rowColor = Color.Black;
                    break;
            }

            if (lstLog.Items.Count == 2048)
                lstLog.Items.RemoveAt(0);

            lstLog.Items.Add(new ColoredListBoxItem(prefix.ToString(),
               text, rowColor));
            
            int h = lstLog.ClientSize.Height - lstLog.Margin.Vertical;
            int i = lstLog.Items.Count - 1;
            while (h >= 0 && i > 0)
            {
                int nh = lstLog.GetItemHeight(i);

                if (nh > h)
                    break;
                else
                {
                    h -= nh;
                    i--;
                }
            }

            lstLog.TopIndex = i;
        }

        /// <summary>
        /// a listitem was been double clicked, show text in message box
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void lstLog_DoubleClick(object sender, EventArgs e)
        {
            // show the selected item
            if (lstLog.SelectedItem != null)
                RTLMessageBox.Show(this,
                    ((ColoredListBoxItem) lstLog.SelectedItem).Text,
                    MessageBoxIcon.Information);
        }

        /// <summary>
        /// user wants to close the form;
        /// this is "transformed" to hide
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">EventArguments used to prevent closing</param>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if the user closes the form via "x"...
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // cancel and hide
                e.Cancel = true;
                this.Hide();
                return;
            }
        }

        /// <summary>
        /// use wants to hide the form
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// listbox redraws an item
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">information about what should be drawed and where</param>
        private void lstLog_DrawItem(object sender, DrawItemEventArgs e)
        {
            // just in case the list is empty...
            if (e.Index == -1)
                return;

            // prefixes are drawed bold
            Font f = new Font(e.Font, FontStyle.Bold);

            ColoredListBoxItem li = (ColoredListBoxItem)
                ((ListBox)sender).Items[e.Index];

            Brush br = new SolidBrush(li.TextColor);

            // prepare the prefix
            string prefix = "[" + li.Prefix + "] ";

            e.DrawBackground();

            // draw the prefix
            e.Graphics.DrawString(prefix, f, br, e.Bounds,
                StringFormat.GenericDefault);

            // calculate the width of the longest prefix
            int w = (int)
                e.Graphics.MeasureString("[Management] ", e.Font, e.Bounds.Width,
                StringFormat.GenericDefault).Width;

            // calculate the new rectangle
            Rectangle newBounds = new Rectangle(e.Bounds.Location, 
                e.Bounds.Size);
            newBounds.X += w;
            newBounds.Width -= w;

            // draw the text
            e.Graphics.DrawString(
                li.Text, e.Font, br, newBounds,
                StringFormat.GenericDefault);

            // draw the focus
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// user wants to edit the configuration
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            m_config.Edit();
        }

        /// <summary>
        /// A button was pressed, perform/simulate the clicks.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">the pressed key</param>
        private void FrmStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                e.Handled = true;
                switch (e.KeyCode)
                {
                    case Keys.C:
                        btnClose_Click(null, null);
                        break;
                    case Keys.E:
                        btnEdit_Click(null, null);
                        break;
                    case Keys.O:
                        btnClose_Click(null, null);
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// The form was resized.
        /// Make shure, the list displays proper data.
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">ignored</param>
        private void FrmStatus_ResizeEnd(object sender, EventArgs e)
        {
            lstLog.Refresh();
        }
    }
}
