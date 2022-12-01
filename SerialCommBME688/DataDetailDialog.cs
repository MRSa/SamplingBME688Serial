using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    class DataDetailDialog : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button? btnClose;
        private System.ComponentModel.Container? components = null;

        public DataDetailDialog()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();

            this.SuspendLayout();

            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(48, 32);
            this.btnClose.Name = "btnClose";
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);


            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.AcceptButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(184, 78);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Detail";
            this.Text = "Data Detail";

            this.ResumeLayout(false);
        }

        private void btnClose_Click(object? sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
