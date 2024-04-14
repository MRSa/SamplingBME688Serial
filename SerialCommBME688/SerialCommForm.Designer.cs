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
            chkCombineSensor = new CheckBox();
            grpEntryDatabase = new GroupBox();
            btnDbStatus = new Button();
            chkDbEntrySingle = new CheckBox();
            urlDatabaseToEntry = new TextBox();
            chkEntryDatabase = new CheckBox();
            grpPort.SuspendLayout();
            grpDataCategory.SuspendLayout();
            grpData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDuplicate).BeginInit();
            grpPort_2.SuspendLayout();
            grpLogConsole.SuspendLayout();
            grpExportOption.SuspendLayout();
            grpEntryDatabase.SuspendLayout();
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
            txtPort.Text = "COM7";
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
            txtConsole.Location = new Point(3, 24);
            txtConsole.Multiline = true;
            txtConsole.Name = "txtConsole";
            txtConsole.ReadOnly = true;
            txtConsole.ScrollBars = ScrollBars.Vertical;
            txtConsole.Size = new Size(803, 87);
            txtConsole.TabIndex = 4;
            // 
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.Location = new Point(747, 33);
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
            btnExport.Location = new Point(703, 438);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(123, 22);
            btnExport.TabIndex = 7;
            btnExport.Text = "Export CSV";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // grpData
            // 
            grpData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpData.Controls.Add(dataGridView1);
            grpData.Location = new Point(12, 375);
            grpData.Name = "grpData";
            grpData.Size = new Size(685, 313);
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
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(679, 291);
            dataGridView1.TabIndex = 0;
            // 
            // chkExportOnlyGasRegistanceLogarithm
            // 
            chkExportOnlyGasRegistanceLogarithm.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            chkExportOnlyGasRegistanceLogarithm.AutoSize = true;
            chkExportOnlyGasRegistanceLogarithm.Checked = true;
            chkExportOnlyGasRegistanceLogarithm.CheckState = CheckState.Checked;
            chkExportOnlyGasRegistanceLogarithm.Location = new Point(6, 22);
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
            btnReset.Location = new Point(703, 663);
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
            btnShowGraph.Location = new Point(703, 581);
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
            numDuplicate.Location = new Point(67, 78);
            numDuplicate.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numDuplicate.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDuplicate.Name = "numDuplicate";
            numDuplicate.Size = new Size(56, 23);
            numDuplicate.TabIndex = 12;
            numDuplicate.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblDuplicate
            // 
            lblDuplicate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblDuplicate.AutoSize = true;
            lblDuplicate.Location = new Point(4, 80);
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
            txtConsole_2.Location = new Point(3, 117);
            txtConsole_2.Multiline = true;
            txtConsole_2.Name = "txtConsole_2";
            txtConsole_2.ReadOnly = true;
            txtConsole_2.ScrollBars = ScrollBars.Vertical;
            txtConsole_2.Size = new Size(803, 108);
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
            grpLogConsole.Location = new Point(12, 138);
            grpLogConsole.Name = "grpLogConsole";
            grpLogConsole.Size = new Size(814, 231);
            grpLogConsole.TabIndex = 16;
            grpLogConsole.TabStop = false;
            grpLogConsole.Text = "Sampling Status";
            // 
            // grpExportOption
            // 
            grpExportOption.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            grpExportOption.Controls.Add(chkCombineSensor);
            grpExportOption.Controls.Add(numDuplicate);
            grpExportOption.Controls.Add(lblDuplicate);
            grpExportOption.Controls.Add(chkExportOnlyGasRegistanceLogarithm);
            grpExportOption.Location = new Point(703, 468);
            grpExportOption.Name = "grpExportOption";
            grpExportOption.Size = new Size(129, 107);
            grpExportOption.TabIndex = 17;
            grpExportOption.TabStop = false;
            grpExportOption.Text = "Export Option";
            // 
            // chkCombineSensor
            // 
            chkCombineSensor.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            chkCombineSensor.AutoSize = true;
            chkCombineSensor.Location = new Point(6, 47);
            chkCombineSensor.Name = "chkCombineSensor";
            chkCombineSensor.Size = new Size(110, 19);
            chkCombineSensor.TabIndex = 14;
            chkCombineSensor.Text = "Combine sensor";
            chkCombineSensor.UseVisualStyleBackColor = true;
            // 
            // grpEntryDatabase
            // 
            grpEntryDatabase.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpEntryDatabase.Controls.Add(btnDbStatus);
            grpEntryDatabase.Controls.Add(chkDbEntrySingle);
            grpEntryDatabase.Controls.Add(urlDatabaseToEntry);
            grpEntryDatabase.Controls.Add(chkEntryDatabase);
            grpEntryDatabase.Location = new Point(12, 74);
            grpEntryDatabase.Name = "grpEntryDatabase";
            grpEntryDatabase.Size = new Size(814, 58);
            grpEntryDatabase.TabIndex = 18;
            grpEntryDatabase.TabStop = false;
            grpEntryDatabase.Text = "Entry Database";
            // 
            // btnDbStatus
            // 
            btnDbStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDbStatus.Location = new Point(735, 22);
            btnDbStatus.Name = "btnDbStatus";
            btnDbStatus.Size = new Size(71, 23);
            btnDbStatus.TabIndex = 6;
            btnDbStatus.Text = "Status";
            btnDbStatus.UseVisualStyleBackColor = true;
            btnDbStatus.Click += btnDbStatus_Click;
            // 
            // chkDbEntrySingle
            // 
            chkDbEntrySingle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkDbEntrySingle.AutoSize = true;
            chkDbEntrySingle.Location = new Point(632, 25);
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
            urlDatabaseToEntry.Size = new Size(541, 23);
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
            // SerialCommForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(838, 700);
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
            grpEntryDatabase.ResumeLayout(false);
            grpEntryDatabase.PerformLayout();
            ResumeLayout(false);
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
    }
}
