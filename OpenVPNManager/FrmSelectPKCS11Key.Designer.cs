namespace OpenVPNManager
{
    partial class FrmSelectPKCS11Key
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSelectPKCS11Key));
        	this.lstKeys = new System.Windows.Forms.ListBox();
        	this.lblKeyDetail = new System.Windows.Forms.Label();
        	this.label2 = new System.Windows.Forms.Label();
        	this.lblAsk = new System.Windows.Forms.Label();
        	this.btnRefresh = new System.Windows.Forms.Button();
        	this.btnSelect = new System.Windows.Forms.Button();
        	this.btnAbort = new System.Windows.Forms.Button();
        	this.SuspendLayout();
        	// 
        	// lstKeys
        	// 
        	resources.ApplyResources(this.lstKeys, "lstKeys");
        	this.lstKeys.FormattingEnabled = true;
        	this.lstKeys.Name = "lstKeys";
        	this.lstKeys.SelectedIndexChanged += new System.EventHandler(this.lstKeys_SelectedIndexChanged);
        	this.lstKeys.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstKeys_MouseDoubleClick);
        	// 
        	// lblKeyDetail
        	// 
        	resources.ApplyResources(this.lblKeyDetail, "lblKeyDetail");
        	this.lblKeyDetail.Name = "lblKeyDetail";
        	// 
        	// label2
        	// 
        	resources.ApplyResources(this.label2, "label2");
        	this.label2.Name = "label2";
        	// 
        	// lblAsk
        	// 
        	resources.ApplyResources(this.lblAsk, "lblAsk");
        	this.lblAsk.Name = "lblAsk";
        	// 
        	// btnRefresh
        	// 
        	resources.ApplyResources(this.btnRefresh, "btnRefresh");
        	this.btnRefresh.DialogResult = System.Windows.Forms.DialogResult.Retry;
        	this.btnRefresh.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Refresh;
        	this.btnRefresh.Name = "btnRefresh";
        	this.btnRefresh.UseVisualStyleBackColor = true;
        	// 
        	// btnSelect
        	// 
        	resources.ApplyResources(this.btnSelect, "btnSelect");
        	this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.btnSelect.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Close;
        	this.btnSelect.Name = "btnSelect";
        	this.btnSelect.UseVisualStyleBackColor = true;
        	// 
        	// btnAbort
        	// 
        	resources.ApplyResources(this.btnAbort, "btnAbort");
        	this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        	this.btnAbort.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Cancel;
        	this.btnAbort.Name = "btnAbort";
        	this.btnAbort.UseVisualStyleBackColor = true;
        	// 
        	// FrmSelectPKCS11Key
        	// 
        	this.AcceptButton = this.btnSelect;
        	resources.ApplyResources(this, "$this");
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.CancelButton = this.btnAbort;
        	this.Controls.Add(this.btnRefresh);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.lblAsk);
        	this.Controls.Add(this.lblKeyDetail);
        	this.Controls.Add(this.btnAbort);
        	this.Controls.Add(this.btnSelect);
        	this.Controls.Add(this.lstKeys);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "FrmSelectPKCS11Key";
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox lstKeys;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Label lblKeyDetail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAsk;
        private System.Windows.Forms.Button btnRefresh;
    }
}
