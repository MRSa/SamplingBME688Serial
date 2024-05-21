
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
        private CheckBox chkRangeZoom;
        private CheckBox chkLogRData;
        private CheckBox chk3Visible;
        private CheckBox chk2Visible;
        private CheckBox chk1Visible;
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
            chkRangeZoom = new CheckBox();
            chkLogRData = new CheckBox();
            chk3Visible = new CheckBox();
            chk2Visible = new CheckBox();
            chk1Visible = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)bar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bar3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bar2).BeginInit();
            SuspendLayout();
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNext.DialogResult = DialogResult.Cancel;
            btnNext.Location = new Point(113, 517);
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
            btnPrev.Location = new Point(12, 517);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(35, 30);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "←";
            btnPrev.Click += btnPrev_Click;
            // 
            // fldIndex
            // 
            fldIndex.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            fldIndex.Location = new Point(53, 522);
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
            lblSelectedIndex.ImageAlign = ContentAlignment.TopLeft;
            lblSelectedIndex.Location = new Point(12, 499);
            lblSelectedIndex.Name = "lblSelectedIndex";
            lblSelectedIndex.Size = new Size(28, 15);
            lblSelectedIndex.TabIndex = 4;
            lblSelectedIndex.Text = "XXX";
            // 
            // bar1
            // 
            bar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            bar1.Location = new Point(254, 499);
            bar1.Maximum = 99;
            bar1.Name = "bar1";
            bar1.Size = new Size(150, 45);
            bar1.TabIndex = 5;
            bar1.Scroll += bar1_Scroll;
            // 
            // bar3
            // 
            bar3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            bar3.Location = new Point(738, 499);
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
            bar2.Location = new Point(499, 499);
            bar2.Maximum = 99;
            bar2.Name = "bar2";
            bar2.Size = new Size(150, 45);
            bar2.TabIndex = 6;
            bar2.Value = 49;
            bar2.Scroll += bar2_Scroll;
            // 
            // chkRangeZoom
            // 
            chkRangeZoom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chkRangeZoom.AutoSize = true;
            chkRangeZoom.Location = new Point(905, 535);
            chkRangeZoom.Name = "chkRangeZoom";
            chkRangeZoom.Size = new Size(57, 19);
            chkRangeZoom.TabIndex = 11;
            chkRangeZoom.Text = "Zoom";
            chkRangeZoom.UseVisualStyleBackColor = true;
            chkRangeZoom.CheckedChanged += chkRangeZoom_CheckedChanged;
            // 
            // chkLogRData
            // 
            chkLogRData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chkLogRData.AutoSize = true;
            chkLogRData.Location = new Point(905, 504);
            chkLogRData.Name = "chkLogRData";
            chkLogRData.Size = new Size(46, 19);
            chkLogRData.TabIndex = 12;
            chkLogRData.Text = "Log";
            chkLogRData.UseVisualStyleBackColor = true;
            chkLogRData.CheckedChanged += chkLogRData_CheckedChanged;
            // 
            // chk3Visible
            // 
            chk3Visible.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chk3Visible.AutoSize = true;
            chk3Visible.Checked = true;
            chk3Visible.CheckState = CheckState.Checked;
            chk3Visible.Location = new Point(675, 504);
            chk3Visible.Name = "chk3Visible";
            chk3Visible.Size = new Size(57, 19);
            chk3Visible.TabIndex = 13;
            chk3Visible.Text = "label3";
            chk3Visible.UseVisualStyleBackColor = true;
            chk3Visible.CheckedChanged += chk3Visible_CheckedChanged;
            // 
            // chk2Visible
            // 
            chk2Visible.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chk2Visible.AutoSize = true;
            chk2Visible.Checked = true;
            chk2Visible.CheckState = CheckState.Checked;
            chk2Visible.Location = new Point(437, 504);
            chk2Visible.Name = "chk2Visible";
            chk2Visible.Size = new Size(57, 19);
            chk2Visible.TabIndex = 14;
            chk2Visible.Text = "label2";
            chk2Visible.UseVisualStyleBackColor = true;
            chk2Visible.CheckedChanged += chk2Visible_CheckedChanged;
            // 
            // chk1Visible
            // 
            chk1Visible.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chk1Visible.AutoSize = true;
            chk1Visible.Checked = true;
            chk1Visible.CheckState = CheckState.Checked;
            chk1Visible.Location = new Point(191, 504);
            chk1Visible.Name = "chk1Visible";
            chk1Visible.Size = new Size(57, 19);
            chk1Visible.TabIndex = 15;
            chk1Visible.Text = "label1";
            chk1Visible.UseVisualStyleBackColor = true;
            chk1Visible.CheckedChanged += chk1Visible_CheckedChanged;
            // 
            // DataDetailDialog
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(974, 561);
            Controls.Add(chk1Visible);
            Controls.Add(chk2Visible);
            Controls.Add(chk3Visible);
            Controls.Add(chkLogRData);
            Controls.Add(chkRangeZoom);
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

            // ----- 描画領域サイズの決定
            float topLeftX = margin;
            float topLeftY = topMargin;
            float areaWidth = Size.Width - (margin * 4);
            float areaHeight = lblSelectedIndex.Location.Y - (topMargin * 3);

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

        public void setSelectedData(ref Dictionary<int, DataGridViewRow> selectedData, Dictionary<String, List<List<GraphDataValue>>> dataSet1, Dictionary<String, List<List<GraphDataValue>>> dataSet2, GraphDataValue lowerLimit, GraphDataValue upperLimit, GraphDataValue lowerLimitZoom, GraphDataValue upperLimitZoom)
        {
            // 描画クラスに描画するデータを送り込む
            graphDrawer.setDataToDraw(ref selectedData, dataSet1, dataSet2, lowerLimit, upperLimit, lowerLimitZoom, upperLimitZoom);

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

            chk1Visible.Text = bar1.Value + "%";
            chk2Visible.Text = bar2.Value + "%";
            chk3Visible.Text = bar3.Value + "%";
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
            chk1Visible.Text = bar1.Value + "%";
            this.Invalidate();
        }

        private void bar2_Scroll(object sender, EventArgs e)
        {
            chk2Visible.Text = bar2.Value + "%";
            this.Invalidate();
        }

        private void bar3_Scroll(object sender, EventArgs e)
        {
            chk3Visible.Text = bar3.Value + "%";
            this.Invalidate();
        }

        private void selectGraphData()
        {
            // ---- 表示するグラフデータ、表示幅の更新
            graphDrawer.selectGraphData(chkLogRData.Checked, chkRangeZoom.Checked);
            this.Invalidate();
        }


        private void chkLogRData_CheckedChanged(object sender, EventArgs e)
        {
            selectGraphData();
        }

        private void chkRangeZoom_CheckedChanged(object sender, EventArgs e)
        {
            selectGraphData();
        }

        private void chk1Visible_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chk2Visible_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chk3Visible_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
