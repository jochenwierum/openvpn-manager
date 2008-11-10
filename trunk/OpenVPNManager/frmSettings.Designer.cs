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
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.llDetect);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtOVPNConf);
            this.groupBox1.Controls.Add(this.txtOVPNFile);
            this.groupBox1.Controls.Add(this.btnBrowseOVPNDir);
            this.groupBox1.Controls.Add(this.btnBrowseOVPNFile);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // llDetect
            // 
            this.llDetect.AccessibleDescription = null;
            this.llDetect.AccessibleName = null;
            resources.ApplyResources(this.llDetect, "llDetect");
            this.llDetect.Font = null;
            this.llDetect.Name = "llDetect";
            this.llDetect.TabStop = true;
            this.toolTip1.SetToolTip(this.llDetect, resources.GetString("llDetect.ToolTip"));
            this.llDetect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDetect_LinkClicked);
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // txtOVPNConf
            // 
            this.txtOVPNConf.AccessibleDescription = null;
            this.txtOVPNConf.AccessibleName = null;
            resources.ApplyResources(this.txtOVPNConf, "txtOVPNConf");
            this.txtOVPNConf.BackgroundImage = null;
            this.txtOVPNConf.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::OpenVPNManager.Properties.Settings.Default, "vpnconf", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOVPNConf.Font = null;
            this.txtOVPNConf.Name = "txtOVPNConf";
            this.txtOVPNConf.ReadOnly = true;
            this.txtOVPNConf.Text = global::OpenVPNManager.Properties.Settings.Default.vpnconf;
            this.toolTip1.SetToolTip(this.txtOVPNConf, resources.GetString("txtOVPNConf.ToolTip"));
            // 
            // txtOVPNFile
            // 
            this.txtOVPNFile.AccessibleDescription = null;
            this.txtOVPNFile.AccessibleName = null;
            resources.ApplyResources(this.txtOVPNFile, "txtOVPNFile");
            this.txtOVPNFile.BackgroundImage = null;
            this.txtOVPNFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::OpenVPNManager.Properties.Settings.Default, "vpnbin", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOVPNFile.Font = null;
            this.txtOVPNFile.Name = "txtOVPNFile";
            this.txtOVPNFile.ReadOnly = true;
            this.txtOVPNFile.Text = global::OpenVPNManager.Properties.Settings.Default.vpnbin;
            this.toolTip1.SetToolTip(this.txtOVPNFile, resources.GetString("txtOVPNFile.ToolTip"));
            // 
            // btnBrowseOVPNDir
            // 
            this.btnBrowseOVPNDir.AccessibleDescription = null;
            this.btnBrowseOVPNDir.AccessibleName = null;
            resources.ApplyResources(this.btnBrowseOVPNDir, "btnBrowseOVPNDir");
            this.btnBrowseOVPNDir.BackgroundImage = null;
            this.btnBrowseOVPNDir.Font = null;
            this.btnBrowseOVPNDir.Name = "btnBrowseOVPNDir";
            this.toolTip1.SetToolTip(this.btnBrowseOVPNDir, resources.GetString("btnBrowseOVPNDir.ToolTip"));
            this.btnBrowseOVPNDir.UseVisualStyleBackColor = true;
            this.btnBrowseOVPNDir.Click += new System.EventHandler(this.btnBrowseOVPNDir_Click);
            // 
            // btnBrowseOVPNFile
            // 
            this.btnBrowseOVPNFile.AccessibleDescription = null;
            this.btnBrowseOVPNFile.AccessibleName = null;
            resources.ApplyResources(this.btnBrowseOVPNFile, "btnBrowseOVPNFile");
            this.btnBrowseOVPNFile.BackgroundImage = null;
            this.btnBrowseOVPNFile.Font = null;
            this.btnBrowseOVPNFile.Name = "btnBrowseOVPNFile";
            this.toolTip1.SetToolTip(this.btnBrowseOVPNFile, resources.GetString("btnBrowseOVPNFile.ToolTip"));
            this.btnBrowseOVPNFile.UseVisualStyleBackColor = true;
            this.btnBrowseOVPNFile.Click += new System.EventHandler(this.btnBrowseOVPNFile_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleDescription = null;
            this.btnClose.AccessibleName = null;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackgroundImage = null;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Font = null;
            this.btnClose.Name = "btnClose";
            this.toolTip1.SetToolTip(this.btnClose, resources.GetString("btnClose.ToolTip"));
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = null;
            this.groupBox2.AccessibleName = null;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackgroundImage = null;
            this.groupBox2.Controls.Add(this.chkAllowCommandline);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbUpdate);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.chkAutostart);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.chkStartMinimized);
            this.groupBox2.Controls.Add(this.numDbgLevel);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Font = null;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
            // 
            // chkAllowCommandline
            // 
            this.chkAllowCommandline.AccessibleDescription = null;
            this.chkAllowCommandline.AccessibleName = null;
            resources.ApplyResources(this.chkAllowCommandline, "chkAllowCommandline");
            this.chkAllowCommandline.BackgroundImage = null;
            this.chkAllowCommandline.Checked = global::OpenVPNManager.Properties.Settings.Default.allowRemoteControl;
            this.chkAllowCommandline.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::OpenVPNManager.Properties.Settings.Default, "allowRemoteControl", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAllowCommandline.Font = null;
            this.chkAllowCommandline.Name = "chkAllowCommandline";
            this.toolTip1.SetToolTip(this.chkAllowCommandline, resources.GetString("chkAllowCommandline.ToolTip"));
            this.chkAllowCommandline.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AccessibleDescription = null;
            this.label6.AccessibleName = null;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Font = null;
            this.label6.Name = "label6";
            this.toolTip1.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // cmbUpdate
            // 
            this.cmbUpdate.AccessibleDescription = null;
            this.cmbUpdate.AccessibleName = null;
            resources.ApplyResources(this.cmbUpdate, "cmbUpdate");
            this.cmbUpdate.BackgroundImage = null;
            this.cmbUpdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUpdate.Font = null;
            this.cmbUpdate.FormattingEnabled = true;
            this.cmbUpdate.Items.AddRange(new object[] {
            resources.GetString("cmbUpdate.Items"),
            resources.GetString("cmbUpdate.Items1"),
            resources.GetString("cmbUpdate.Items2"),
            resources.GetString("cmbUpdate.Items3")});
            this.cmbUpdate.Name = "cmbUpdate";
            this.toolTip1.SetToolTip(this.cmbUpdate, resources.GetString("cmbUpdate.ToolTip"));
            // 
            // label5
            // 
            this.label5.AccessibleDescription = null;
            this.label5.AccessibleName = null;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Font = null;
            this.label5.Name = "label5";
            this.toolTip1.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
            // 
            // chkAutostart
            // 
            this.chkAutostart.AccessibleDescription = null;
            this.chkAutostart.AccessibleName = null;
            resources.ApplyResources(this.chkAutostart, "chkAutostart");
            this.chkAutostart.BackgroundImage = null;
            this.chkAutostart.Font = null;
            this.chkAutostart.Name = "chkAutostart";
            this.toolTip1.SetToolTip(this.chkAutostart, resources.GetString("chkAutostart.ToolTip"));
            this.chkAutostart.UseVisualStyleBackColor = true;
            this.chkAutostart.CheckedChanged += new System.EventHandler(this.chkAutostart_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Font = null;
            this.label4.Name = "label4";
            this.toolTip1.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
            // 
            // chkStartMinimized
            // 
            this.chkStartMinimized.AccessibleDescription = null;
            this.chkStartMinimized.AccessibleName = null;
            resources.ApplyResources(this.chkStartMinimized, "chkStartMinimized");
            this.chkStartMinimized.BackgroundImage = null;
            this.chkStartMinimized.Checked = global::OpenVPNManager.Properties.Settings.Default.startMinimized;
            this.chkStartMinimized.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStartMinimized.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::OpenVPNManager.Properties.Settings.Default, "startMinimized", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkStartMinimized.Font = null;
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.toolTip1.SetToolTip(this.chkStartMinimized, resources.GetString("chkStartMinimized.ToolTip"));
            this.chkStartMinimized.UseVisualStyleBackColor = true;
            // 
            // numDbgLevel
            // 
            this.numDbgLevel.AccessibleDescription = null;
            this.numDbgLevel.AccessibleName = null;
            resources.ApplyResources(this.numDbgLevel, "numDbgLevel");
            this.numDbgLevel.Font = null;
            this.numDbgLevel.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numDbgLevel.Name = "numDbgLevel";
            this.toolTip1.SetToolTip(this.numDbgLevel, resources.GetString("numDbgLevel.ToolTip"));
            // 
            // label3
            // 
            this.label3.AccessibleDescription = null;
            this.label3.AccessibleName = null;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Font = null;
            this.label3.Name = "label3";
            this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnClose;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
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