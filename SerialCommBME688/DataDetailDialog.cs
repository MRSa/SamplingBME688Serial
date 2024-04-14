using SerialCommBME688;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace SamplingBME688Serial
{
    class DataDetailDialog : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button? btnClose;
        private System.ComponentModel.Container? components = null;
        private DrawDataGraph graphDrawer = new DrawDataGraph();



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
            btnClose = new Button();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(248, 319);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(88, 30);
            btnClose.TabIndex = 0;
            btnClose.Text = "Close";
            btnClose.Click += btnClose_Click;
            // 
            // DataDetailDialog
            // 
            AcceptButton = btnClose;
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(584, 361);
            Controls.Add(btnClose);
            DoubleBuffered = true;
            Name = "DataDetailDialog";
            Text = "Data Detail";
            Load += DataDetailDialog_Load;
            ResumeLayout(false);
        }

        private void btnClose_Click(object? sender, System.EventArgs e)
        {
            this.Close();
        }

        private void DataDetailDialog_Load(object sender, EventArgs e)
        {

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 画面のグラフィック描画

            // ウィンドウサイズ
            int margin = 10;
            int topMargin = 5;
            int bottomMargin = 90;

            // ----- 描画領域サイズの決定
            float topLeftX = margin;
            float topLeftY = topMargin;
            float areaWidth = Size.Width - (margin * 4);
            float areaHeight = Size.Height - bottomMargin;


            // Graphics オブジェクトを取得
            Graphics g = e.Graphics;

            // 背景領域の描画
            RectangleF drawArea = new RectangleF(topLeftX, topLeftY, areaWidth, areaHeight);
            graphDrawer.drawBackground(g, drawArea);

            // 軸の描画
            graphDrawer.drawAixs(g, drawArea);

            // 凡例の描画
            graphDrawer.drawUsage(g, drawArea);

            // グラフの描画
            graphDrawer.drawGraph(g, drawArea);
        }

        public void setSelectedData(ref Dictionary<int, DataGridViewRow> selectedData,  Dictionary<String, List<List<double>>> dataSet1, Dictionary<String, List<List<double>>> dataSet2)
        {
            // 描画クラスに描画するデータを送り込む
            graphDrawer.setDataToDraw(ref selectedData, dataSet1, dataSet2);
        }
    }
}
