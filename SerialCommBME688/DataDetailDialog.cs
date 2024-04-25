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
        private System.ComponentModel.Container? components = null;
        private Button btnNext;
        private Button btnPrev;
        private TextBox fldIndex;
        private DrawDataGraph graphDrawer = new DrawDataGraph();
        private int currentIndexNumber = 1;
        private Label lblSelectedIndex;
        private int maxIndexNumber = 1;
        private List<String> labelList = new List<String>();



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
            btnNext = new Button();
            btnPrev = new Button();
            fldIndex = new TextBox();
            lblSelectedIndex = new Label();
            SuspendLayout();
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNext.DialogResult = DialogResult.Cancel;
            btnNext.Location = new Point(113, 324);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(35, 30);
            btnNext.TabIndex = 3;
            btnNext.Text = "→";
            btnNext.Click += btnNext_Click;
            // 
            // btnPrev
            // 
            btnPrev.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPrev.DialogResult = DialogResult.Cancel;
            btnPrev.Location = new Point(12, 324);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(35, 30);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "←";
            btnPrev.Click += btnPrev_Click;
            // 
            // fldIndex
            // 
            fldIndex.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            fldIndex.Location = new Point(53, 329);
            fldIndex.Name = "fldIndex";
            fldIndex.ReadOnly = true;
            fldIndex.Size = new Size(54, 23);
            fldIndex.TabIndex = 2;
            fldIndex.TextAlign = HorizontalAlignment.Center;
            // 
            // lblSelectedIndex
            // 
            lblSelectedIndex.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblSelectedIndex.AutoSize = true;
            lblSelectedIndex.Location = new Point(154, 332);
            lblSelectedIndex.Name = "lblSelectedIndex";
            lblSelectedIndex.Size = new Size(0, 15);
            lblSelectedIndex.TabIndex = 4;
            // 
            // DataDetailDialog
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(584, 361);
            Controls.Add(lblSelectedIndex);
            Controls.Add(fldIndex);
            Controls.Add(btnPrev);
            Controls.Add(btnNext);
            DoubleBuffered = true;
            Name = "DataDetailDialog";
            Text = "Data Detail";
            Load += DataDetailDialog_Load;
            ResumeLayout(false);
            PerformLayout();
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
            graphDrawer.drawGraph(g, drawArea, currentIndexNumber);
        }

        public void setSelectedData(ref Dictionary<int, DataGridViewRow> selectedData, Dictionary<String, List<List<double>>> dataSet1, Dictionary<String, List<List<double>>> dataSet2)
        {
            // 描画クラスに描画するデータを送り込む
            graphDrawer.setDataToDraw(ref selectedData, dataSet1, dataSet2);

            maxIndexNumber = selectedData.Count;

            fldIndex.Text = currentIndexNumber + "/" + maxIndexNumber;

            labelList.Clear();

            foreach (KeyValuePair<int, DataGridViewRow> pair in selectedData)
            {
                DataGridViewRow rowData = pair.Value;
                String sensorIdStr = rowData.Cells[1].Value.ToString() ?? "1";
                int sensorId = int.Parse(sensorIdStr);
                String? key = rowData.Cells[0].Value.ToString();
                String categoryName = key ?? "";
                labelList.Add(categoryName + "(" + sensorIdStr + ")");
            }
            lblSelectedIndex.Text = labelList[0];
         }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            currentIndexNumber--;
            if (currentIndexNumber <= 0)
            {
                currentIndexNumber = maxIndexNumber;
            }
            fldIndex.Text = currentIndexNumber + "/" + maxIndexNumber;
            lblSelectedIndex.Text = labelList[currentIndexNumber - 1];
            this.Invalidate();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            currentIndexNumber++;
            if (currentIndexNumber > maxIndexNumber)
            {
                currentIndexNumber = 1;
            }
            fldIndex.Text = currentIndexNumber + "/" + maxIndexNumber;
            lblSelectedIndex.Text = labelList[currentIndexNumber - 1];
            this.Invalidate();
        }
    }
}
