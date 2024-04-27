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
        private TrackBar bar1;
        private TrackBar bar3;
        private TrackBar bar2;
        private Label label1Value;
        private Label label2Value;
        private Label label3Value;
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
            bar1 = new TrackBar();
            bar3 = new TrackBar();
            bar2 = new TrackBar();
            label1Value = new Label();
            label2Value = new Label();
            label3Value = new Label();
            ((System.ComponentModel.ISupportInitialize)bar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bar3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bar2).BeginInit();
            SuspendLayout();
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNext.DialogResult = DialogResult.Cancel;
            btnNext.Location = new Point(113, 524);
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
            btnPrev.Location = new Point(12, 524);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(35, 30);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "←";
            btnPrev.Click += btnPrev_Click;
            // 
            // fldIndex
            // 
            fldIndex.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            fldIndex.Location = new Point(53, 529);
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
            lblSelectedIndex.Location = new Point(12, 506);
            lblSelectedIndex.Name = "lblSelectedIndex";
            lblSelectedIndex.Size = new Size(28, 15);
            lblSelectedIndex.TabIndex = 4;
            lblSelectedIndex.Text = "XXX";
            // 
            // bar1
            // 
            bar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            bar1.Location = new Point(198, 509);
            bar1.Maximum = 99;
            bar1.Name = "bar1";
            bar1.Size = new Size(150, 45);
            bar1.TabIndex = 5;
            bar1.Scroll += bar1_Scroll;
            // 
            // bar3
            // 
            bar3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            bar3.Location = new Point(622, 509);
            bar3.Maximum = 99;
            bar3.Name = "bar3";
            bar3.Size = new Size(150, 45);
            bar3.TabIndex = 7;
            bar3.Value = 99;
            bar3.Scroll += bar3_Scroll;
            // 
            // bar2
            // 
            bar2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            bar2.Location = new Point(407, 509);
            bar2.Maximum = 99;
            bar2.Name = "bar2";
            bar2.Size = new Size(150, 45);
            bar2.TabIndex = 6;
            bar2.Value = 49;
            bar2.Scroll += bar2_Scroll;
            // 
            // label1Value
            // 
            label1Value.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1Value.AutoSize = true;
            label1Value.Location = new Point(154, 509);
            label1Value.Name = "label1Value";
            label1Value.Size = new Size(38, 15);
            label1Value.TabIndex = 8;
            label1Value.Text = "label1";
            // 
            // label2Value
            // 
            label2Value.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2Value.AutoSize = true;
            label2Value.Location = new Point(363, 509);
            label2Value.Name = "label2Value";
            label2Value.Size = new Size(38, 15);
            label2Value.TabIndex = 9;
            label2Value.Text = "label2";
            // 
            // label3Value
            // 
            label3Value.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3Value.AutoSize = true;
            label3Value.Location = new Point(578, 509);
            label3Value.Name = "label3Value";
            label3Value.Size = new Size(38, 15);
            label3Value.TabIndex = 10;
            label3Value.Text = "label3";
            // 
            // DataDetailDialog
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(784, 561);
            Controls.Add(label3Value);
            Controls.Add(label2Value);
            Controls.Add(label1Value);
            Controls.Add(bar2);
            Controls.Add(bar3);
            Controls.Add(bar1);
            Controls.Add(lblSelectedIndex);
            Controls.Add(fldIndex);
            Controls.Add(btnPrev);
            Controls.Add(btnNext);
            DoubleBuffered = true;
            Name = "DataDetailDialog";
            Text = "Data Detail";
            Load += DataDetailDialog_Load;
            ((System.ComponentModel.ISupportInitialize)bar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)bar3).EndInit();
            ((System.ComponentModel.ISupportInitialize)bar2).EndInit();
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
            int bottomMargin = 120;

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
            graphDrawer.drawGraph(g, drawArea, currentIndexNumber, bar1.Value / 100.0f, bar2.Value / 100.0f, bar3.Value / 100.0f);
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

            label1Value.Text = bar1.Value + "%";
            label2Value.Text = bar2.Value + "%";
            label3Value.Text = bar3.Value + "%";
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

        private void bar1_Scroll(object sender, EventArgs e)
        {
            label1Value.Text = bar1.Value + "%";
            this.Invalidate();
        }

        private void bar2_Scroll(object sender, EventArgs e)
        {
            label2Value.Text = bar2.Value + "%";
            this.Invalidate();
        }

        private void bar3_Scroll(object sender, EventArgs e)
        {
            label3Value.Text = bar3.Value + "%";
            this.Invalidate();
        }
    }
}
