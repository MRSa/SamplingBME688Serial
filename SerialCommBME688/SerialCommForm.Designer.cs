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
            this.grpPort.SuspendLayout();
            this.grpDataCategory.SuspendLayout();
            this.grpData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDuplicate)).BeginInit();
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
            this.grpPort.Location = new System.Drawing.Point(12, 12);
            this.grpPort.Name = "grpPort";
            this.grpPort.Size = new System.Drawing.Size(240, 56);
            this.grpPort.TabIndex = 3;
            this.grpPort.TabStop = false;
            this.grpPort.Text = "Communication Port";
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
            this.txtConsole.Location = new System.Drawing.Point(18, 76);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConsole.Size = new System.Drawing.Size(658, 79);
            this.txtConsole.TabIndex = 4;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(597, 33);
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
            this.grpDataCategory.Location = new System.Drawing.Point(258, 12);
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
            this.btnExport.Location = new System.Drawing.Point(551, 180);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(125, 22);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // grpData
            // 
            this.grpData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpData.Controls.Add(this.dataGridView1);
            this.grpData.Location = new System.Drawing.Point(12, 161);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(533, 269);
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
            this.dataGridView1.Size = new System.Drawing.Size(527, 247);
            this.dataGridView1.TabIndex = 0;
            // 
            // chkExportOnlyGasRegistanceLogarithm
            // 
            this.chkExportOnlyGasRegistanceLogarithm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkExportOnlyGasRegistanceLogarithm.AutoSize = true;
            this.chkExportOnlyGasRegistanceLogarithm.Checked = true;
            this.chkExportOnlyGasRegistanceLogarithm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportOnlyGasRegistanceLogarithm.Location = new System.Drawing.Point(551, 208);
            this.chkExportOnlyGasRegistanceLogarithm.Name = "chkExportOnlyGasRegistanceLogarithm";
            this.chkExportOnlyGasRegistanceLogarithm.Size = new System.Drawing.Size(114, 19);
            this.chkExportOnlyGasRegistanceLogarithm.TabIndex = 9;
            this.chkExportOnlyGasRegistanceLogarithm.Text = "Only Gas R.(Log)";
            this.chkExportOnlyGasRegistanceLogarithm.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(551, 405);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(125, 22);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnShowGraph
            // 
            this.btnShowGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowGraph.Location = new System.Drawing.Point(551, 293);
            this.btnShowGraph.Name = "btnShowGraph";
            this.btnShowGraph.Size = new System.Drawing.Size(125, 23);
            this.btnShowGraph.TabIndex = 11;
            this.btnShowGraph.Text = "Show Graph";
            this.btnShowGraph.UseVisualStyleBackColor = true;
            this.btnShowGraph.Visible = false;
            this.btnShowGraph.Click += new System.EventHandler(this.btnShowGraph_Click);
            // 
            // numDuplicate
            // 
            this.numDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numDuplicate.Location = new System.Drawing.Point(611, 239);
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
            this.numDuplicate.Size = new System.Drawing.Size(65, 23);
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
            this.lblDuplicate.Location = new System.Drawing.Point(548, 241);
            this.lblDuplicate.Name = "lblDuplicate";
            this.lblDuplicate.Size = new System.Drawing.Size(57, 15);
            this.lblDuplicate.TabIndex = 13;
            this.lblDuplicate.Text = "Duplicate";
            this.lblDuplicate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SerialCommForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 442);
            this.Controls.Add(this.lblDuplicate);
            this.Controls.Add(this.numDuplicate);
            this.Controls.Add(this.btnShowGraph);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.chkExportOnlyGasRegistanceLogarithm);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.grpData);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.grpDataCategory);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.grpPort);
            this.Name = "SerialCommForm";
            this.Text = "BME688Serial";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SerialCommForm_FormClosing);
            this.Load += new System.EventHandler(this.SerialCommForm_Load);
            this.grpPort.ResumeLayout(false);
            this.grpPort.PerformLayout();
            this.grpDataCategory.ResumeLayout(false);
            this.grpDataCategory.PerformLayout();
            this.grpData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDuplicate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}
