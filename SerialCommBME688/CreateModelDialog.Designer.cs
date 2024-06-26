﻿namespace SamplingBME688Serial
{
    partial class CreateModelDialog
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
            btnLoadModel = new Button();
            btnSaveModel = new Button();
            btnCreateModel = new Button();
            grpInformation = new GroupBox();
            txtMessage = new TextBox();
            grpSensor = new GroupBox();
            selSensor2 = new RadioButton();
            selSensor1and2 = new RadioButton();
            selSensor1or2 = new RadioButton();
            selSensor1 = new RadioButton();
            lblModel = new Label();
            cmbModel = new ComboBox();
            lblCategory = new Label();
            txtCategory = new TextBox();
            btnSelectCategory = new Button();
            lblFrom = new Label();
            rangeFromPercent = new NumericUpDown();
            rangeToPercent = new NumericUpDown();
            lblTo = new Label();
            lblDuplicate = new Label();
            selDuplicate = new ComboBox();
            grpData = new GroupBox();
            chkPresTempHumidity = new CheckBox();
            chkDataLog = new CheckBox();
            txtDataCount = new TextBox();
            lblDataCount = new Label();
            txtResult = new TextBox();
            cmbBinaryModel = new ComboBox();
            btnClearMessage = new Button();
            grpInformation.SuspendLayout();
            grpSensor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rangeFromPercent).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rangeToPercent).BeginInit();
            grpData.SuspendLayout();
            SuspendLayout();
            // 
            // btnLoadModel
            // 
            btnLoadModel.Location = new Point(13, 170);
            btnLoadModel.Margin = new Padding(2);
            btnLoadModel.Name = "btnLoadModel";
            btnLoadModel.Size = new Size(105, 25);
            btnLoadModel.TabIndex = 10;
            btnLoadModel.Text = "Load Model";
            btnLoadModel.UseVisualStyleBackColor = true;
            btnLoadModel.Click += btnLoadModel_Click;
            // 
            // btnSaveModel
            // 
            btnSaveModel.Location = new Point(13, 199);
            btnSaveModel.Margin = new Padding(2);
            btnSaveModel.Name = "btnSaveModel";
            btnSaveModel.Size = new Size(105, 25);
            btnSaveModel.TabIndex = 11;
            btnSaveModel.Text = "Save Model";
            btnSaveModel.UseVisualStyleBackColor = true;
            btnSaveModel.Visible = false;
            btnSaveModel.Click += btnSaveModel_Click;
            // 
            // btnCreateModel
            // 
            btnCreateModel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCreateModel.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnCreateModel.Location = new Point(565, 170);
            btnCreateModel.Margin = new Padding(2);
            btnCreateModel.Name = "btnCreateModel";
            btnCreateModel.Size = new Size(105, 25);
            btnCreateModel.TabIndex = 12;
            btnCreateModel.Text = "Create Model";
            btnCreateModel.UseVisualStyleBackColor = true;
            btnCreateModel.Click += btnCreateModel_Click;
            // 
            // grpInformation
            // 
            grpInformation.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpInformation.Controls.Add(txtMessage);
            grpInformation.Location = new Point(8, 228);
            grpInformation.Margin = new Padding(2);
            grpInformation.Name = "grpInformation";
            grpInformation.Padding = new Padding(2);
            grpInformation.Size = new Size(661, 145);
            grpInformation.TabIndex = 3;
            grpInformation.TabStop = false;
            grpInformation.Text = "Message";
            // 
            // txtMessage
            // 
            txtMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtMessage.Location = new Point(11, 18);
            txtMessage.Margin = new Padding(2);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.ReadOnly = true;
            txtMessage.ScrollBars = ScrollBars.Both;
            txtMessage.Size = new Size(640, 125);
            txtMessage.TabIndex = 0;
            // 
            // grpSensor
            // 
            grpSensor.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpSensor.Controls.Add(selSensor2);
            grpSensor.Controls.Add(selSensor1and2);
            grpSensor.Controls.Add(selSensor1or2);
            grpSensor.Controls.Add(selSensor1);
            grpSensor.Location = new Point(8, 7);
            grpSensor.Margin = new Padding(2);
            grpSensor.Name = "grpSensor";
            grpSensor.Padding = new Padding(2);
            grpSensor.Size = new Size(661, 45);
            grpSensor.TabIndex = 4;
            grpSensor.TabStop = false;
            grpSensor.Text = "Sensors to create";
            // 
            // selSensor2
            // 
            selSensor2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            selSensor2.AutoSize = true;
            selSensor2.Location = new Point(192, 18);
            selSensor2.Margin = new Padding(2);
            selSensor2.Name = "selSensor2";
            selSensor2.Size = new Size(59, 19);
            selSensor2.TabIndex = 2;
            selSensor2.Text = "2 Only";
            selSensor2.UseVisualStyleBackColor = true;
            // 
            // selSensor1and2
            // 
            selSensor1and2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            selSensor1and2.AutoSize = true;
            selSensor1and2.Location = new Point(11, 18);
            selSensor1and2.Margin = new Padding(2);
            selSensor1and2.Name = "selSensor1and2";
            selSensor1and2.Size = new Size(63, 19);
            selSensor1and2.TabIndex = 0;
            selSensor1and2.Text = "1 and 2";
            selSensor1and2.UseVisualStyleBackColor = true;
            // 
            // selSensor1or2
            // 
            selSensor1or2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            selSensor1or2.AutoSize = true;
            selSensor1or2.Checked = true;
            selSensor1or2.Location = new Point(275, 18);
            selSensor1or2.Margin = new Padding(2);
            selSensor1or2.Name = "selSensor1or2";
            selSensor1or2.Size = new Size(110, 19);
            selSensor1or2.TabIndex = 3;
            selSensor1or2.TabStop = true;
            selSensor1or2.Text = "1 or 2 (Separate)";
            selSensor1or2.UseVisualStyleBackColor = true;
            // 
            // selSensor1
            // 
            selSensor1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            selSensor1.AutoSize = true;
            selSensor1.Location = new Point(106, 18);
            selSensor1.Margin = new Padding(2);
            selSensor1.Name = "selSensor1";
            selSensor1.Size = new Size(59, 19);
            selSensor1.TabIndex = 1;
            selSensor1.Text = "1 Only";
            selSensor1.UseVisualStyleBackColor = true;
            // 
            // lblModel
            // 
            lblModel.AutoSize = true;
            lblModel.Location = new Point(8, 66);
            lblModel.Margin = new Padding(2, 0, 2, 0);
            lblModel.Name = "lblModel";
            lblModel.Size = new Size(41, 15);
            lblModel.TabIndex = 13;
            lblModel.Text = "Model";
            // 
            // cmbModel
            // 
            cmbModel.FormattingEnabled = true;
            cmbModel.Location = new Point(72, 63);
            cmbModel.Margin = new Padding(2);
            cmbModel.Name = "cmbModel";
            cmbModel.Size = new Size(176, 23);
            cmbModel.TabIndex = 14;
            cmbModel.SelectedIndexChanged += cmbModel_SelectedIndexChanged;
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(8, 93);
            lblCategory.Margin = new Padding(2, 0, 2, 0);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(54, 15);
            lblCategory.TabIndex = 15;
            lblCategory.Text = "Category";
            // 
            // txtCategory
            // 
            txtCategory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCategory.Location = new Point(72, 90);
            txtCategory.Margin = new Padding(2);
            txtCategory.Name = "txtCategory";
            txtCategory.ReadOnly = true;
            txtCategory.Size = new Size(516, 23);
            txtCategory.TabIndex = 16;
            // 
            // btnSelectCategory
            // 
            btnSelectCategory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSelectCategory.Location = new Point(591, 88);
            btnSelectCategory.Margin = new Padding(2);
            btnSelectCategory.Name = "btnSelectCategory";
            btnSelectCategory.Size = new Size(78, 25);
            btnSelectCategory.TabIndex = 17;
            btnSelectCategory.Text = "Select...";
            btnSelectCategory.UseVisualStyleBackColor = true;
            btnSelectCategory.Visible = false;
            btnSelectCategory.Click += btnSelectCategory_Click;
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Location = new Point(4, 19);
            lblFrom.Margin = new Padding(2, 0, 2, 0);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(54, 15);
            lblFrom.TabIndex = 19;
            lblFrom.Text = "From (%)";
            // 
            // rangeFromPercent
            // 
            rangeFromPercent.Location = new Point(62, 17);
            rangeFromPercent.Margin = new Padding(2);
            rangeFromPercent.Name = "rangeFromPercent";
            rangeFromPercent.Size = new Size(52, 23);
            rangeFromPercent.TabIndex = 20;
            rangeFromPercent.ValueChanged += rangeFromPercent_ValueChanged;
            // 
            // rangeToPercent
            // 
            rangeToPercent.Location = new Point(162, 17);
            rangeToPercent.Margin = new Padding(2);
            rangeToPercent.Name = "rangeToPercent";
            rangeToPercent.Size = new Size(52, 23);
            rangeToPercent.TabIndex = 21;
            rangeToPercent.Value = new decimal(new int[] { 100, 0, 0, 0 });
            rangeToPercent.ValueChanged += rangeToPercent_ValueChanged;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Location = new Point(118, 19);
            lblTo.Margin = new Padding(2, 0, 2, 0);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(40, 15);
            lblTo.TabIndex = 22;
            lblTo.Text = "To (%)";
            // 
            // lblDuplicate
            // 
            lblDuplicate.AutoSize = true;
            lblDuplicate.Location = new Point(218, 20);
            lblDuplicate.Margin = new Padding(2, 0, 2, 0);
            lblDuplicate.Name = "lblDuplicate";
            lblDuplicate.Size = new Size(57, 15);
            lblDuplicate.TabIndex = 23;
            lblDuplicate.Text = "Duplicate";
            // 
            // selDuplicate
            // 
            selDuplicate.FormattingEnabled = true;
            selDuplicate.Location = new Point(279, 17);
            selDuplicate.Margin = new Padding(2);
            selDuplicate.Name = "selDuplicate";
            selDuplicate.Size = new Size(78, 23);
            selDuplicate.TabIndex = 24;
            selDuplicate.SelectedIndexChanged += selDuplicate_SelectedIndexChanged;
            // 
            // grpData
            // 
            grpData.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpData.Controls.Add(chkPresTempHumidity);
            grpData.Controls.Add(chkDataLog);
            grpData.Controls.Add(txtDataCount);
            grpData.Controls.Add(lblDataCount);
            grpData.Controls.Add(selDuplicate);
            grpData.Controls.Add(lblFrom);
            grpData.Controls.Add(lblDuplicate);
            grpData.Controls.Add(rangeFromPercent);
            grpData.Controls.Add(rangeToPercent);
            grpData.Controls.Add(lblTo);
            grpData.Location = new Point(8, 118);
            grpData.Margin = new Padding(2);
            grpData.Name = "grpData";
            grpData.Padding = new Padding(2);
            grpData.Size = new Size(661, 45);
            grpData.TabIndex = 25;
            grpData.TabStop = false;
            grpData.Text = "Data";
            // 
            // chkPresTempHumidity
            // 
            chkPresTempHumidity.AutoSize = true;
            chkPresTempHumidity.Checked = true;
            chkPresTempHumidity.CheckState = CheckState.Checked;
            chkPresTempHumidity.Location = new Point(587, 19);
            chkPresTempHumidity.Margin = new Padding(2);
            chkPresTempHumidity.Name = "chkPresTempHumidity";
            chkPresTempHumidity.Size = new Size(70, 19);
            chkPresTempHumidity.TabIndex = 30;
            chkPresTempHumidity.Text = "w./P T H";
            chkPresTempHumidity.UseVisualStyleBackColor = true;
            // 
            // chkDataLog
            // 
            chkDataLog.AutoSize = true;
            chkDataLog.Location = new Point(537, 19);
            chkDataLog.Margin = new Padding(2);
            chkDataLog.Name = "chkDataLog";
            chkDataLog.Size = new Size(46, 19);
            chkDataLog.TabIndex = 27;
            chkDataLog.Text = "Log";
            chkDataLog.UseVisualStyleBackColor = true;
            chkDataLog.CheckedChanged += chkDataLog_CheckedChanged;
            // 
            // txtDataCount
            // 
            txtDataCount.Location = new Point(413, 17);
            txtDataCount.Margin = new Padding(2);
            txtDataCount.Name = "txtDataCount";
            txtDataCount.ReadOnly = true;
            txtDataCount.Size = new Size(113, 23);
            txtDataCount.TabIndex = 26;
            // 
            // lblDataCount
            // 
            lblDataCount.AutoSize = true;
            lblDataCount.Location = new Point(361, 20);
            lblDataCount.Margin = new Padding(2, 0, 2, 0);
            lblDataCount.Name = "lblDataCount";
            lblDataCount.Size = new Size(48, 15);
            lblDataCount.TabIndex = 25;
            lblDataCount.Text = "Count : ";
            // 
            // txtResult
            // 
            txtResult.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtResult.Font = new Font("Yu Gothic UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            txtResult.Location = new Point(135, 171);
            txtResult.Margin = new Padding(2);
            txtResult.Name = "txtResult";
            txtResult.ReadOnly = true;
            txtResult.Size = new Size(426, 23);
            txtResult.TabIndex = 27;
            // 
            // cmbBinaryModel
            // 
            cmbBinaryModel.Enabled = false;
            cmbBinaryModel.FormattingEnabled = true;
            cmbBinaryModel.Location = new Point(267, 63);
            cmbBinaryModel.Margin = new Padding(2);
            cmbBinaryModel.Name = "cmbBinaryModel";
            cmbBinaryModel.Size = new Size(211, 23);
            cmbBinaryModel.TabIndex = 28;
            cmbBinaryModel.Visible = false;
            cmbBinaryModel.SelectedIndexChanged += cmbBinaryModel_SelectedIndexChanged;
            // 
            // btnClearMessage
            // 
            btnClearMessage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClearMessage.Location = new Point(565, 199);
            btnClearMessage.Margin = new Padding(2);
            btnClearMessage.Name = "btnClearMessage";
            btnClearMessage.Size = new Size(105, 25);
            btnClearMessage.TabIndex = 29;
            btnClearMessage.Text = "Clear Message";
            btnClearMessage.UseVisualStyleBackColor = true;
            btnClearMessage.Click += btnClearMessage_Click;
            // 
            // CreateModelDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(678, 380);
            Controls.Add(btnClearMessage);
            Controls.Add(cmbBinaryModel);
            Controls.Add(txtResult);
            Controls.Add(grpData);
            Controls.Add(btnSelectCategory);
            Controls.Add(txtCategory);
            Controls.Add(lblCategory);
            Controls.Add(cmbModel);
            Controls.Add(lblModel);
            Controls.Add(grpSensor);
            Controls.Add(grpInformation);
            Controls.Add(btnCreateModel);
            Controls.Add(btnSaveModel);
            Controls.Add(btnLoadModel);
            Margin = new Padding(2);
            Name = "CreateModelDialog";
            Text = "Create Model";
            grpInformation.ResumeLayout(false);
            grpInformation.PerformLayout();
            grpSensor.ResumeLayout(false);
            grpSensor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)rangeFromPercent).EndInit();
            ((System.ComponentModel.ISupportInitialize)rangeToPercent).EndInit();
            grpData.ResumeLayout(false);
            grpData.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLoadModel;
        private Button btnSaveModel;
        private Button btnCreateModel;
        private GroupBox grpInformation;
        private TextBox txtMessage;
        private GroupBox grpSensor;
        private RadioButton selSensor1and2;
        private RadioButton selSensor1or2;
        private RadioButton selSensor1;
        private RadioButton selSensor2;
        private Label lblModel;
        private ComboBox cmbModel;
        private Label lblCategory;
        private TextBox txtCategory;
        private Button btnSelectCategory;
        private Label lblFrom;
        private NumericUpDown rangeFromPercent;
        private NumericUpDown rangeToPercent;
        private Label lblTo;
        private Label lblDuplicate;
        private ComboBox selDuplicate;
        private GroupBox grpData;
        private TextBox txtDataCount;
        private Label lblDataCount;
        private TextBox txtResult;
        private CheckBox chkDataLog;
        private ComboBox cmbBinaryModel;
        private Button btnClearMessage;
        private CheckBox chkPresTempHumidity;
    }
}