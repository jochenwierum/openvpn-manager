﻿using OpenVPNUtils;

namespace OpenVPNManager
{
    partial class FrmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSettings));
            this.btnClose = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkAllowCommandline = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbSmartCard = new System.Windows.Forms.CheckBox();
            this.btnClearOVPNDir = new System.Windows.Forms.Button();
            this.btnClearOVPNFile = new System.Windows.Forms.Button();
            this.llHowChange = new System.Windows.Forms.LinkLabel();
            this.lblServiceEnabled = new System.Windows.Forms.Label();
            this.txtOVPNManagereServiceConf = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtOVPNServiceExt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtOVPNServiceConf = new System.Windows.Forms.TextBox();
            this.llWhy = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.llDetect = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOVPNConf = new System.Windows.Forms.TextBox();
            this.txtOVPNFile = new System.Windows.Forms.TextBox();
            this.btnBrowseOVPNDir = new System.Windows.Forms.Button();
            this.btnBrowseOVPNFile = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbUpdate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkAutostart = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkStartMinimized = new System.Windows.Forms.CheckBox();
            this.numDbgLevel = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.RemoveServiceButton = new System.Windows.Forms.Button();
            this.UpdateServiceButton = new System.Windows.Forms.Button();
            this.InstallServiceButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDbgLevel)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Image = global::OpenVPNManager.Properties.Resources.BUTTON_Close;
            this.btnClose.Name = "btnClose";
            this.toolTip1.SetToolTip(this.btnClose, resources.GetString("btnClose.ToolTip"));
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.toolTip1.SetToolTip(this.tabControl1, resources.GetString("tabControl1.ToolTip"));
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.cbSmartCard);
            this.tabPage1.Controls.Add(this.btnClearOVPNDir);
            this.tabPage1.Controls.Add(this.btnClearOVPNFile);
            this.tabPage1.Controls.Add(this.llHowChange);
            this.tabPage1.Controls.Add(this.lblServiceEnabled);
            this.tabPage1.Controls.Add(this.txtOVPNManagereServiceConf);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.txtOVPNServiceExt);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.txtOVPNServiceConf);
            this.tabPage1.Controls.Add(this.llWhy);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.llDetect);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtOVPNConf);
            this.tabPage1.Controls.Add(this.txtOVPNFile);
            this.tabPage1.Controls.Add(this.btnBrowseOVPNDir);
            this.tabPage1.Controls.Add(this.btnBrowseOVPNFile);
            this.tabPage1.Name = "tabPage1";
            this.toolTip1.SetToolTip(this.tabPage1, resources.GetString("tabPage1.ToolTip"));
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbSmartCard
            // 
            resources.ApplyResources(this.cbSmartCard, "cbSmartCard");
            this.cbSmartCard.Checked = global::OpenVPNManager.Properties.Settings.Default.smartCardSupport;
            this.cbSmartCard.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::OpenVPNManager.Properties.Settings.Default, "smartCardSupport", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbSmartCard.Name = "cbSmartCard";
            this.toolTip1.SetToolTip(this.cbSmartCard, resources.GetString("cbSmartCard.ToolTip"));
            this.cbSmartCard.UseVisualStyleBackColor = true;
            // 
            // btnClearOVPNDir
            // 
            resources.ApplyResources(this.btnClearOVPNDir, "btnClearOVPNDir");
            this.btnClearOVPNDir.Name = "btnClearOVPNDir";
            this.toolTip1.SetToolTip(this.btnClearOVPNDir, resources.GetString("btnClearOVPNDir.ToolTip"));
            this.btnClearOVPNDir.UseVisualStyleBackColor = true;
            this.btnClearOVPNDir.Click += new System.EventHandler(this.btnClearOVPNDir_Click);
            // 
            // btnClearOVPNFile
            // 
            resources.ApplyResources(this.btnClearOVPNFile, "btnClearOVPNFile");
            this.btnClearOVPNFile.Name = "btnClearOVPNFile";
            this.toolTip1.SetToolTip(this.btnClearOVPNFile, resources.GetString("btnClearOVPNFile.ToolTip"));
            this.btnClearOVPNFile.UseVisualStyleBackColor = true;
            this.btnClearOVPNFile.Click += new System.EventHandler(this.btnClearOVPNFile_Click);
            // 
            // llHowChange
            // 
            resources.ApplyResources(this.llHowChange, "llHowChange");
            this.llHowChange.Name = "llHowChange";
            this.llHowChange.TabStop = true;
            this.toolTip1.SetToolTip(this.llHowChange, resources.GetString("llHowChange.ToolTip"));
            this.llHowChange.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llHowChange_LinkClicked);
            // 
            // lblServiceEnabled
            // 
            resources.ApplyResources(this.lblServiceEnabled, "lblServiceEnabled");
            this.lblServiceEnabled.Name = "lblServiceEnabled";
            this.toolTip1.SetToolTip(this.lblServiceEnabled, resources.GetString("lblServiceEnabled.ToolTip"));
            // 
            // txtOVPNManagereServiceConf
            // 
            resources.ApplyResources(this.txtOVPNManagereServiceConf, "txtOVPNManagereServiceConf");
            this.txtOVPNManagereServiceConf.Name = "txtOVPNManagereServiceConf";
            this.txtOVPNManagereServiceConf.ReadOnly = true;
            this.toolTip1.SetToolTip(this.txtOVPNManagereServiceConf, resources.GetString("txtOVPNManagereServiceConf.ToolTip"));
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            this.toolTip1.SetToolTip(this.label10, resources.GetString("label10.ToolTip"));
            // 
            // txtOVPNServiceExt
            // 
            resources.ApplyResources(this.txtOVPNServiceExt, "txtOVPNServiceExt");
            this.txtOVPNServiceExt.Name = "txtOVPNServiceExt";
            this.txtOVPNServiceExt.ReadOnly = true;
            this.toolTip1.SetToolTip(this.txtOVPNServiceExt, resources.GetString("txtOVPNServiceExt.ToolTip"));
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            this.toolTip1.SetToolTip(this.label9, resources.GetString("label9.ToolTip"));
            // 
            // txtOVPNServiceConf
            // 
            resources.ApplyResources(this.txtOVPNServiceConf, "txtOVPNServiceConf");
            this.txtOVPNServiceConf.Name = "txtOVPNServiceConf";
            this.txtOVPNServiceConf.ReadOnly = true;
            this.toolTip1.SetToolTip(this.txtOVPNServiceConf, resources.GetString("txtOVPNServiceConf.ToolTip"));
            // 
            // llWhy
            // 
            resources.ApplyResources(this.llWhy, "llWhy");
            this.llWhy.Name = "llWhy";
            this.llWhy.TabStop = true;
            this.toolTip1.SetToolTip(this.llWhy, resources.GetString("llWhy.ToolTip"));
            this.llWhy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llWhy_LinkClicked);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            this.toolTip1.SetToolTip(this.label8, resources.GetString("label8.ToolTip"));
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            this.toolTip1.SetToolTip(this.label7, resources.GetString("label7.ToolTip"));
            // 
            // llDetect
            // 
            resources.ApplyResources(this.llDetect, "llDetect");
            this.llDetect.Name = "llDetect";
            this.llDetect.TabStop = true;
            this.toolTip1.SetToolTip(this.llDetect, resources.GetString("llDetect.ToolTip"));
            this.llDetect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDetect_LinkClicked);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // txtOVPNConf
            // 
            resources.ApplyResources(this.txtOVPNConf, "txtOVPNConf");
            this.txtOVPNConf.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::OpenVPNManager.Properties.Settings.Default, "vpnconf", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOVPNConf.Name = "txtOVPNConf";
            this.txtOVPNConf.ReadOnly = true;
            this.txtOVPNConf.Text = global::OpenVPNManager.Properties.Settings.Default.vpnconf;
            this.toolTip1.SetToolTip(this.txtOVPNConf, resources.GetString("txtOVPNConf.ToolTip"));
            // 
            // txtOVPNFile
            // 
            resources.ApplyResources(this.txtOVPNFile, "txtOVPNFile");
            this.txtOVPNFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::OpenVPNManager.Properties.Settings.Default, "vpnbin", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtOVPNFile.Name = "txtOVPNFile";
            this.txtOVPNFile.ReadOnly = true;
            this.txtOVPNFile.Text = global::OpenVPNManager.Properties.Settings.Default.vpnbin;
            this.toolTip1.SetToolTip(this.txtOVPNFile, resources.GetString("txtOVPNFile.ToolTip"));
            // 
            // btnBrowseOVPNDir
            // 
            resources.ApplyResources(this.btnBrowseOVPNDir, "btnBrowseOVPNDir");
            this.btnBrowseOVPNDir.Name = "btnBrowseOVPNDir";
            this.toolTip1.SetToolTip(this.btnBrowseOVPNDir, resources.GetString("btnBrowseOVPNDir.ToolTip"));
            this.btnBrowseOVPNDir.UseVisualStyleBackColor = true;
            this.btnBrowseOVPNDir.Click += new System.EventHandler(this.btnBrowseOVPNDir_Click);
            // 
            // btnBrowseOVPNFile
            // 
            resources.ApplyResources(this.btnBrowseOVPNFile, "btnBrowseOVPNFile");
            this.btnBrowseOVPNFile.Name = "btnBrowseOVPNFile";
            this.toolTip1.SetToolTip(this.btnBrowseOVPNFile, resources.GetString("btnBrowseOVPNFile.ToolTip"));
            this.btnBrowseOVPNFile.UseVisualStyleBackColor = true;
            this.btnBrowseOVPNFile.Click += new System.EventHandler(this.btnBrowseOVPNFile_Click);
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Controls.Add(this.chkAllowCommandline);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.cmbUpdate);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.chkAutostart);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.chkStartMinimized);
            this.tabPage2.Controls.Add(this.numDbgLevel);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Name = "tabPage2";
            this.toolTip1.SetToolTip(this.tabPage2, resources.GetString("tabPage2.ToolTip"));
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            this.toolTip1.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // cmbUpdate
            // 
            resources.ApplyResources(this.cmbUpdate, "cmbUpdate");
            this.cmbUpdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            this.toolTip1.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
            // 
            // chkAutostart
            // 
            resources.ApplyResources(this.chkAutostart, "chkAutostart");
            this.chkAutostart.Name = "chkAutostart";
            this.toolTip1.SetToolTip(this.chkAutostart, resources.GetString("chkAutostart.ToolTip"));
            this.chkAutostart.UseVisualStyleBackColor = true;
            this.chkAutostart.Click += new System.EventHandler(this.chkAutostart_CheckedChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.toolTip1.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
            // 
            // chkStartMinimized
            // 
            resources.ApplyResources(this.chkStartMinimized, "chkStartMinimized");
            this.chkStartMinimized.Checked = global::OpenVPNManager.Properties.Settings.Default.startMinimized;
            this.chkStartMinimized.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStartMinimized.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::OpenVPNManager.Properties.Settings.Default, "startMinimized", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.toolTip1.SetToolTip(this.chkStartMinimized, resources.GetString("chkStartMinimized.ToolTip"));
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
            this.toolTip1.SetToolTip(this.numDbgLevel, resources.GetString("numDbgLevel.ToolTip"));
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // tabPage3
            // 
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Controls.Add(this.RemoveServiceButton);
            this.tabPage3.Controls.Add(this.UpdateServiceButton);
            this.tabPage3.Controls.Add(this.InstallServiceButton);
            this.tabPage3.Name = "tabPage3";
            this.toolTip1.SetToolTip(this.tabPage3, resources.GetString("tabPage3.ToolTip"));
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // RemoveServiceButton
            // 
            resources.ApplyResources(this.RemoveServiceButton, "RemoveServiceButton");
            this.RemoveServiceButton.Name = "RemoveServiceButton";
            this.toolTip1.SetToolTip(this.RemoveServiceButton, resources.GetString("RemoveServiceButton.ToolTip"));
            this.RemoveServiceButton.UseVisualStyleBackColor = true;
            this.RemoveServiceButton.Click += new System.EventHandler(this.RemoveServiceButton_Click);
            // 
            // UpdateServiceButton
            // 
            resources.ApplyResources(this.UpdateServiceButton, "UpdateServiceButton");
            this.UpdateServiceButton.Name = "UpdateServiceButton";
            this.toolTip1.SetToolTip(this.UpdateServiceButton, resources.GetString("UpdateServiceButton.ToolTip"));
            this.UpdateServiceButton.UseVisualStyleBackColor = true;
            this.UpdateServiceButton.Click += new System.EventHandler(this.UpdateServiceButton_Click);
            // 
            // InstallServiceButton
            // 
            resources.ApplyResources(this.InstallServiceButton, "InstallServiceButton");
            this.InstallServiceButton.Name = "InstallServiceButton";
            this.toolTip1.SetToolTip(this.InstallServiceButton, resources.GetString("InstallServiceButton.ToolTip"));
            this.InstallServiceButton.UseVisualStyleBackColor = true;
            this.InstallServiceButton.Click += new System.EventHandler(this.InstallServiceButton_Click);
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSettings_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSettings_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDbgLevel)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtOVPNServiceExt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtOVPNServiceConf;
        private System.Windows.Forms.LinkLabel llWhy;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel llDetect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOVPNConf;
        private System.Windows.Forms.TextBox txtOVPNFile;
        private System.Windows.Forms.Button btnBrowseOVPNDir;
        private System.Windows.Forms.Button btnBrowseOVPNFile;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chkAllowCommandline;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbUpdate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkAutostart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkStartMinimized;
        private System.Windows.Forms.NumericUpDown numDbgLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel llHowChange;
        private System.Windows.Forms.Label lblServiceEnabled;
        private System.Windows.Forms.Button btnClearOVPNDir;
        private System.Windows.Forms.Button btnClearOVPNFile;
        private System.Windows.Forms.CheckBox cbSmartCard;
        private System.Windows.Forms.TextBox txtOVPNManagereServiceConf;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button InstallServiceButton;
        private System.Windows.Forms.Button RemoveServiceButton;
        private System.Windows.Forms.Button UpdateServiceButton;

    }
}
