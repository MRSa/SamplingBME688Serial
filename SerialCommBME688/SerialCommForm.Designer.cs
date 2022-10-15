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
            this.grpPort.SuspendLayout();
            this.grpDataCategory.SuspendLayout();
            this.grpData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.txtConsole.Size = new System.Drawing.Size(644, 117);
            this.txtConsole.TabIndex = 4;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(583, 33);
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
            this.btnExport.Location = new System.Drawing.Point(583, 218);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(79, 22);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "CSV Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // grpData
            // 
            this.grpData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpData.Controls.Add(this.dataGridView1);
            this.grpData.Location = new System.Drawing.Point(12, 199);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(563, 269);
            this.grpData.TabIndex = 8;
            this.grpData.TabStop = false;
            this.grpData.Text = "Data";
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
            this.dataGridView1.Size = new System.Drawing.Size(557, 247);
            this.dataGridView1.TabIndex = 0;
            // 
            // SerialCommForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 480);
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
    }
}
