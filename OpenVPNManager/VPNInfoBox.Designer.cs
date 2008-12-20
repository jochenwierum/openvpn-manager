namespace OpenVPNManager
{
    partial class VPNInfoBox
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VPNInfoBox));
            this.lblName = new System.Windows.Forms.Label();
            this.llReadError = new System.Windows.Forms.LinkLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.pbStatus = new System.Windows.Forms.PictureBox();
            this.llIP = new System.Windows.Forms.LinkLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyIPAndSubnetshortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyIPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyIPAndSubnetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AccessibleDescription = null;
            this.lblName.AccessibleName = null;
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Font = null;
            this.lblName.Name = "lblName";
            this.toolTip.SetToolTip(this.lblName, resources.GetString("lblName.ToolTip"));
            // 
            // llReadError
            // 
            this.llReadError.AccessibleDescription = null;
            this.llReadError.AccessibleName = null;
            resources.ApplyResources(this.llReadError, "llReadError");
            this.llReadError.Font = null;
            this.llReadError.Name = "llReadError";
            this.llReadError.TabStop = true;
            this.toolTip.SetToolTip(this.llReadError, resources.GetString("llReadError.ToolTip"));
            this.llReadError.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llReadError_LinkClicked);
            // 
            // btnEdit
            // 
            this.btnEdit.AccessibleDescription = null;
            this.btnEdit.AccessibleName = null;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.BackgroundImage = null;
            this.btnEdit.Font = null;
            this.btnEdit.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Edit;
            this.btnEdit.Name = "btnEdit";
            this.toolTip.SetToolTip(this.btnEdit, resources.GetString("btnEdit.ToolTip"));
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnShow
            // 
            this.btnShow.AccessibleDescription = null;
            this.btnShow.AccessibleName = null;
            resources.ApplyResources(this.btnShow, "btnShow");
            this.btnShow.BackgroundImage = null;
            this.btnShow.Font = null;
            this.btnShow.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Details;
            this.btnShow.Name = "btnShow";
            this.toolTip.SetToolTip(this.btnShow, resources.GetString("btnShow.ToolTip"));
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.AccessibleDescription = null;
            this.btnConnect.AccessibleName = null;
            resources.ApplyResources(this.btnConnect, "btnConnect");
            this.btnConnect.BackgroundImage = null;
            this.btnConnect.Font = null;
            this.btnConnect.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Connect;
            this.btnConnect.Name = "btnConnect";
            this.toolTip.SetToolTip(this.btnConnect, resources.GetString("btnConnect.ToolTip"));
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.AccessibleDescription = null;
            this.btnDisconnect.AccessibleName = null;
            resources.ApplyResources(this.btnDisconnect, "btnDisconnect");
            this.btnDisconnect.BackgroundImage = null;
            this.btnDisconnect.Font = null;
            this.btnDisconnect.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Disconnect;
            this.btnDisconnect.Name = "btnDisconnect";
            this.toolTip.SetToolTip(this.btnDisconnect, resources.GetString("btnDisconnect.ToolTip"));
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // pbStatus
            // 
            this.pbStatus.AccessibleDescription = null;
            this.pbStatus.AccessibleName = null;
            resources.ApplyResources(this.pbStatus, "pbStatus");
            this.pbStatus.BackgroundImage = null;
            this.pbStatus.Font = null;
            this.pbStatus.ImageLocation = null;
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.TabStop = false;
            this.toolTip.SetToolTip(this.pbStatus, resources.GetString("pbStatus.ToolTip"));
            // 
            // llIP
            // 
            this.llIP.AccessibleDescription = null;
            this.llIP.AccessibleName = null;
            this.llIP.ActiveLinkColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.llIP, "llIP");
            this.llIP.ContextMenuStrip = this.contextMenuStrip1;
            this.llIP.Font = null;
            this.llIP.Name = "llIP";
            this.llIP.TabStop = true;
            this.toolTip.SetToolTip(this.llIP, resources.GetString("llIP.ToolTip"));
            this.llIP.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llIP.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llIP_LinkClicked);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.AccessibleDescription = null;
            this.contextMenuStrip1.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.BackgroundImage = null;
            this.contextMenuStrip1.Font = null;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyIPAndSubnetshortToolStripMenuItem,
            this.copyIPToolStripMenuItem,
            this.copyIPAndSubnetToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.toolTip.SetToolTip(this.contextMenuStrip1, resources.GetString("contextMenuStrip1.ToolTip"));
            // 
            // copyIPAndSubnetshortToolStripMenuItem
            // 
            this.copyIPAndSubnetshortToolStripMenuItem.AccessibleDescription = null;
            this.copyIPAndSubnetshortToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.copyIPAndSubnetshortToolStripMenuItem, "copyIPAndSubnetshortToolStripMenuItem");
            this.copyIPAndSubnetshortToolStripMenuItem.BackgroundImage = null;
            this.copyIPAndSubnetshortToolStripMenuItem.Name = "copyIPAndSubnetshortToolStripMenuItem";
            this.copyIPAndSubnetshortToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.copyIPAndSubnetshortToolStripMenuItem.Click += new System.EventHandler(this.copyIPAndSubnetshortToolStripMenuItem_Click);
            // 
            // copyIPToolStripMenuItem
            // 
            this.copyIPToolStripMenuItem.AccessibleDescription = null;
            this.copyIPToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.copyIPToolStripMenuItem, "copyIPToolStripMenuItem");
            this.copyIPToolStripMenuItem.BackgroundImage = null;
            this.copyIPToolStripMenuItem.Name = "copyIPToolStripMenuItem";
            this.copyIPToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.copyIPToolStripMenuItem.Click += new System.EventHandler(this.copyIPToolStripMenuItem_Click);
            // 
            // copyIPAndSubnetToolStripMenuItem
            // 
            this.copyIPAndSubnetToolStripMenuItem.AccessibleDescription = null;
            this.copyIPAndSubnetToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.copyIPAndSubnetToolStripMenuItem, "copyIPAndSubnetToolStripMenuItem");
            this.copyIPAndSubnetToolStripMenuItem.BackgroundImage = null;
            this.copyIPAndSubnetToolStripMenuItem.Name = "copyIPAndSubnetToolStripMenuItem";
            this.copyIPAndSubnetToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.copyIPAndSubnetToolStripMenuItem.Click += new System.EventHandler(this.copyIPAndSubnetToolStripMenuItem_Click);
            // 
            // VPNInfoBox
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = null;
            this.Controls.Add(this.llIP);
            this.Controls.Add(this.pbStatus);
            this.Controls.Add(this.llReadError);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.lblName);
            this.Font = null;
            this.MinimumSize = new System.Drawing.Size(470, 29);
            this.Name = "VPNInfoBox";
            this.toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.LinkLabel llReadError;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pbStatus;
        private System.Windows.Forms.LinkLabel llIP;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyIPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyIPAndSubnetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyIPAndSubnetshortToolStripMenuItem;
    }
}
