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
            gridDbStatus.Size = new Size(458, 278);
            gridDbStatus.TabIndex = 0;
            // 
            // DbStatusDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(482, 302);
            Controls.Add(gridDbStatus);
            Name = "DbStatusDialog";
            Text = "Dabase Status";
            ((System.ComponentModel.ISupportInitialize)gridDbStatus).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView gridDbStatus;
    }
}