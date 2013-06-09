namespace OpenVPNManager
{
    partial class FrmStatus
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStatus));
        	this.btnConnect = new System.Windows.Forms.Button();
        	this.lstLog = new System.Windows.Forms.ListBox();
        	this.lblState = new System.Windows.Forms.Label();
        	this.btnClose = new System.Windows.Forms.Button();
        	this.btnEdit = new System.Windows.Forms.Button();
        	this.toolTip = new System.Windows.Forms.ToolTip(this.components);
        	this.pbStatus = new System.Windows.Forms.PictureBox();
        	this.lblVPNState = new System.Windows.Forms.Label();
        	this.llIP = new OpenVPNManager.IPLabel();
        	((System.ComponentModel.ISupportInitialize)(this.pbStatus)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// btnConnect
        	// 
        	resources.ApplyResources(this.btnConnect, "btnConnect");
        	this.btnConnect.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Connect;
        	this.btnConnect.Name = "btnConnect";
        	this.toolTip.SetToolTip(this.btnConnect, resources.GetString("btnConnect.ToolTip"));
        	this.btnConnect.UseVisualStyleBackColor = true;
        	this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
        	// 
        	// lstLog
        	// 
        	resources.ApplyResources(this.lstLog, "lstLog");
        	this.lstLog.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        	this.lstLog.FormattingEnabled = true;
        	this.lstLog.Name = "lstLog";
        	this.toolTip.SetToolTip(this.lstLog, resources.GetString("lstLog.ToolTip"));
        	this.lstLog.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstLog_DrawItem);
        	this.lstLog.DoubleClick += new System.EventHandler(this.lstLog_DoubleClick);
        	// 
        	// lblState
        	// 
        	resources.ApplyResources(this.lblState, "lblState");
        	this.lblState.Name = "lblState";
        	this.toolTip.SetToolTip(this.lblState, resources.GetString("lblState.ToolTip"));
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
        	// btnEdit
        	// 
        	resources.ApplyResources(this.btnEdit, "btnEdit");
        	this.btnEdit.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Edit;
        	this.btnEdit.Name = "btnEdit";
        	this.toolTip.SetToolTip(this.btnEdit, resources.GetString("btnEdit.ToolTip"));
        	this.btnEdit.UseVisualStyleBackColor = true;
        	this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
        	// 
        	// pbStatus
        	// 
        	resources.ApplyResources(this.pbStatus, "pbStatus");
        	this.pbStatus.Image = global::OpenVPNManager.Properties.Resources.STATE_Stopped;
        	this.pbStatus.Name = "pbStatus";
        	this.pbStatus.TabStop = false;
        	this.toolTip.SetToolTip(this.pbStatus, resources.GetString("pbStatus.ToolTip"));
        	// 
        	// lblVPNState
        	// 
        	resources.ApplyResources(this.lblVPNState, "lblVPNState");
        	this.lblVPNState.Name = "lblVPNState";
        	this.toolTip.SetToolTip(this.lblVPNState, resources.GetString("lblVPNState.ToolTip"));
        	// 
        	// llIP
        	// 
        	resources.ApplyResources(this.llIP, "llIP");
        	this.llIP.ActiveLinkColor = System.Drawing.Color.Blue;
        	this.llIP.Name = "llIP";
        	this.llIP.TabStop = true;
        	this.toolTip.SetToolTip(this.llIP, resources.GetString("llIP.ToolTip"));
        	this.llIP.VisitedLinkColor = System.Drawing.Color.Blue;
        	// 
        	// FrmStatus
        	// 
        	resources.ApplyResources(this, "$this");
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.Controls.Add(this.lblVPNState);
        	this.Controls.Add(this.llIP);
        	this.Controls.Add(this.pbStatus);
        	this.Controls.Add(this.btnEdit);
        	this.Controls.Add(this.btnClose);
        	this.Controls.Add(this.lblState);
        	this.Controls.Add(this.lstLog);
        	this.Controls.Add(this.btnConnect);
        	this.KeyPreview = true;
        	this.Name = "FrmStatus";
        	this.toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
        	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
        	this.ResizeEnd += new System.EventHandler(this.FrmStatus_ResizeEnd);
        	this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmStatus_KeyDown);
        	((System.ComponentModel.ISupportInitialize)(this.pbStatus)).EndInit();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pbStatus;
        private IPLabel llIP;
        private System.Windows.Forms.Label lblVPNState;
    }
}

