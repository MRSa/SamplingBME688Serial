namespace SerialCommBME688
{
    // パッケージの追加 : System.IO.Ports
    partial class SerialCommForm
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnConnect = new Button();
            txtPort = new TextBox();
            grpPort = new GroupBox();
            btnStop = new Button();
            txtConsole = new TextBox();
            btnClear = new Button();
            grpDataCategory = new GroupBox();
            txtDataCategory = new TextBox();
            btnExport = new Button();
            grpData = new GroupBox();
            dataGridView1 = new DataGridView();
            chkExportOnlyGasRegistanceLogarithm = new CheckBox();
            btnReset = new Button();
            btnShowGraph = new Button();
            numDuplicate = new NumericUpDown();
            lblDuplicate = new Label();
            txtConsole_2 = new TextBox();
            grpPort_2 = new GroupBox();
            btnStop_2 = new Button();
            btnConnect_2 = new Button();
            txtPort_2 = new TextBox();
            grpLogConsole = new GroupBox();
            grpExportOption = new GroupBox();
            importToPercent = new NumericUpDown();
            label1 = new Label();
            importFromPercent = new NumericUpDown();
            lblFrom = new Label();
            chkCombineSensor = new CheckBox();
            grpEntryDatabase = new GroupBox();
            btnLoadData = new Button();
            btnDbStatus = new Button();
            chkDbEntrySingle = new CheckBox();
            urlDatabaseToEntry = new TextBox();
            chkEntryDatabase = new CheckBox();
            btnImport = new Button();
            btnCreateModel = new Button();
            chkAnalyze = new CheckBox();
            grpAnalysis = new GroupBox();
            chkWithPresTempHumi = new CheckBox();
            chkAnLog = new CheckBox();
            chkAnalysis = new CheckBox();
            lblResult1 = new Label();
            lblResult2 = new Label();
            fldResult2 = new TextBox();
            fldResult1 = new TextBox();
            grpPort.SuspendLayout();
            grpDataCategory.SuspendLayout();
            grpData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDuplicate).BeginInit();
            grpPort_2.SuspendLayout();
            grpLogConsole.SuspendLayout();
            grpExportOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)importToPercent).BeginInit();
            ((System.ComponentModel.ISupportInitialize)importFromPercent).BeginInit();
            grpEntryDatabase.SuspendLayout();
            grpAnalysis.SuspendLayout();
            SuspendLayout();
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(76, 20);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(75, 23);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(6, 21);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(64, 23);
            txtPort.TabIndex = 2;
            txtPort.Text = "COM5";
            txtPort.TextAlign = HorizontalAlignment.Center;
            // 
            // grpPort
            // 
            grpPort.Controls.Add(btnStop);
            grpPort.Controls.Add(btnConnect);
            grpPort.Controls.Add(txtPort);
            grpPort.Location = new Point(233, 12);
            grpPort.Name = "grpPort";
            grpPort.Size = new Size(240, 56);
            grpPort.TabIndex = 3;
            grpPort.TabStop = false;
            grpPort.Text = "Sensor1";
            // 
            // btnStop
            // 
            btnStop.Enabled = false;
            btnStop.Location = new Point(157, 20);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 3;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // txtConsole
            // 
            txtConsole.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtConsole.HideSelection = false;
            txtConsole.Location = new Point(4, 20);
            txtConsole.Multiline = true;
            txtConsole.Name = "txtConsole";
            txtConsole.ReadOnly = true;
            txtConsole.ScrollBars = ScrollBars.Vertical;
            txtConsole.Size = new Size(800, 46);
            txtConsole.TabIndex = 4;
            // 
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.Location = new Point(741, 33);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(79, 23);
            btnClear.TabIndex = 5;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // grpDataCategory
            // 
            grpDataCategory.Controls.Add(txtDataCategory);
            grpDataCategory.Location = new Point(12, 12);
            grpDataCategory.Name = "grpDataCategory";
            grpDataCategory.Size = new Size(215, 56);
            grpDataCategory.TabIndex = 6;
            grpDataCategory.TabStop = false;
            grpDataCategory.Text = "Data Category";
            // 
            // txtDataCategory
            // 
            txtDataCategory.Location = new Point(6, 20);
            txtDataCategory.Name = "txtDataCategory";
            txtDataCategory.Size = new Size(203, 23);
            txtDataCategory.TabIndex = 0;
            // 
            // btnExport
            // 
            btnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExport.Location = new Point(703, 296);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(120, 22);
            btnExport.TabIndex = 7;
            btnExport.Text = "Export CSV";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // grpData
            // 
            grpData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpData.Controls.Add(dataGridView1);
            grpData.Location = new Point(11, 323);
            grpData.Name = "grpData";
            grpData.Size = new Size(682, 228);
            grpData.TabIndex = 8;
            grpData.TabStop = false;
            grpData.Text = "Collected Data";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 19);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(676, 206);
            dataGridView1.TabIndex = 0;
            // 
            // chkExportOnlyGasRegistanceLogarithm
            // 
            chkExportOnlyGasRegistanceLogarithm.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            chkExportOnlyGasRegistanceLogarithm.AutoSize = true;
            chkExportOnlyGasRegistanceLogarithm.Location = new Point(9, 18);
            chkExportOnlyGasRegistanceLogarithm.Name = "chkExportOnlyGasRegistanceLogarithm";
            chkExportOnlyGasRegistanceLogarithm.Size = new Size(114, 19);
            chkExportOnlyGasRegistanceLogarithm.TabIndex = 9;
            chkExportOnlyGasRegistanceLogarithm.Text = "Only Gas R.(Log)";
            chkExportOnlyGasRegistanceLogarithm.UseVisualStyleBackColor = true;
            chkExportOnlyGasRegistanceLogarithm.CheckedChanged += chkExportOnlyGasRegistanceLogarithm_CheckedChanged;
            // 
            // btnReset
            // 
            btnReset.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnReset.Location = new Point(700, 529);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(123, 22);
            btnReset.TabIndex = 10;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnShowGraph
            // 
            btnShowGraph.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnShowGraph.Location = new Point(699, 472);
            btnShowGraph.Name = "btnShowGraph";
            btnShowGraph.Size = new Size(123, 23);
            btnShowGraph.TabIndex = 11;
            btnShowGraph.Text = "Show Graph";
            btnShowGraph.UseVisualStyleBackColor = true;
            btnShowGraph.Click += btnShowGraph_Click;
            // 
            // numDuplicate
            // 
            numDuplicate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            numDuplicate.Location = new Point(66, 114);
            numDuplicate.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numDuplicate.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDuplicate.Name = "numDuplicate";
            numDuplicate.Size = new Size(56, 23);
            numDuplicate.TabIndex = 12;
            numDuplicate.TextAlign = HorizontalAlignment.Center;
            numDuplicate.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblDuplicate
            // 
            lblDuplicate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblDuplicate.AutoSize = true;
            lblDuplicate.Location = new Point(6, 116);
            lblDuplicate.Name = "lblDuplicate";
            lblDuplicate.Size = new Size(57, 15);
            lblDuplicate.TabIndex = 13;
            lblDuplicate.Text = "Duplicate";
            lblDuplicate.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtConsole_2
            // 
            txtConsole_2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtConsole_2.HideSelection = false;
            txtConsole_2.Location = new Point(4, 71);
            txtConsole_2.Multiline = true;
            txtConsole_2.Name = "txtConsole_2";
            txtConsole_2.ReadOnly = true;
            txtConsole_2.ScrollBars = ScrollBars.Vertical;
            txtConsole_2.Size = new Size(800, 47);
            txtConsole_2.TabIndex = 14;
            // 
            // grpPort_2
            // 
            grpPort_2.Controls.Add(btnStop_2);
            grpPort_2.Controls.Add(btnConnect_2);
            grpPort_2.Controls.Add(txtPort_2);
            grpPort_2.Location = new Point(479, 12);
            grpPort_2.Name = "grpPort_2";
            grpPort_2.Size = new Size(240, 56);
            grpPort_2.TabIndex = 15;
            grpPort_2.TabStop = false;
            grpPort_2.Text = "Sensor2";
            // 
            // btnStop_2
            // 
            btnStop_2.Enabled = false;
            btnStop_2.Location = new Point(157, 20);
            btnStop_2.Name = "btnStop_2";
            btnStop_2.Size = new Size(75, 23);
            btnStop_2.TabIndex = 3;
            btnStop_2.Text = "Stop";
            btnStop_2.UseVisualStyleBackColor = true;
            btnStop_2.Click += btnStop_2_Click;
            // 
            // btnConnect_2
            // 
            btnConnect_2.Location = new Point(76, 20);
            btnConnect_2.Name = "btnConnect_2";
            btnConnect_2.Size = new Size(75, 23);
            btnConnect_2.TabIndex = 0;
            btnConnect_2.Text = "Connect";
            btnConnect_2.UseVisualStyleBackColor = true;
            btnConnect_2.Click += btnConnect_2_Click;
            // 
            // txtPort_2
            // 
            txtPort_2.Location = new Point(6, 21);
            txtPort_2.Name = "txtPort_2";
            txtPort_2.Size = new Size(64, 23);
            txtPort_2.TabIndex = 2;
            txtPort_2.Text = "COM6";
            txtPort_2.TextAlign = HorizontalAlignment.Center;
            // 
            // grpLogConsole
            // 
            grpLogConsole.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpLogConsole.Controls.Add(txtConsole_2);
            grpLogConsole.Controls.Add(txtConsole);
            grpLogConsole.Location = new Point(15, 138);
            grpLogConsole.Name = "grpLogConsole";
            grpLogConsole.Size = new Size(811, 125);
            grpLogConsole.TabIndex = 16;
            grpLogConsole.TabStop = false;
            grpLogConsole.Text = "Sampling Status";
            // 
            // grpExportOption
            // 
            grpExportOption.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            grpExportOption.Controls.Add(importToPercent);
            grpExportOption.Controls.Add(label1);
            grpExportOption.Controls.Add(importFromPercent);
            grpExportOption.Controls.Add(lblFrom);
            grpExportOption.Controls.Add(chkCombineSensor);
            grpExportOption.Controls.Add(numDuplicate);
            grpExportOption.Controls.Add(lblDuplicate);
            grpExportOption.Controls.Add(chkExportOnlyGasRegistanceLogarithm);
            grpExportOption.Location = new Point(697, 323);
            grpExportOption.Name = "grpExportOption";
            grpExportOption.Size = new Size(129, 143);
            grpExportOption.TabIndex = 17;
            grpExportOption.TabStop = false;
            grpExportOption.Text = "Export Option";
            // 
            // importToPercent
            // 
            importToPercent.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            importToPercent.Location = new Point(66, 91);
            importToPercent.Name = "importToPercent";
            importToPercent.Size = new Size(56, 23);
            importToPercent.TabIndex = 24;
            importToPercent.TextAlign = HorizontalAlignment.Center;
            importToPercent.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(6, 93);
            label1.Name = "label1";
            label1.Size = new Size(40, 15);
            label1.TabIndex = 23;
            label1.Text = "To (%)";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // importFromPercent
            // 
            importFromPercent.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            importFromPercent.Location = new Point(66, 68);
            importFromPercent.Name = "importFromPercent";
            importFromPercent.Size = new Size(56, 23);
            importFromPercent.TabIndex = 22;
            importFromPercent.TextAlign = HorizontalAlignment.Center;
            // 
            // lblFrom
            // 
            lblFrom.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblFrom.AutoSize = true;
            lblFrom.Location = new Point(6, 70);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(54, 15);
            lblFrom.TabIndex = 21;
            lblFrom.Text = "From (%)";
            lblFrom.TextAlign = ContentAlignment.MiddleRight;
            // 
            // chkCombineSensor
            // 
            chkCombineSensor.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            chkCombineSensor.AutoSize = true;
            chkCombineSensor.Location = new Point(10, 41);
            chkCombineSensor.Name = "chkCombineSensor";
            chkCombineSensor.Size = new Size(110, 19);
            chkCombineSensor.TabIndex = 14;
            chkCombineSensor.Text = "Combine sensor";
            chkCombineSensor.UseVisualStyleBackColor = true;
            // 
            // grpEntryDatabase
            // 
            grpEntryDatabase.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpEntryDatabase.Controls.Add(btnLoadData);
            grpEntryDatabase.Controls.Add(btnDbStatus);
            grpEntryDatabase.Controls.Add(chkDbEntrySingle);
            grpEntryDatabase.Controls.Add(urlDatabaseToEntry);
            grpEntryDatabase.Controls.Add(chkEntryDatabase);
            grpEntryDatabase.Location = new Point(12, 74);
            grpEntryDatabase.Name = "grpEntryDatabase";
            grpEntryDatabase.Size = new Size(811, 58);
            grpEntryDatabase.TabIndex = 18;
            grpEntryDatabase.TabStop = false;
            grpEntryDatabase.Text = "Database";
            // 
            // btnLoadData
            // 
            btnLoadData.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoadData.Enabled = false;
            btnLoadData.Location = new Point(650, 22);
            btnLoadData.Name = "btnLoadData";
            btnLoadData.Size = new Size(71, 23);
            btnLoadData.TabIndex = 22;
            btnLoadData.Text = "Load data";
            btnLoadData.UseVisualStyleBackColor = true;
            btnLoadData.Click += btnLoadData_Click;
            // 
            // btnDbStatus
            // 
            btnDbStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDbStatus.Location = new Point(729, 22);
            btnDbStatus.Name = "btnDbStatus";
            btnDbStatus.Size = new Size(76, 23);
            btnDbStatus.TabIndex = 6;
            btnDbStatus.Text = "Status";
            btnDbStatus.UseVisualStyleBackColor = true;
            btnDbStatus.Click += btnDbStatus_Click;
            // 
            // chkDbEntrySingle
            // 
            chkDbEntrySingle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkDbEntrySingle.AutoSize = true;
            chkDbEntrySingle.Checked = true;
            chkDbEntrySingle.CheckState = CheckState.Checked;
            chkDbEntrySingle.Location = new Point(548, 24);
            chkDbEntrySingle.Name = "chkDbEntrySingle";
            chkDbEntrySingle.Size = new Size(97, 19);
            chkDbEntrySingle.TabIndex = 2;
            chkDbEntrySingle.Text = "Entry All Data";
            chkDbEntrySingle.UseVisualStyleBackColor = true;
            // 
            // urlDatabaseToEntry
            // 
            urlDatabaseToEntry.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            urlDatabaseToEntry.Location = new Point(77, 23);
            urlDatabaseToEntry.Name = "urlDatabaseToEntry";
            urlDatabaseToEntry.Size = new Size(460, 23);
            urlDatabaseToEntry.TabIndex = 1;
            urlDatabaseToEntry.Text = "http://localhost:3010/";
            // 
            // chkEntryDatabase
            // 
            chkEntryDatabase.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            chkEntryDatabase.AutoSize = true;
            chkEntryDatabase.Location = new Point(6, 25);
            chkEntryDatabase.Name = "chkEntryDatabase";
            chkEntryDatabase.Size = new Size(68, 19);
            chkEntryDatabase.TabIndex = 1;
            chkEntryDatabase.Text = "     URL :";
            chkEntryDatabase.TextAlign = ContentAlignment.MiddleRight;
            chkEntryDatabase.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            btnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnImport.Location = new Point(703, 269);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(120, 22);
            btnImport.TabIndex = 19;
            btnImport.Text = "Import CSV";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // btnCreateModel
            // 
            btnCreateModel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCreateModel.Location = new Point(700, 501);
            btnCreateModel.Name = "btnCreateModel";
            btnCreateModel.Size = new Size(123, 22);
            btnCreateModel.TabIndex = 20;
            btnCreateModel.Text = "Create Model";
            btnCreateModel.UseVisualStyleBackColor = true;
            btnCreateModel.Click += btnCreateModel_Click;
            // 
            // chkAnalyze
            // 
            chkAnalyze.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkAnalyze.AutoSize = true;
            chkAnalyze.Location = new Point(733, 8);
            chkAnalyze.Name = "chkAnalyze";
            chkAnalyze.Size = new Size(93, 19);
            chkAnalyze.TabIndex = 15;
            chkAnalyze.Text = "Analyze data";
            chkAnalyze.UseVisualStyleBackColor = true;
            chkAnalyze.Visible = false;
            // 
            // grpAnalysis
            // 
            grpAnalysis.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpAnalysis.Controls.Add(chkWithPresTempHumi);
            grpAnalysis.Controls.Add(chkAnLog);
            grpAnalysis.Controls.Add(chkAnalysis);
            grpAnalysis.Controls.Add(lblResult1);
            grpAnalysis.Controls.Add(lblResult2);
            grpAnalysis.Controls.Add(fldResult2);
            grpAnalysis.Controls.Add(fldResult1);
            grpAnalysis.Enabled = false;
            grpAnalysis.Location = new Point(12, 269);
            grpAnalysis.Margin = new Padding(2);
            grpAnalysis.Name = "grpAnalysis";
            grpAnalysis.Padding = new Padding(2);
            grpAnalysis.Size = new Size(681, 49);
            grpAnalysis.TabIndex = 21;
            grpAnalysis.TabStop = false;
            grpAnalysis.Text = "Prediction";
            // 
            // chkWithPresTempHumi
            // 
            chkWithPresTempHumi.AutoSize = true;
            chkWithPresTempHumi.Location = new Point(598, 22);
            chkWithPresTempHumi.Margin = new Padding(2);
            chkWithPresTempHumi.Name = "chkWithPresTempHumi";
            chkWithPresTempHumi.Size = new Size(70, 19);
            chkWithPresTempHumi.TabIndex = 22;
            chkWithPresTempHumi.Text = "w./P T H";
            chkWithPresTempHumi.UseVisualStyleBackColor = true;
            // 
            // chkAnLog
            // 
            chkAnLog.AutoSize = true;
            chkAnLog.Location = new Point(548, 22);
            chkAnLog.Margin = new Padding(2);
            chkAnLog.Name = "chkAnLog";
            chkAnLog.Size = new Size(46, 19);
            chkAnLog.TabIndex = 5;
            chkAnLog.Text = "Log";
            chkAnLog.UseVisualStyleBackColor = true;
            // 
            // chkAnalysis
            // 
            chkAnalysis.AutoSize = true;
            chkAnalysis.Location = new Point(8, 22);
            chkAnalysis.Margin = new Padding(2);
            chkAnalysis.Name = "chkAnalysis";
            chkAnalysis.Size = new Size(67, 19);
            chkAnalysis.TabIndex = 4;
            chkAnalysis.Text = "Analyze";
            chkAnalysis.UseVisualStyleBackColor = true;
            chkAnalysis.CheckedChanged += chkAnalysis_CheckedChanged;
            // 
            // lblResult1
            // 
            lblResult1.AutoSize = true;
            lblResult1.Location = new Point(79, 24);
            lblResult1.Margin = new Padding(2, 0, 2, 0);
            lblResult1.Name = "lblResult1";
            lblResult1.Size = new Size(19, 15);
            lblResult1.TabIndex = 3;
            lblResult1.Text = "1: ";
            lblResult1.Visible = false;
            // 
            // lblResult2
            // 
            lblResult2.AutoSize = true;
            lblResult2.Location = new Point(314, 24);
            lblResult2.Margin = new Padding(2, 0, 2, 0);
            lblResult2.Name = "lblResult2";
            lblResult2.Size = new Size(19, 15);
            lblResult2.TabIndex = 2;
            lblResult2.Text = "2: ";
            lblResult2.Visible = false;
            // 
            // fldResult2
            // 
            fldResult2.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            fldResult2.Location = new Point(337, 15);
            fldResult2.Margin = new Padding(2);
            fldResult2.MaxLength = 128;
            fldResult2.Name = "fldResult2";
            fldResult2.ReadOnly = true;
            fldResult2.Size = new Size(200, 29);
            fldResult2.TabIndex = 1;
            // 
            // fldResult1
            // 
            fldResult1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            fldResult1.Location = new Point(102, 15);
            fldResult1.Margin = new Padding(2);
            fldResult1.MaxLength = 128;
            fldResult1.Name = "fldResult1";
            fldResult1.ReadOnly = true;
            fldResult1.Size = new Size(200, 29);
            fldResult1.TabIndex = 0;
            // 
            // SerialCommForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(835, 566);
            Controls.Add(chkAnalyze);
            Controls.Add(grpAnalysis);
            Controls.Add(btnCreateModel);
            Controls.Add(btnImport);
            Controls.Add(grpEntryDatabase);
            Controls.Add(grpExportOption);
            Controls.Add(grpPort_2);
            Controls.Add(btnShowGraph);
            Controls.Add(btnReset);
            Controls.Add(grpData);
            Controls.Add(btnExport);
            Controls.Add(grpDataCategory);
            Controls.Add(btnClear);
            Controls.Add(grpPort);
            Controls.Add(grpLogConsole);
            Name = "SerialCommForm";
            Text = "BME688 Sampling";
            FormClosing += SerialCommForm_FormClosing;
            Load += SerialCommForm_Load;
            grpPort.ResumeLayout(false);
            grpPort.PerformLayout();
            grpDataCategory.ResumeLayout(false);
            grpDataCategory.PerformLayout();
            grpData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDuplicate).EndInit();
            grpPort_2.ResumeLayout(false);
            grpPort_2.PerformLayout();
            grpLogConsole.ResumeLayout(false);
            grpLogConsole.PerformLayout();
            grpExportOption.ResumeLayout(false);
            grpExportOption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)importToPercent).EndInit();
            ((System.ComponentModel.ISupportInitialize)importFromPercent).EndInit();
            grpEntryDatabase.ResumeLayout(false);
            grpEntryDatabase.PerformLayout();
            grpAnalysis.ResumeLayout(false);
            grpAnalysis.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnConnect;
        private TextBox txtPort;
        private GroupBox grpPort;
        private Button btnStop;
        private TextBox txtConsole;
        private Button btnClear;
        private GroupBox grpDataCategory;
        private TextBox txtDataCategory;
        private Button btnExport;
        private GroupBox grpData;
        private DataGridView dataGridView1;
        private CheckBox chkExportOnlyGasRegistanceLogarithm;
        private Button btnReset;
        private Button btnShowGraph;
        private NumericUpDown numDuplicate;
        private Label lblDuplicate;
        private TextBox txtConsole_2;
        private GroupBox grpPort_2;
        private Button btnStop_2;
        private Button btnConnect_2;
        private TextBox txtPort_2;
        private GroupBox grpLogConsole;
        private GroupBox grpExportOption;
        private CheckBox chkCombineSensor;
        private GroupBox grpEntryDatabase;
        private TextBox urlDatabaseToEntry;
        private CheckBox chkEntryDatabase;
        private CheckBox chkDbEntrySingle;
        private Button btnDbStatus;
        private Button btnImport;
        private Button btnCreateModel;
        private CheckBox chkAnalyze;
        private NumericUpDown importToPercent;
        private Label label1;
        private NumericUpDown importFromPercent;
        private Label lblFrom;
        private GroupBox grpAnalysis;
        private Label lblResult1;
        private Label lblResult2;
        private TextBox fldResult2;
        private TextBox fldResult1;
        private CheckBox chkAnalysis;
        private CheckBox chkAnLog;
        private Button btnLoadData;
        private CheckBox chkWithPresTempHumi;
    }
}
