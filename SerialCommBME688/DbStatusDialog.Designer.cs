namespace SamplingBME688Serial
{
    partial class DbStatusDialog
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
            gridDbStatus = new DataGridView();
            btnLoad = new Button();
            btnCheckUncheckAll = new Button();
            lblFrom = new Label();
            lblCount = new Label();
            fldFrom = new NumericUpDown();
            fldCount = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)gridDbStatus).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fldFrom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fldCount).BeginInit();
            SuspendLayout();
            // 
            // gridDbStatus
            // 
            gridDbStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridDbStatus.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridDbStatus.Location = new Point(12, 12);
            gridDbStatus.Name = "gridDbStatus";
            gridDbStatus.RowTemplate.Height = 25;
            gridDbStatus.Size = new Size(560, 313);
            gridDbStatus.TabIndex = 0;
            // 
            // btnLoad
            // 
            btnLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLoad.Enabled = false;
            btnLoad.Location = new Point(497, 331);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(75, 23);
            btnLoad.TabIndex = 2;
            btnLoad.Text = "Load Data";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Visible = false;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnCheckUncheckAll
            // 
            btnCheckUncheckAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCheckUncheckAll.Enabled = false;
            btnCheckUncheckAll.Location = new Point(12, 331);
            btnCheckUncheckAll.Name = "btnCheckUncheckAll";
            btnCheckUncheckAll.Size = new Size(120, 23);
            btnCheckUncheckAll.TabIndex = 3;
            btnCheckUncheckAll.Text = "Check/Uncheck All";
            btnCheckUncheckAll.UseVisualStyleBackColor = true;
            btnCheckUncheckAll.Visible = false;
            btnCheckUncheckAll.Click += btnCheckUncheckAll_Click;
            // 
            // lblFrom
            // 
            lblFrom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblFrom.AutoSize = true;
            lblFrom.Location = new Point(226, 335);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(33, 15);
            lblFrom.TabIndex = 4;
            lblFrom.Text = "From";
            // 
            // lblCount
            // 
            lblCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCount.AutoSize = true;
            lblCount.Location = new Point(366, 335);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(39, 15);
            lblCount.TabIndex = 5;
            lblCount.Text = "Count";
            // 
            // fldFrom
            // 
            fldFrom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            fldFrom.Location = new Point(265, 333);
            fldFrom.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            fldFrom.Name = "fldFrom";
            fldFrom.Size = new Size(80, 23);
            fldFrom.TabIndex = 8;
            // 
            // fldCount
            // 
            fldCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            fldCount.Location = new Point(411, 333);
            fldCount.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            fldCount.Name = "fldCount";
            fldCount.Size = new Size(80, 23);
            fldCount.TabIndex = 9;
            fldCount.Value = new decimal(new int[] { 1000000, 0, 0, 0 });
            // 
            // DbStatusDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 361);
            Controls.Add(fldCount);
            Controls.Add(fldFrom);
            Controls.Add(lblCount);
            Controls.Add(lblFrom);
            Controls.Add(btnCheckUncheckAll);
            Controls.Add(btnLoad);
            Controls.Add(gridDbStatus);
            Name = "DbStatusDialog";
            Text = "Dabase Entry Status";
            ((System.ComponentModel.ISupportInitialize)gridDbStatus).EndInit();
            ((System.ComponentModel.ISupportInitialize)fldFrom).EndInit();
            ((System.ComponentModel.ISupportInitialize)fldCount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView gridDbStatus;
        private Button btnLoad;
        private Button btnCheckUncheckAll;
        private Label lblFrom;
        private Label lblCount;
        private NumericUpDown fldFrom;
        private NumericUpDown fldCount;
    }
}