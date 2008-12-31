namespace OpenVPNManager
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.llDetect = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOVPNConf = new System.Windows.Forms.TextBox();
            this.txtOVPNFile = new System.Windows.Forms.TextBox();
            this.btnBrowseOVPNDir = new System.Windows.Forms.Button();
            this.btnBrowseOVPNFile = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkAllowCommandline = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbUpdate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkAutostart = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkStartMinimized = new System.Windows.Forms.CheckBox();
            this.numDbgLevel = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDbgLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.llDetect);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtOVPNConf);
            this.groupBox1.Controls.Add(this.txtOVPNFile);
            this.groupBox1.Controls.Add(this.btnBrowseOVPNDir);
            this.groupBox1.Controls.Add(this.btnBrowseOVPNFile);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // llDetect
            // 
            resources.ApplyResources(this.llDetect, "llDetect");
            this.llDetect.Name = "llDetect";
            this.llDetect.TabStop = true;
            this.llDetect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDetect_LinkClicked);
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
            // txtOVPNConf
            // 
            this.txtOVPNConf.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::OpenVPNManager.Properties.Settings.Default, "vpnconf", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.txtOVPNConf, "txtOVPNConf");
            this.txtOVPNConf.Name = "txtOVPNConf";
            this.txtOVPNConf.ReadOnly = true;
            this.txtOVPNConf.Text = global::OpenVPNManager.Properties.Settings.Default.vpnconf;
            // 
            // txtOVPNFile
            // 
            this.txtOVPNFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::OpenVPNManager.Properties.Settings.Default, "vpnbin", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.txtOVPNFile, "txtOVPNFile");
            this.txtOVPNFile.Name = "txtOVPNFile";
            this.txtOVPNFile.ReadOnly = true;
            this.txtOVPNFile.Text = global::OpenVPNManager.Properties.Settings.Default.vpnbin;
            // 
            // btnBrowseOVPNDir
            // 
            resources.ApplyResources(this.btnBrowseOVPNDir, "btnBrowseOVPNDir");
            this.btnBrowseOVPNDir.Name = "btnBrowseOVPNDir";
            this.btnBrowseOVPNDir.UseVisualStyleBackColor = true;
            this.btnBrowseOVPNDir.Click += new System.EventHandler(this.btnBrowseOVPNDir_Click);
            // 
            // btnBrowseOVPNFile
            // 
            resources.ApplyResources(this.btnBrowseOVPNFile, "btnBrowseOVPNFile");
            this.btnBrowseOVPNFile.Name = "btnBrowseOVPNFile";
            this.btnBrowseOVPNFile.UseVisualStyleBackColor = true;
            this.btnBrowseOVPNFile.Click += new System.EventHandler(this.btnBrowseOVPNFile_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkAllowCommandline);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbUpdate);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.chkAutostart);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.chkStartMinimized);
            this.groupBox2.Controls.Add(this.numDbgLevel);
            this.groupBox2.Controls.Add(this.label3);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // chkAllowCommandline
            // 
            resources.ApplyResources(this.chkAllowCommandline, "chkAllowCommandline");
            this.chkAllowCommandline.Checked = global::OpenVPNManager.Properties.Settings.Default.allowRemoteControl;
            this.chkAllowCommandline.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::OpenVPNManager.Properties.Settings.Default, "allowRemoteControl", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAllowCommandline.Name = "chkAllowCommandline";
            this.toolTip1.SetToolTip(this.chkAllowCommandline, resources.GetString("chkAllowCommandline.ToolTip"));
            this.chkAllowCommandline.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmbUpdate
            // 
            this.cmbUpdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpdate.FormattingEnabled = true;
            this.cmbUpdate.Items.AddRange(new object[] {
            resources.GetString("cmbUpdate.Items"),
            resources.GetString("cmbUpdate.Items1"),
            resources.GetString("cmbUpdate.Items2"),
            resources.GetString("cmbUpdate.Items3")});
            resources.ApplyResources(this.cmbUpdate, "cmbUpdate");
            this.cmbUpdate.Name = "cmbUpdate";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // chkAutostart
            // 
            resources.ApplyResources(this.chkAutostart, "chkAutostart");
            this.chkAutostart.Name = "chkAutostart";
            this.chkAutostart.UseVisualStyleBackColor = true;
            this.chkAutostart.CheckedChanged += new System.EventHandler(this.chkAutostart_CheckedChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkStartMinimized
            // 
            resources.ApplyResources(this.chkStartMinimized, "chkStartMinimized");
            this.chkStartMinimized.Checked = global::OpenVPNManager.Properties.Settings.Default.startMinimized;
            this.chkStartMinimized.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStartMinimized.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::OpenVPNManager.Properties.Settings.Default, "startMinimized", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.UseVisualStyleBackColor = true;
            // 
            // numDbgLevel
            // 
            resources.ApplyResources(this.numDbgLevel, "numDbgLevel");
            this.numDbgLevel.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numDbgLevel.Name = "numDbgLevel";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSettings_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDbgLevel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel llDetect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOVPNConf;
        private System.Windows.Forms.TextBox txtOVPNFile;
        private System.Windows.Forms.Button btnBrowseOVPNDir;
        private System.Windows.Forms.Button btnBrowseOVPNFile;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbUpdate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkAutostart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkStartMinimized;
        private System.Windows.Forms.NumericUpDown numDbgLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkAllowCommandline;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label6;

    }
}