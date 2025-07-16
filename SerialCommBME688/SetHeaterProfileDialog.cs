
using System.Diagnostics;
using System.Windows.Forms;

namespace SamplingBME688Serial
{
    class SetHeaterProfileDialog : System.Windows.Forms.Form
    {
        private System.ComponentModel.Container? components = null;
        private TextBox fldPortNo;
        private DrawHeaterProfileSettings graphDrawer = new DrawHeaterProfileSettings();
        private int currentIndexNumber = 1;
        private Label lblPortNo;
        private int maxIndexNumber = 1;
        private Button btnLoadProfile;
        private Button btnTransfer;
        private TextBox fldInformation;
        private DataGridView gridHeaterProfile;
        private Label lblHeaterProfile;
        private List<string> labelList = new List<string>();
        private HeaterProfileDataGrid dataGrid = new HeaterProfileDataGrid();

        public SetHeaterProfileDialog()
        {
            InitializeComponent();
        }

        public void setPortNo(String portNo)
        {
            fldPortNo.Text = portNo;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetHeaterProfileDialog));
            fldPortNo = new TextBox();
            lblPortNo = new Label();
            btnLoadProfile = new Button();
            btnTransfer = new Button();
            fldInformation = new TextBox();
            gridHeaterProfile = new DataGridView();
            lblHeaterProfile = new Label();
            ((System.ComponentModel.ISupportInitialize)gridHeaterProfile).BeginInit();
            SuspendLayout();
            // 
            // fldPortNo
            // 
            fldPortNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            fldPortNo.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            fldPortNo.Location = new Point(12, 486);
            fldPortNo.Name = "fldPortNo";
            fldPortNo.ReadOnly = true;
            fldPortNo.Size = new Size(79, 27);
            fldPortNo.TabIndex = 2;
            fldPortNo.TextAlign = HorizontalAlignment.Center;
            // 
            // lblPortNo
            // 
            lblPortNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblPortNo.AutoSize = true;
            lblPortNo.ImageAlign = ContentAlignment.TopLeft;
            lblPortNo.Location = new Point(12, 468);
            lblPortNo.Name = "lblPortNo";
            lblPortNo.Size = new Size(51, 15);
            lblPortNo.TabIndex = 4;
            lblPortNo.Text = "Port No.";
            // 
            // btnLoadProfile
            // 
            btnLoadProfile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLoadProfile.Image = (Image)resources.GetObject("btnLoadProfile.Image");
            btnLoadProfile.ImageAlign = ContentAlignment.MiddleLeft;
            btnLoadProfile.Location = new Point(12, 519);
            btnLoadProfile.Name = "btnLoadProfile";
            btnLoadProfile.Size = new Size(79, 30);
            btnLoadProfile.TabIndex = 5;
            btnLoadProfile.Text = "   Reload";
            btnLoadProfile.UseVisualStyleBackColor = true;
            btnLoadProfile.Click += btnLoadProfile_Click;
            // 
            // btnTransfer
            // 
            btnTransfer.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnTransfer.Enabled = false;
            btnTransfer.Image = (Image)resources.GetObject("btnTransfer.Image");
            btnTransfer.ImageAlign = ContentAlignment.MiddleLeft;
            btnTransfer.Location = new Point(12, 413);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new Size(79, 30);
            btnTransfer.TabIndex = 6;
            btnTransfer.Text = "   Transfer";
            btnTransfer.UseVisualStyleBackColor = true;
            btnTransfer.Click += btnTransfer_Click;
            // 
            // fldInformation
            // 
            fldInformation.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            fldInformation.Location = new Point(97, 449);
            fldInformation.Multiline = true;
            fldInformation.Name = "fldInformation";
            fldInformation.ReadOnly = true;
            fldInformation.ScrollBars = ScrollBars.Both;
            fldInformation.Size = new Size(865, 100);
            fldInformation.TabIndex = 7;
            // 
            // gridHeaterProfile
            // 
            gridHeaterProfile.AllowUserToAddRows = false;
            gridHeaterProfile.AllowUserToDeleteRows = false;
            gridHeaterProfile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridHeaterProfile.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            gridHeaterProfile.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridHeaterProfile.Location = new Point(97, 363);
            gridHeaterProfile.Name = "gridHeaterProfile";
            gridHeaterProfile.Size = new Size(865, 80);
            gridHeaterProfile.TabIndex = 8;
            // 
            // lblHeaterProfile
            // 
            lblHeaterProfile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblHeaterProfile.AutoSize = true;
            lblHeaterProfile.ImageAlign = ContentAlignment.TopLeft;
            lblHeaterProfile.Location = new Point(12, 9);
            lblHeaterProfile.Name = "lblHeaterProfile";
            lblHeaterProfile.Size = new Size(79, 15);
            lblHeaterProfile.TabIndex = 9;
            lblHeaterProfile.Text = "Heater Profile";
            // 
            // SetHeaterProfileDialog
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(974, 561);
            Controls.Add(lblHeaterProfile);
            Controls.Add(gridHeaterProfile);
            Controls.Add(fldInformation);
            Controls.Add(btnTransfer);
            Controls.Add(btnLoadProfile);
            Controls.Add(lblPortNo);
            Controls.Add(fldPortNo);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SetHeaterProfileDialog";
            Text = "Heater Profile Setting";
            Load += DataDetailDialog_Load;
            ((System.ComponentModel.ISupportInitialize)gridHeaterProfile).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void DataDetailDialog_Load(object sender, EventArgs e)
        {
            gridHeaterProfile.DataSource = dataGrid.HeaterProfile;
            gridHeaterProfile.Columns[0].ReadOnly = true;

            // ヒータープロファイルをセンサ（デバイス）から読み込む
            reloadHeaterProfileFromDevice(fldPortNo.Text);
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
            float topLeftY = lblHeaterProfile.Location.Y + 20; //topMargin;
            float areaWidth = Size.Width - (margin * 4);
            float areaHeight = gridHeaterProfile.Location.Y - (topLeftY + topMargin); //(topMargin * 3);

            // Graphics オブジェクトを取得
            Graphics g = e.Graphics;

            // 背景領域の描画
            RectangleF drawArea = new RectangleF(topLeftX, topLeftY, areaWidth, areaHeight);
            graphDrawer.drawBackground(g, drawArea);

            // 軸の描画
            graphDrawer.drawAixs(g, drawArea);


        }

        public void setSelectedData(ref Dictionary<int, DataGridViewRow> selectedData, Dictionary<string, List<List<GraphDataValue>>> dataSet1, Dictionary<string, List<List<GraphDataValue>>> dataSet2, GraphDataValue lowerLimit, GraphDataValue upperLimit, GraphDataValue lowerLimitZoom, GraphDataValue upperLimitZoom)
        {
            // 描画クラスに描画するデータを送り込む
            graphDrawer.setDataToDraw(ref selectedData, dataSet1, dataSet2, lowerLimit, upperLimit, lowerLimitZoom, upperLimitZoom);

            maxIndexNumber = selectedData.Count;

            fldPortNo.Text = currentIndexNumber + "/" + maxIndexNumber;

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
            lblPortNo.Text = labelList[0];

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            currentIndexNumber--;
            if (currentIndexNumber <= 0)
            {
                currentIndexNumber = maxIndexNumber;
            }
            fldPortNo.Text = currentIndexNumber + "/" + maxIndexNumber;
            lblPortNo.Text = labelList[currentIndexNumber - 1];
            this.Invalidate();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            currentIndexNumber++;
            if (currentIndexNumber > maxIndexNumber)
            {
                currentIndexNumber = 1;
            }
            fldPortNo.Text = currentIndexNumber + "/" + maxIndexNumber;
            lblPortNo.Text = labelList[currentIndexNumber - 1];
            this.Invalidate();
        }

        private void selectGraphData()
        {
            this.Invalidate();
        }

        private void reloadHeaterProfileFromDevice(String portNoString)
        {
            try
            {
                Debug.WriteLine(DateTime.Now + " reloadHeaterProfileFromDevice(" + portNoString + ") : TRY");
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " reloadHeaterProfileFromDevice(" + portNoString + ")" + e.Message + "\r\n\r\n" + e.StackTrace);
            }
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

        private void btnLoadProfile_Click(object sender, EventArgs e)
        {
            // ヒータープロファイルをセンサ（デバイス）から読み込む
            reloadHeaterProfileFromDevice(fldPortNo.Text);
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            // ヒータープロファイルをセンサ（デバイス）へ書き込む


        }
    }
}
