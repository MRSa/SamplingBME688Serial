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
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.grpPort = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.grpDataCategory = new System.Windows.Forms.GroupBox();
            this.txtDataCategory = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.grpData = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chkExportOnlyGasRegistanceLogarithm = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnShowGraph = new System.Windows.Forms.Button();
            this.numDuplicate = new System.Windows.Forms.NumericUpDown();
            this.lblDuplicate = new System.Windows.Forms.Label();
            this.txtConsole_2 = new System.Windows.Forms.TextBox();
            this.grpPort_2 = new System.Windows.Forms.GroupBox();
            this.btnStop_2 = new System.Windows.Forms.Button();
            this.btnConnect_2 = new System.Windows.Forms.Button();
            this.txtPort_2 = new System.Windows.Forms.TextBox();
            this.grpLogConsole = new System.Windows.Forms.GroupBox();
            this.grpExportOption = new System.Windows.Forms.GroupBox();
            this.chkCombineSensor = new System.Windows.Forms.CheckBox();
            this.grpPort.SuspendLayout();
            this.grpDataCategory.SuspendLayout();
            this.grpData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDuplicate)).BeginInit();
            this.grpPort_2.SuspendLayout();
            this.grpLogConsole.SuspendLayout();
            this.grpExportOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(76, 20);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(6, 21);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(64, 23);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "COM7";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // grpPort
            // 
            this.grpPort.Controls.Add(this.btnStop);
            this.grpPort.Controls.Add(this.btnConnect);
            this.grpPort.Controls.Add(this.txtPort);
            this.grpPort.Location = new System.Drawing.Point(233, 12);
            this.grpPort.Name = "grpPort";
            this.grpPort.Size = new System.Drawing.Size(240, 56);
            this.grpPort.TabIndex = 3;
            this.grpPort.TabStop = false;
            this.grpPort.Text = "Sensor1";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(157, 20);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtConsole
            // 
            this.txtConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConsole.HideSelection = false;
            this.txtConsole.Location = new System.Drawing.Point(3, 24);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConsole.Size = new System.Drawing.Size(803, 87);
            this.txtConsole.TabIndex = 4;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(747, 33);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(79, 23);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // grpDataCategory
            // 
            this.grpDataCategory.Controls.Add(this.txtDataCategory);
            this.grpDataCategory.Location = new System.Drawing.Point(12, 12);
            this.grpDataCategory.Name = "grpDataCategory";
            this.grpDataCategory.Size = new System.Drawing.Size(215, 56);
            this.grpDataCategory.TabIndex = 6;
            this.grpDataCategory.TabStop = false;
            this.grpDataCategory.Text = "Data Category";
            // 
            // txtDataCategory
            // 
            this.txtDataCategory.Location = new System.Drawing.Point(6, 20);
            this.txtDataCategory.Name = "txtDataCategory";
            this.txtDataCategory.Size = new System.Drawing.Size(203, 23);
            this.txtDataCategory.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(703, 330);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(123, 22);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // grpData
            // 
            this.grpData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpData.Controls.Add(this.dataGridView1);
            this.grpData.Location = new System.Drawing.Point(12, 311);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(685, 269);
            this.grpData.TabIndex = 8;
            this.grpData.TabStop = false;
            this.grpData.Text = "Collected Data";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(679, 247);
            this.dataGridView1.TabIndex = 0;
            // 
            // chkExportOnlyGasRegistanceLogarithm
            // 
            this.chkExportOnlyGasRegistanceLogarithm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkExportOnlyGasRegistanceLogarithm.AutoSize = true;
            this.chkExportOnlyGasRegistanceLogarithm.Checked = true;
            this.chkExportOnlyGasRegistanceLogarithm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportOnlyGasRegistanceLogarithm.Location = new System.Drawing.Point(6, 22);
            this.chkExportOnlyGasRegistanceLogarithm.Name = "chkExportOnlyGasRegistanceLogarithm";
            this.chkExportOnlyGasRegistanceLogarithm.Size = new System.Drawing.Size(114, 19);
            this.chkExportOnlyGasRegistanceLogarithm.TabIndex = 9;
            this.chkExportOnlyGasRegistanceLogarithm.Text = "Only Gas R.(Log)";
            this.chkExportOnlyGasRegistanceLogarithm.UseVisualStyleBackColor = true;
            this.chkExportOnlyGasRegistanceLogarithm.CheckedChanged += new System.EventHandler(this.chkExportOnlyGasRegistanceLogarithm_CheckedChanged);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(703, 555);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(123, 22);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnShowGraph
            // 
            this.btnShowGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowGraph.Location = new System.Drawing.Point(703, 473);
            this.btnShowGraph.Name = "btnShowGraph";
            this.btnShowGraph.Size = new System.Drawing.Size(123, 23);
            this.btnShowGraph.TabIndex = 11;
            this.btnShowGraph.Text = "Show Graph";
            this.btnShowGraph.UseVisualStyleBackColor = true;
            this.btnShowGraph.Visible = false;
            this.btnShowGraph.Click += new System.EventHandler(this.btnShowGraph_Click);
            // 
            // numDuplicate
            // 
            this.numDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numDuplicate.Location = new System.Drawing.Point(67, 78);
            this.numDuplicate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numDuplicate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDuplicate.Name = "numDuplicate";
            this.numDuplicate.Size = new System.Drawing.Size(56, 23);
            this.numDuplicate.TabIndex = 12;
            this.numDuplicate.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblDuplicate
            // 
            this.lblDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDuplicate.AutoSize = true;
            this.lblDuplicate.Location = new System.Drawing.Point(4, 80);
            this.lblDuplicate.Name = "lblDuplicate";
            this.lblDuplicate.Size = new System.Drawing.Size(57, 15);
            this.lblDuplicate.TabIndex = 13;
            this.lblDuplicate.Text = "Duplicate";
            this.lblDuplicate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtConsole_2
            // 
            this.txtConsole_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConsole_2.HideSelection = false;
            this.txtConsole_2.Location = new System.Drawing.Point(3, 117);
            this.txtConsole_2.Multiline = true;
            this.txtConsole_2.Name = "txtConsole_2";
            this.txtConsole_2.ReadOnly = true;
            this.txtConsole_2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConsole_2.Size = new System.Drawing.Size(803, 108);
            this.txtConsole_2.TabIndex = 14;
            // 
            // grpPort_2
            // 
            this.grpPort_2.Controls.Add(this.btnStop_2);
            this.grpPort_2.Controls.Add(this.btnConnect_2);
            this.grpPort_2.Controls.Add(this.txtPort_2);
            this.grpPort_2.Location = new System.Drawing.Point(479, 12);
            this.grpPort_2.Name = "grpPort_2";
            this.grpPort_2.Size = new System.Drawing.Size(240, 56);
            this.grpPort_2.TabIndex = 15;
            this.grpPort_2.TabStop = false;
            this.grpPort_2.Text = "Sensor2";
            // 
            // btnStop_2
            // 
            this.btnStop_2.Enabled = false;
            this.btnStop_2.Location = new System.Drawing.Point(157, 20);
            this.btnStop_2.Name = "btnStop_2";
            this.btnStop_2.Size = new System.Drawing.Size(75, 23);
            this.btnStop_2.TabIndex = 3;
            this.btnStop_2.Text = "Stop";
            this.btnStop_2.UseVisualStyleBackColor = true;
            this.btnStop_2.Click += new System.EventHandler(this.btnStop_2_Click);
            // 
            // btnConnect_2
            // 
            this.btnConnect_2.Location = new System.Drawing.Point(76, 20);
            this.btnConnect_2.Name = "btnConnect_2";
            this.btnConnect_2.Size = new System.Drawing.Size(75, 23);
            this.btnConnect_2.TabIndex = 0;
            this.btnConnect_2.Text = "Connect";
            this.btnConnect_2.UseVisualStyleBackColor = true;
            this.btnConnect_2.Click += new System.EventHandler(this.btnConnect_2_Click);
            // 
            // txtPort_2
            // 
            this.txtPort_2.Location = new System.Drawing.Point(6, 21);
            this.txtPort_2.Name = "txtPort_2";
            this.txtPort_2.Size = new System.Drawing.Size(64, 23);
            this.txtPort_2.TabIndex = 2;
            this.txtPort_2.Text = "COM6";
            this.txtPort_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // grpLogConsole
            // 
            this.grpLogConsole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLogConsole.Controls.Add(this.txtConsole_2);
            this.grpLogConsole.Controls.Add(this.txtConsole);
            this.grpLogConsole.Location = new System.Drawing.Point(12, 74);
            this.grpLogConsole.Name = "grpLogConsole";
            this.grpLogConsole.Size = new System.Drawing.Size(814, 231);
            this.grpLogConsole.TabIndex = 16;
            this.grpLogConsole.TabStop = false;
            this.grpLogConsole.Text = "Sampling Status";
            // 
            // grpExportOption
            // 
            this.grpExportOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpExportOption.Controls.Add(this.chkCombineSensor);
            this.grpExportOption.Controls.Add(this.numDuplicate);
            this.grpExportOption.Controls.Add(this.lblDuplicate);
            this.grpExportOption.Controls.Add(this.chkExportOnlyGasRegistanceLogarithm);
            this.grpExportOption.Location = new System.Drawing.Point(703, 360);
            this.grpExportOption.Name = "grpExportOption";
            this.grpExportOption.Size = new System.Drawing.Size(129, 107);
            this.grpExportOption.TabIndex = 17;
            this.grpExportOption.TabStop = false;
            this.grpExportOption.Text = "Export Option";
            // 
            // chkCombineSensor
            // 
            this.chkCombineSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCombineSensor.AutoSize = true;
            this.chkCombineSensor.Location = new System.Drawing.Point(6, 47);
            this.chkCombineSensor.Name = "chkCombineSensor";
            this.chkCombineSensor.Size = new System.Drawing.Size(110, 19);
            this.chkCombineSensor.TabIndex = 14;
            this.chkCombineSensor.Text = "Combine sensor";
            this.chkCombineSensor.UseVisualStyleBackColor = true;
            // 
            // SerialCommForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 592);
            this.Controls.Add(this.grpExportOption);
            this.Controls.Add(this.grpPort_2);
            this.Controls.Add(this.btnShowGraph);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.grpData);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.grpDataCategory);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.grpPort);
            this.Controls.Add(this.grpLogConsole);
            this.Name = "SerialCommForm";
            this.Text = "BME688 Sampling";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SerialCommForm_FormClosing);
            this.Load += new System.EventHandler(this.SerialCommForm_Load);
            this.grpPort.ResumeLayout(false);
            this.grpPort.PerformLayout();
            this.grpDataCategory.ResumeLayout(false);
            this.grpDataCategory.PerformLayout();
            this.grpData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDuplicate)).EndInit();
            this.grpPort_2.ResumeLayout(false);
            this.grpPort_2.PerformLayout();
            this.grpLogConsole.ResumeLayout(false);
            this.grpLogConsole.PerformLayout();
            this.grpExportOption.ResumeLayout(false);
            this.grpExportOption.PerformLayout();
            this.ResumeLayout(false);

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
    }
}
