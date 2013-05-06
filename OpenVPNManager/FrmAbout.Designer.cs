namespace OpenVPNManager
{
    partial class FrmAbout
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
        	this.btnClose = new System.Windows.Forms.Button();
        	this.lblName = new System.Windows.Forms.Label();
        	this.panel1 = new System.Windows.Forms.Panel();
        	this.label2 = new System.Windows.Forms.Label();
        	this.linkLabel2 = new System.Windows.Forms.LinkLabel();
        	this.linkLabel1 = new System.Windows.Forms.LinkLabel();
        	this.label1 = new System.Windows.Forms.Label();
        	this.btnUpdateCheck = new System.Windows.Forms.Button();
        	this.toolTip = new System.Windows.Forms.ToolTip(this.components);
        	this.panel1.SuspendLayout();
        	this.SuspendLayout();
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
        	// lblName
        	// 
        	resources.ApplyResources(this.lblName, "lblName");
        	this.lblName.Name = "lblName";
        	this.toolTip.SetToolTip(this.lblName, resources.GetString("lblName.ToolTip"));
        	// 
        	// panel1
        	// 
        	resources.ApplyResources(this.panel1, "panel1");
        	this.panel1.Controls.Add(this.label2);
        	this.panel1.Controls.Add(this.linkLabel2);
        	this.panel1.Controls.Add(this.linkLabel1);
        	this.panel1.Controls.Add(this.label1);
        	this.panel1.Name = "panel1";
        	this.toolTip.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
        	// 
        	// label2
        	// 
        	resources.ApplyResources(this.label2, "label2");
        	this.label2.MaximumSize = new System.Drawing.Size(200, 0);
        	this.label2.Name = "label2";
        	this.toolTip.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
        	// 
        	// linkLabel2
        	// 
        	resources.ApplyResources(this.linkLabel2, "linkLabel2");
        	this.linkLabel2.Name = "linkLabel2";
        	this.linkLabel2.TabStop = true;
        	this.toolTip.SetToolTip(this.linkLabel2, resources.GetString("linkLabel2.ToolTip"));
        	this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
        	// 
        	// linkLabel1
        	// 
        	resources.ApplyResources(this.linkLabel1, "linkLabel1");
        	this.linkLabel1.Name = "linkLabel1";
        	this.linkLabel1.TabStop = true;
        	this.toolTip.SetToolTip(this.linkLabel1, resources.GetString("linkLabel1.ToolTip"));
        	this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
        	// 
        	// label1
        	// 
        	resources.ApplyResources(this.label1, "label1");
        	this.label1.MaximumSize = new System.Drawing.Size(200, 0);
        	this.label1.Name = "label1";
        	this.toolTip.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
        	// 
        	// btnUpdateCheck
        	// 
        	resources.ApplyResources(this.btnUpdateCheck, "btnUpdateCheck");
        	this.btnUpdateCheck.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Update;
        	this.btnUpdateCheck.Name = "btnUpdateCheck";
        	this.toolTip.SetToolTip(this.btnUpdateCheck, resources.GetString("btnUpdateCheck.ToolTip"));
        	this.btnUpdateCheck.UseVisualStyleBackColor = true;
        	this.btnUpdateCheck.Click += new System.EventHandler(this.btnUpdateCheck_Click);
        	// 
        	// FrmAbout
        	// 
        	resources.ApplyResources(this, "$this");
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ControlBox = false;
        	this.Controls.Add(this.btnUpdateCheck);
        	this.Controls.Add(this.panel1);
        	this.Controls.Add(this.lblName);
        	this.Controls.Add(this.btnClose);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.KeyPreview = true;
        	this.Name = "FrmAbout";
        	this.toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
        	this.Load += new System.EventHandler(this.FrmAbout_Load);
        	this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmAbout_KeyDown);
        	this.panel1.ResumeLayout(false);
        	this.panel1.PerformLayout();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUpdateCheck;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
