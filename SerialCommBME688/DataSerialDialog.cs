
using System.Diagnostics;

namespace SamplingBME688Serial
{
    class DataSerialDialog : System.Windows.Forms.Form
    {
        private System.ComponentModel.Container? components = null;
        private Button btnNext;
        private Button btnPrev;
        private TextBox fldIndex;
        private DrawDataGraphSerial graphDrawer = new DrawDataGraphSerial();
        private int currentIndexNumber = 1;
        private Label lblSelectedIndex;
        private int maxIndexNumber = 1;
        private CheckBox chkRangeZoom;
        private CheckBox chkLogRData;
        private Label lblHumidity;
        private Label lblTemperature;
        private Label lblPressure;
        private Label lblMessage;
        private CheckBox chkStep0;
        private CheckBox chkStep1;
        private CheckBox chkStep2;
        private CheckBox chkStep3;
        private CheckBox chkStep4;
        private CheckBox chkStep5;
        private CheckBox chkStep6;
        private CheckBox chkStep7;
        private CheckBox chkStep8;
        private CheckBox chkStep9;
        private CheckBox chkHumidity;
        private CheckBox chkTemperature;
        private CheckBox chkPressure;
        private List<string> labelList = new List<string>();

        private bool isPointed = false;
        private double pointedPositionX = 0.0d;
        private double pointedPositionY = 0.0d;

        public DataSerialDialog()
        {
            InitializeComponent();
            this.MouseClick += new MouseEventHandler(DataSerialDialog_MouseClick);
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
            chkRangeZoom = new CheckBox();
            chkLogRData = new CheckBox();
            lblPressure = new Label();
            lblMessage = new Label();
            lblTemperature = new Label();
            lblHumidity = new Label();
            chkStep0 = new CheckBox();
            chkStep1 = new CheckBox();
            chkStep2 = new CheckBox();
            chkStep3 = new CheckBox();
            chkStep4 = new CheckBox();
            chkStep5 = new CheckBox();
            chkStep6 = new CheckBox();
            chkStep7 = new CheckBox();
            chkStep8 = new CheckBox();
            chkStep9 = new CheckBox();
            chkHumidity = new CheckBox();
            chkTemperature = new CheckBox();
            chkPressure = new CheckBox();
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
            // chkRangeZoom
            // 
            chkRangeZoom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
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
            chkLogRData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkLogRData.AutoSize = true;
            chkLogRData.Location = new Point(905, 504);
            chkLogRData.Name = "chkLogRData";
            chkLogRData.Size = new Size(46, 19);
            chkLogRData.TabIndex = 12;
            chkLogRData.Text = "Log";
            chkLogRData.UseVisualStyleBackColor = true;
            chkLogRData.CheckedChanged += chkLogRData_CheckedChanged;
            // 
            // lblPressure
            // 
            lblPressure.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblPressure.AutoSize = true;
            lblPressure.ImageAlign = ContentAlignment.TopLeft;
            lblPressure.Location = new Point(250, 535);
            lblPressure.Name = "lblPressure";
            lblPressure.Size = new Size(28, 15);
            lblPressure.TabIndex = 17;
            lblPressure.Text = "XXX";
            // 
            // lblMessage
            // 
            lblMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblMessage.AutoSize = true;
            lblMessage.ImageAlign = ContentAlignment.TopLeft;
            lblMessage.Location = new Point(828, 535);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(28, 15);
            lblMessage.TabIndex = 20;
            lblMessage.Text = "XXX";
            // 
            // lblTemperature
            // 
            lblTemperature.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTemperature.AutoSize = true;
            lblTemperature.ImageAlign = ContentAlignment.TopLeft;
            lblTemperature.Location = new Point(487, 535);
            lblTemperature.Name = "lblTemperature";
            lblTemperature.Size = new Size(28, 15);
            lblTemperature.TabIndex = 21;
            lblTemperature.Text = "XXX";
            // 
            // lblHumidity
            // 
            lblHumidity.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblHumidity.AutoSize = true;
            lblHumidity.ImageAlign = ContentAlignment.TopLeft;
            lblHumidity.Location = new Point(687, 535);
            lblHumidity.Name = "lblHumidity";
            lblHumidity.Size = new Size(28, 15);
            lblHumidity.TabIndex = 24;
            lblHumidity.Text = "XXX";
            // 
            // chkStep0
            // 
            chkStep0.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkStep0.AutoSize = true;
            chkStep0.Checked = true;
            chkStep0.CheckState = CheckState.Checked;
            chkStep0.Location = new Point(174, 504);
            chkStep0.Name = "chkStep0";
            chkStep0.Size = new Size(55, 19);
            chkStep0.TabIndex = 26;
            chkStep0.Text = "Step0";
            chkStep0.UseVisualStyleBackColor = true;
            chkStep0.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkStep1
            // 
            chkStep1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkStep1.AutoSize = true;
            chkStep1.Checked = true;
            chkStep1.CheckState = CheckState.Checked;
            chkStep1.Location = new Point(246, 504);
            chkStep1.Name = "chkStep1";
            chkStep1.Size = new Size(55, 19);
            chkStep1.TabIndex = 27;
            chkStep1.Text = "Step1";
            chkStep1.UseVisualStyleBackColor = true;
            chkStep1.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkStep2
            // 
            chkStep2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkStep2.AutoSize = true;
            chkStep2.Checked = true;
            chkStep2.CheckState = CheckState.Checked;
            chkStep2.Location = new Point(318, 504);
            chkStep2.Name = "chkStep2";
            chkStep2.Size = new Size(55, 19);
            chkStep2.TabIndex = 28;
            chkStep2.Text = "Step2";
            chkStep2.UseVisualStyleBackColor = true;
            chkStep2.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkStep3
            // 
            chkStep3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkStep3.AutoSize = true;
            chkStep3.Checked = true;
            chkStep3.CheckState = CheckState.Checked;
            chkStep3.Location = new Point(390, 504);
            chkStep3.Name = "chkStep3";
            chkStep3.Size = new Size(55, 19);
            chkStep3.TabIndex = 29;
            chkStep3.Text = "Step3";
            chkStep3.UseVisualStyleBackColor = true;
            chkStep3.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkStep4
            // 
            chkStep4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkStep4.AutoSize = true;
            chkStep4.Checked = true;
            chkStep4.CheckState = CheckState.Checked;
            chkStep4.Location = new Point(462, 504);
            chkStep4.Name = "chkStep4";
            chkStep4.Size = new Size(55, 19);
            chkStep4.TabIndex = 30;
            chkStep4.Text = "Step4";
            chkStep4.UseVisualStyleBackColor = true;
            chkStep4.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkStep5
            // 
            chkStep5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkStep5.AutoSize = true;
            chkStep5.Checked = true;
            chkStep5.CheckState = CheckState.Checked;
            chkStep5.Location = new Point(534, 504);
            chkStep5.Name = "chkStep5";
            chkStep5.Size = new Size(55, 19);
            chkStep5.TabIndex = 31;
            chkStep5.Text = "Step5";
            chkStep5.UseVisualStyleBackColor = true;
            chkStep5.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkStep6
            // 
            chkStep6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkStep6.AutoSize = true;
            chkStep6.Checked = true;
            chkStep6.CheckState = CheckState.Checked;
            chkStep6.Location = new Point(606, 504);
            chkStep6.Name = "chkStep6";
            chkStep6.Size = new Size(55, 19);
            chkStep6.TabIndex = 32;
            chkStep6.Text = "Step6";
            chkStep6.UseVisualStyleBackColor = true;
            chkStep6.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkStep7
            // 
            chkStep7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkStep7.AutoSize = true;
            chkStep7.Checked = true;
            chkStep7.CheckState = CheckState.Checked;
            chkStep7.Location = new Point(678, 504);
            chkStep7.Name = "chkStep7";
            chkStep7.Size = new Size(55, 19);
            chkStep7.TabIndex = 33;
            chkStep7.Text = "Step7";
            chkStep7.UseVisualStyleBackColor = true;
            chkStep7.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkStep8
            // 
            chkStep8.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkStep8.AutoSize = true;
            chkStep8.Checked = true;
            chkStep8.CheckState = CheckState.Checked;
            chkStep8.Location = new Point(750, 504);
            chkStep8.Name = "chkStep8";
            chkStep8.Size = new Size(55, 19);
            chkStep8.TabIndex = 34;
            chkStep8.Text = "Step8";
            chkStep8.UseVisualStyleBackColor = true;
            chkStep8.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkStep9
            // 
            chkStep9.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkStep9.AutoSize = true;
            chkStep9.Checked = true;
            chkStep9.CheckState = CheckState.Checked;
            chkStep9.Location = new Point(828, 504);
            chkStep9.Name = "chkStep9";
            chkStep9.Size = new Size(55, 19);
            chkStep9.TabIndex = 35;
            chkStep9.Text = "Step9";
            chkStep9.UseVisualStyleBackColor = true;
            chkStep9.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkHumidity
            // 
            chkHumidity.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkHumidity.AutoSize = true;
            chkHumidity.Location = new Point(606, 535);
            chkHumidity.Name = "chkHumidity";
            chkHumidity.Size = new Size(75, 19);
            chkHumidity.TabIndex = 36;
            chkHumidity.Text = "Humidity";
            chkHumidity.UseVisualStyleBackColor = true;
            chkHumidity.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkTemperature
            // 
            chkTemperature.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkTemperature.AutoSize = true;
            chkTemperature.Location = new Point(390, 535);
            chkTemperature.Name = "chkTemperature";
            chkTemperature.Size = new Size(91, 19);
            chkTemperature.TabIndex = 37;
            chkTemperature.Text = "Temperature";
            chkTemperature.UseVisualStyleBackColor = true;
            chkTemperature.CheckedChanged += chkStep_CheckedChanged;
            // 
            // chkPressure
            // 
            chkPressure.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkPressure.AutoSize = true;
            chkPressure.Location = new Point(174, 535);
            chkPressure.Name = "chkPressure";
            chkPressure.Size = new Size(70, 19);
            chkPressure.TabIndex = 38;
            chkPressure.Text = "Pressure";
            chkPressure.UseVisualStyleBackColor = true;
            chkPressure.CheckedChanged += chkStep_CheckedChanged;
            // 
            // DataSerialDialog
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(974, 561);
            Controls.Add(chkPressure);
            Controls.Add(chkTemperature);
            Controls.Add(chkHumidity);
            Controls.Add(chkStep9);
            Controls.Add(chkStep8);
            Controls.Add(chkStep7);
            Controls.Add(chkStep6);
            Controls.Add(chkStep5);
            Controls.Add(chkStep4);
            Controls.Add(chkStep3);
            Controls.Add(chkStep2);
            Controls.Add(chkStep1);
            Controls.Add(chkStep0);
            Controls.Add(lblHumidity);
            Controls.Add(lblTemperature);
            Controls.Add(lblMessage);
            Controls.Add(lblPressure);
            Controls.Add(chkLogRData);
            Controls.Add(chkRangeZoom);
            Controls.Add(lblSelectedIndex);
            Controls.Add(fldIndex);
            Controls.Add(btnPrev);
            Controls.Add(btnNext);
            DoubleBuffered = true;
            Name = "DataSerialDialog";
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
            graphDrawer.drawAixs(g, drawArea, currentIndexNumber,
                chkStep0.Checked, chkStep1.Checked, chkStep2.Checked, chkStep3.Checked, chkStep4.Checked,
                chkStep5.Checked, chkStep6.Checked, chkStep7.Checked, chkStep8.Checked, chkStep9.Checked,
                chkPressure.Checked, chkTemperature.Checked, chkHumidity.Checked);

            // 凡例の描画
            graphDrawer.drawUsage(g, drawArea);

            // グラフの描画
            graphDrawer.drawGraph(g, drawArea, currentIndexNumber,
                chkStep0.Checked, chkStep1.Checked, chkStep2.Checked, chkStep3.Checked, chkStep4.Checked,
                chkStep5.Checked, chkStep6.Checked, chkStep7.Checked, chkStep8.Checked, chkStep9.Checked,
                chkPressure.Checked, chkTemperature.Checked, chkHumidity.Checked);

            if (isPointed)
            {
                //  マウスをクリックした場合には、グラフのインジケーターを描画する
                graphDrawer.drawDataIndicator(g, drawArea, currentIndexNumber, pointedPositionX, pointedPositionY,
                                    chkStep0.Checked, chkStep1.Checked, chkStep2.Checked, chkStep3.Checked, chkStep4.Checked,
                                    chkStep5.Checked, chkStep6.Checked, chkStep7.Checked, chkStep8.Checked, chkStep9.Checked,
                                    chkPressure.Checked, chkTemperature.Checked, chkHumidity.Checked);
            }

            // 温度、圧力、湿度の上下限の値を表示する
            lblTemperature.Text = graphDrawer.getTemperatureRangeStr();
            lblPressure.Text = graphDrawer.getPressureRangeStr();
            lblHumidity.Text = graphDrawer.getHumidityRangeStr();
        }

        public void setSelectedData(ref Dictionary<int, DataGridViewRow> selectedData, Dictionary<string, List<List<GraphDataValue>>> dataSet1, Dictionary<string, List<List<GraphDataValue>>> dataSet2, GraphDataValue lowerLimit, GraphDataValue upperLimit, GraphDataValue lowerLimitZoom, GraphDataValue upperLimitZoom)
        {
            // 描画クラスに描画するデータを送り込む
            graphDrawer.setDataToDraw(ref selectedData, dataSet1, dataSet2, lowerLimit, upperLimit, lowerLimitZoom, upperLimitZoom);

            maxIndexNumber = selectedData.Count;

            fldIndex.Text = currentIndexNumber + "/" + maxIndexNumber;

            labelList.Clear();

            foreach (KeyValuePair<int, DataGridViewRow> pair in selectedData)
            {
                DataGridViewRow rowData = pair.Value;
                string sensorIdStr = rowData.Cells[1].Value.ToString() ?? "1";
                //int sensorId = int.Parse(sensorIdStr);
                string? key = rowData.Cells[0].Value.ToString();
                string categoryName = key ?? "";
                labelList.Add(categoryName + "(" + sensorIdStr + ")");
            }
            lblSelectedIndex.Text = labelList[0];

            lblPressure.Text = "";
            lblTemperature.Text = "";
            lblHumidity.Text = "";
            lblMessage.Text = "";
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

        private void chkStep_CheckedChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void DataSerialDialog_MouseClick(object sender, MouseEventArgs e)
        {
            // ウィンドウサイズ
            int margin = 10;
            int topMargin = 5;

            // ----- 描画領域サイズの決定
            float topLeftX = margin;
            float topLeftY = topMargin;
            float areaWidth = Size.Width - (margin * 4);
            float areaHeight = lblSelectedIndex.Location.Y - (topMargin * 3);

            // ----- マウスクリックした位置に線をひく
            pointedPositionX = e.X; //  - topLeftX;
            pointedPositionY = e.Y; //  - topLeftY;
            if ((pointedPositionX >= 0.0f)&&(pointedPositionX <= areaWidth)&&
                (pointedPositionY >= 0.0f)&&(pointedPositionY <= areaHeight))
            {
                isPointed = true;
            }
            else
            {
                isPointed = false;
            }
            // Debug.WriteLine($"Mouse Click: X={e.X}, Y={e.Y},  ({pointedPositionX},{pointedPositionY}) IN :{isPointed} ");
            this.Invalidate();
        }
    }
}
