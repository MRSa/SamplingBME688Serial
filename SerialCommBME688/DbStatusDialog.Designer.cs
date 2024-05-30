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
            btnClose = new Button();
            btnLoad = new Button();
            ((System.ComponentModel.ISupportInitialize)gridDbStatus).BeginInit();
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
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom;
            btnClose.Location = new Point(256, 331);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 1;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnLoad
            // 
            btnLoad.Anchor = AnchorStyles.Bottom;
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
            // DbStatusDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 361);
            Controls.Add(btnLoad);
            Controls.Add(btnClose);
            Controls.Add(gridDbStatus);
            Name = "DbStatusDialog";
            Text = "Dabase Entry Status";
            ((System.ComponentModel.ISupportInitialize)gridDbStatus).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView gridDbStatus;
        private Button btnClose;
        private Button btnLoad;
    }
}