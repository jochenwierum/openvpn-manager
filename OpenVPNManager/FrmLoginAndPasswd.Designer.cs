namespace OpenVPNManager
{
    partial class FrmLoginAndPasswd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLoginAndPasswd));
        	this.label2 = new System.Windows.Forms.Label();
        	this.label1 = new System.Windows.Forms.Label();
        	this.lblText = new System.Windows.Forms.Label();
        	this.btnCancel = new System.Windows.Forms.Button();
        	this.btnAccept = new System.Windows.Forms.Button();
        	this.txtPasswd = new System.Windows.Forms.TextBox();
        	this.txtUsername = new System.Windows.Forms.TextBox();
        	this.label3 = new System.Windows.Forms.Label();
        	this.lblAsk = new System.Windows.Forms.Label();
        	this.lblName = new System.Windows.Forms.Label();
        	this.chkRememberName = new System.Windows.Forms.CheckBox();
        	this.SuspendLayout();
        	// 
        	// label2
        	// 
        	resources.ApplyResources(this.label2, "label2");
        	this.label2.Name = "label2";
        	// 
        	// label1
        	// 
        	resources.ApplyResources(this.label1, "label1");
        	this.label1.Name = "label1";
        	// 
        	// lblText
        	// 
        	resources.ApplyResources(this.lblText, "lblText");
        	this.lblText.Name = "lblText";
        	// 
        	// btnCancel
        	// 
        	resources.ApplyResources(this.btnCancel, "btnCancel");
        	this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        	this.btnCancel.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Cancel;
        	this.btnCancel.Name = "btnCancel";
        	this.btnCancel.UseVisualStyleBackColor = true;
        	// 
        	// btnAccept
        	// 
        	resources.ApplyResources(this.btnAccept, "btnAccept");
        	this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.btnAccept.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Close;
        	this.btnAccept.Name = "btnAccept";
        	this.btnAccept.UseVisualStyleBackColor = true;
        	// 
        	// txtPasswd
        	// 
        	resources.ApplyResources(this.txtPasswd, "txtPasswd");
        	this.txtPasswd.Name = "txtPasswd";
        	this.txtPasswd.Enter += new System.EventHandler(this.txt_EnterSelectAll);
        	// 
        	// txtUsername
        	// 
        	resources.ApplyResources(this.txtUsername, "txtUsername");
        	this.txtUsername.Name = "txtUsername";
        	this.txtUsername.Enter += new System.EventHandler(this.txt_EnterSelectAll);
        	// 
        	// label3
        	// 
        	resources.ApplyResources(this.label3, "label3");
        	this.label3.Name = "label3";
        	// 
        	// lblAsk
        	// 
        	resources.ApplyResources(this.lblAsk, "lblAsk");
        	this.lblAsk.Name = "lblAsk";
        	// 
        	// lblName
        	// 
        	resources.ApplyResources(this.lblName, "lblName");
        	this.lblName.Name = "lblName";
        	// 
        	// chkRememberName
        	// 
        	resources.ApplyResources(this.chkRememberName, "chkRememberName");
        	this.chkRememberName.Name = "chkRememberName";
        	this.chkRememberName.UseVisualStyleBackColor = true;
        	// 
        	// FrmLoginAndPasswd
        	// 
        	this.AcceptButton = this.btnAccept;
        	resources.ApplyResources(this, "$this");
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.CancelButton = this.btnCancel;
        	this.Controls.Add(this.chkRememberName);
        	this.Controls.Add(this.lblName);
        	this.Controls.Add(this.lblAsk);
        	this.Controls.Add(this.txtUsername);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.lblText);
        	this.Controls.Add(this.btnCancel);
        	this.Controls.Add(this.btnAccept);
        	this.Controls.Add(this.txtPasswd);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "FrmLoginAndPasswd";
        	this.Shown += new System.EventHandler(this.FrmLoginAndPasswd_Shown);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.TextBox txtPasswd;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAsk;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chkRememberName;
    }
}
