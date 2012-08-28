namespace OpenVPNManager
{
    partial class FrmGlobalStatus
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGlobalStatus));
            this.pnlStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.niIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlStatus
            // 
            resources.ApplyResources(this.pnlStatus, "pnlStatus");
            this.pnlStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Resize += new System.EventHandler(this.pnlStatus_Resize);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripSeparator2,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.contextMenu.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenu, "contextMenu");
            // 
            // statusToolStripMenuItem
            // 
            resources.ApplyResources(this.statusToolStripMenuItem, "statusToolStripMenuItem");
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Click += new System.EventHandler(this.statusToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Settings;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Info;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            resources.ApplyResources(this.quitToolStripMenuItem, "quitToolStripMenuItem");
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // niIcon
            // 
            this.niIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.niIcon.ContextMenuStrip = this.contextMenu;
            resources.ApplyResources(this.niIcon, "niIcon");
            this.niIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.niIcon_MouseDoubleClick);
            // 
            // btnSettings
            // 
            resources.ApplyResources(this.btnSettings, "btnSettings");
            this.btnSettings.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Settings;
            this.btnSettings.Name = "btnSettings";
            this.toolTip.SetToolTip(this.btnSettings, resources.GetString("btnSettings.ToolTip"));
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnAbout
            // 
            resources.ApplyResources(this.btnAbout, "btnAbout");
            this.btnAbout.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Info;
            this.btnAbout.Name = "btnAbout";
            this.toolTip.SetToolTip(this.btnAbout, resources.GetString("btnAbout.ToolTip"));
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnQuit
            // 
            resources.ApplyResources(this.btnQuit, "btnQuit");
            this.btnQuit.Name = "btnQuit";
            this.toolTip.SetToolTip(this.btnQuit, resources.GetString("btnQuit.ToolTip"));
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Close;
            this.btnClose.Name = "btnClose";
            this.toolTip.SetToolTip(this.btnClose, resources.GetString("btnClose.ToolTip"));
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmGlobalStatus
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnAbout);
            this.KeyPreview = true;
            this.Name = "FrmGlobalStatus";
            this.Opacity = 0D;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmGlobalStatus_FormClosing);
            this.Shown += new System.EventHandler(this.FrmGlobalStatus_Shown);
            this.ResizeEnd += new System.EventHandler(this.FrmGlobalStatus_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmGlobalStatus_KeyDown);
            this.Resize += new System.EventHandler(this.FrmGlobalStatus_Resize);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlStatus;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon niIcon;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
