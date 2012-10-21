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
            this.pbStatus = new System.Windows.Forms.PictureBox();
            this.llIP = new OpenVPNManager.IPLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Name = "lblName";
            // 
            // llReadError
            // 
            resources.ApplyResources(this.llReadError, "llReadError");
            this.llReadError.Name = "llReadError";
            this.llReadError.TabStop = true;
            this.llReadError.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llReadError_LinkClicked);
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
            // btnShow
            // 
            resources.ApplyResources(this.btnShow, "btnShow");
            this.btnShow.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Details;
            this.btnShow.Name = "btnShow";
            this.toolTip.SetToolTip(this.btnShow, resources.GetString("btnShow.ToolTip"));
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
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
            // pbStatus
            // 
            resources.ApplyResources(this.pbStatus, "pbStatus");
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.TabStop = false;
            // 
            // llIP
            // 
            this.llIP.ActiveLinkColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.llIP, "llIP");
            this.llIP.Name = "llIP";
            this.llIP.TabStop = true;
            this.llIP.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // VPNInfoBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.llReadError);
            this.Controls.Add(this.llIP);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.pbStatus);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.btnConnect);
            this.MinimumSize = new System.Drawing.Size(470, 29);
            this.Name = "VPNInfoBox";
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.LinkLabel llReadError;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pbStatus;
        private IPLabel llIP;
    }
}
