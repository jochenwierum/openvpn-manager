namespace OpenVPNManager
{
    partial class FrmPasswd
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPasswd));
        	this.txtPasswd = new System.Windows.Forms.TextBox();
        	this.btnAccept = new System.Windows.Forms.Button();
        	this.btnCancel = new System.Windows.Forms.Button();
        	this.lblAsk = new System.Windows.Forms.Label();
        	this.lblText = new System.Windows.Forms.Label();
        	this.label1 = new System.Windows.Forms.Label();
        	this.label2 = new System.Windows.Forms.Label();
        	this.lblName = new System.Windows.Forms.Label();
        	this.SuspendLayout();
        	// 
        	// txtPasswd
        	// 
        	resources.ApplyResources(this.txtPasswd, "txtPasswd");
        	this.txtPasswd.Name = "txtPasswd";
        	// 
        	// btnAccept
        	// 
        	resources.ApplyResources(this.btnAccept, "btnAccept");
        	this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.btnAccept.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Close;
        	this.btnAccept.Name = "btnAccept";
        	this.btnAccept.UseVisualStyleBackColor = true;
        	// 
        	// btnCancel
        	// 
        	resources.ApplyResources(this.btnCancel, "btnCancel");
        	this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        	this.btnCancel.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Cancel;
        	this.btnCancel.Name = "btnCancel";
        	this.btnCancel.UseVisualStyleBackColor = true;
        	// 
        	// lblAsk
        	// 
        	resources.ApplyResources(this.lblAsk, "lblAsk");
        	this.lblAsk.Name = "lblAsk";
        	// 
        	// lblText
        	// 
        	resources.ApplyResources(this.lblText, "lblText");
        	this.lblText.Name = "lblText";
        	// 
        	// label1
        	// 
        	resources.ApplyResources(this.label1, "label1");
        	this.label1.Name = "label1";
        	// 
        	// label2
        	// 
        	resources.ApplyResources(this.label2, "label2");
        	this.label2.Name = "label2";
        	// 
        	// lblName
        	// 
        	resources.ApplyResources(this.lblName, "lblName");
        	this.lblName.Name = "lblName";
        	// 
        	// FrmPasswd
        	// 
        	this.AcceptButton = this.btnAccept;
        	resources.ApplyResources(this, "$this");
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.CancelButton = this.btnCancel;
        	this.Controls.Add(this.lblName);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.lblText);
        	this.Controls.Add(this.lblAsk);
        	this.Controls.Add(this.btnCancel);
        	this.Controls.Add(this.btnAccept);
        	this.Controls.Add(this.txtPasswd);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "FrmPasswd";
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtPasswd;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblAsk;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblName;
    }
}
