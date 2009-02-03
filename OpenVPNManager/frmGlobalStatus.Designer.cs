namespace OpenVPNManager
{
    partial class frmGlobalStatus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGlobalStatus));
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
            this.pnlStatus.AccessibleDescription = null;
            this.pnlStatus.AccessibleName = null;
            resources.ApplyResources(this.pnlStatus, "pnlStatus");
            this.pnlStatus.BackgroundImage = null;
            this.pnlStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStatus.Font = null;
            this.pnlStatus.Name = "pnlStatus";
            this.toolTip.SetToolTip(this.pnlStatus, resources.GetString("pnlStatus.ToolTip"));
            this.pnlStatus.Resize += new System.EventHandler(this.pnlStatus_Resize);
            // 
            // contextMenu
            // 
            this.contextMenu.AccessibleDescription = null;
            this.contextMenu.AccessibleName = null;
            resources.ApplyResources(this.contextMenu, "contextMenu");
            this.contextMenu.BackgroundImage = null;
            this.contextMenu.Font = null;
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripSeparator2,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.contextMenu.Name = "contextMenuStrip1";
            this.toolTip.SetToolTip(this.contextMenu, resources.GetString("contextMenu.ToolTip"));
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.AccessibleDescription = null;
            this.statusToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.statusToolStripMenuItem, "statusToolStripMenuItem");
            this.statusToolStripMenuItem.BackgroundImage = null;
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.statusToolStripMenuItem.Click += new System.EventHandler(this.statusToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AccessibleDescription = null;
            this.toolStripSeparator1.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AccessibleDescription = null;
            this.toolStripSeparator2.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.AccessibleDescription = null;
            this.settingsToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.BackgroundImage = null;
            this.settingsToolStripMenuItem.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Settings;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.AccessibleDescription = null;
            this.aboutToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.BackgroundImage = null;
            this.aboutToolStripMenuItem.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Info;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.AccessibleDescription = null;
            this.quitToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.quitToolStripMenuItem, "quitToolStripMenuItem");
            this.quitToolStripMenuItem.BackgroundImage = null;
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // niIcon
            // 
            this.niIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            resources.ApplyResources(this.niIcon, "niIcon");
            this.niIcon.ContextMenuStrip = this.contextMenu;
            this.niIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.niIcon_MouseDoubleClick);
            // 
            // btnSettings
            // 
            this.btnSettings.AccessibleDescription = null;
            this.btnSettings.AccessibleName = null;
            resources.ApplyResources(this.btnSettings, "btnSettings");
            this.btnSettings.BackgroundImage = null;
            this.btnSettings.Font = null;
            this.btnSettings.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Settings;
            this.btnSettings.Name = "btnSettings";
            this.toolTip.SetToolTip(this.btnSettings, resources.GetString("btnSettings.ToolTip"));
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.AccessibleDescription = null;
            this.btnAbout.AccessibleName = null;
            resources.ApplyResources(this.btnAbout, "btnAbout");
            this.btnAbout.BackgroundImage = null;
            this.btnAbout.Font = null;
            this.btnAbout.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Info;
            this.btnAbout.Name = "btnAbout";
            this.toolTip.SetToolTip(this.btnAbout, resources.GetString("btnAbout.ToolTip"));
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.AccessibleDescription = null;
            this.btnQuit.AccessibleName = null;
            resources.ApplyResources(this.btnQuit, "btnQuit");
            this.btnQuit.BackgroundImage = null;
            this.btnQuit.Font = null;
            this.btnQuit.Name = "btnQuit";
            this.toolTip.SetToolTip(this.btnQuit, resources.GetString("btnQuit.ToolTip"));
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleDescription = null;
            this.btnClose.AccessibleName = null;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackgroundImage = null;
            this.btnClose.Font = null;
            this.btnClose.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Close;
            this.btnClose.Name = "btnClose";
            this.toolTip.SetToolTip(this.btnClose, resources.GetString("btnClose.ToolTip"));
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmGlobalStatus
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnAbout);
            this.Font = null;
            this.KeyPreview = true;
            this.Name = "frmGlobalStatus";
            this.toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Shown += new System.EventHandler(this.frmGlobalStatus_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGlobalStatus_FormClosing);
            this.Resize += new System.EventHandler(this.frmGlobalStatus_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmGlobalStatus_KeyDown);
            this.ResizeEnd += new System.EventHandler(this.frmGlobalStatus_ResizeEnd);
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