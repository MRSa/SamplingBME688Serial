
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
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
        private TextBox fldProfileName;
        private Button btnLoad;
        private Button btnSave;
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
            fldProfileName = new TextBox();
            btnLoad = new Button();
            btnSave = new Button();
            ((System.ComponentModel.ISupportInitialize)gridHeaterProfile).BeginInit();
            SuspendLayout();
            // 
            // fldPortNo
            // 
            fldPortNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            fldPortNo.Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            fldPortNo.Location = new Point(12, 461);
            fldPortNo.Name = "fldPortNo";
            fldPortNo.ReadOnly = true;
            fldPortNo.Size = new Size(112, 23);
            fldPortNo.TabIndex = 2;
            fldPortNo.TextAlign = HorizontalAlignment.Center;
            // 
            // lblPortNo
            // 
            lblPortNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblPortNo.AutoSize = true;
            lblPortNo.ImageAlign = ContentAlignment.TopLeft;
            lblPortNo.Location = new Point(12, 443);
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
            btnLoadProfile.Size = new Size(112, 30);
            btnLoadProfile.TabIndex = 5;
            btnLoadProfile.Text = "   Reload";
            btnLoadProfile.UseVisualStyleBackColor = true;
            btnLoadProfile.Click += btnLoadProfile_Click;
            // 
            // btnTransfer
            // 
            btnTransfer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnTransfer.Enabled = false;
            btnTransfer.Image = (Image)resources.GetObject("btnTransfer.Image");
            btnTransfer.ImageAlign = ContentAlignment.MiddleLeft;
            btnTransfer.Location = new Point(12, 363);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new Size(112, 30);
            btnTransfer.TabIndex = 6;
            btnTransfer.Text = "   Transfer";
            btnTransfer.UseVisualStyleBackColor = true;
            btnTransfer.Click += btnTransfer_Click;
            // 
            // fldInformation
            // 
            fldInformation.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            fldInformation.Location = new Point(130, 449);
            fldInformation.Multiline = true;
            fldInformation.Name = "fldInformation";
            fldInformation.ReadOnly = true;
            fldInformation.ScrollBars = ScrollBars.Both;
            fldInformation.Size = new Size(832, 100);
            fldInformation.TabIndex = 7;
            // 
            // gridHeaterProfile
            // 
            gridHeaterProfile.AllowUserToAddRows = false;
            gridHeaterProfile.AllowUserToDeleteRows = false;
            gridHeaterProfile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridHeaterProfile.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            gridHeaterProfile.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridHeaterProfile.Location = new Point(130, 363);
            gridHeaterProfile.Name = "gridHeaterProfile";
            gridHeaterProfile.Size = new Size(832, 80);
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
            // fldProfileName
            // 
            fldProfileName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            fldProfileName.Location = new Point(12, 490);
            fldProfileName.Name = "fldProfileName";
            fldProfileName.Size = new Size(112, 23);
            fldProfileName.TabIndex = 10;
            fldProfileName.TextChanged += fldProfileName_TextChanged;
            // 
            // btnLoad
            // 
            btnLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLoad.Location = new Point(12, 417);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(53, 23);
            btnLoad.TabIndex = 11;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSave.Location = new Point(71, 417);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(53, 23);
            btnSave.TabIndex = 12;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // SetHeaterProfileDialog
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(974, 561);
            Controls.Add(btnSave);
            Controls.Add(btnLoad);
            Controls.Add(fldProfileName);
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

            // ヒータープロファイル名を反映させる
            fldProfileName.Text = dataGrid.HeaterProfileName;

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

        private void fldProfileName_TextChanged(object sender, EventArgs e)
        {
            // ----- ヒータープロファイル名称を反映
            dataGrid.HeaterProfileName = fldProfileName.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // ----- ヒータープロファイルをファイルに保存する
            String heaterProfileString = getHeaterProfileString(dataGrid);

            Debug.WriteLine(DateTime.Now + " Heater Profile : " + heaterProfileString);

            Stream myStream;
            SaveFileDialog saveJsonFileDialog = new SaveFileDialog();

            saveJsonFileDialog.FileName = dataGrid.HeaterProfileName + ".txt";
            saveJsonFileDialog.Filter = "Text files (*.txt)|*.txt";
            saveJsonFileDialog.FilterIndex = 2;
            saveJsonFileDialog.RestoreDirectory = true;

            if (saveJsonFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveJsonFileDialog.OpenFile()) != null)
                    {
                        Debug.WriteLine("OpenFile : canWrite: " + myStream.CanWrite);
                        StreamWriter writer = new StreamWriter(myStream, Encoding.UTF8);
                        writer.AutoFlush = true;
                        writer.WriteLine(heaterProfileString);
                        writer.Close();
                        myStream.Close();
                    }
                    MessageBox.Show(
                        " Saved Heater Profile (" + dataGrid.HeaterProfileName + "): " + saveJsonFileDialog.FileName,
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(DateTime.Now + " btnSave_Click(" + ")" + ex.Message + "\r\n\r\n" + ex.StackTrace);

                    MessageBox.Show(
                        "Failed to save a Heater Profile : " + dataGrid.HeaterProfileName,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private String getHeaterProfileString(HeaterProfileDataGrid data)
        {
            String cmdData = "";
            try
            {
                cmdData += "CMD:SETPROF {";
                cmdData += "\"name\":\"" + data.HeaterProfileName + "\"";
                cmdData += ",\"tempProf\":[";
                cmdData += dataGrid.HeaterProfile[0].Step0 + ",";
                cmdData += dataGrid.HeaterProfile[0].Step1 + ",";
                cmdData += dataGrid.HeaterProfile[0].Step2 + ",";
                cmdData += dataGrid.HeaterProfile[0].Step3 + ",";
                cmdData += dataGrid.HeaterProfile[0].Step4 + ",";
                cmdData += dataGrid.HeaterProfile[0].Step5 + ",";
                cmdData += dataGrid.HeaterProfile[0].Step6 + ",";
                cmdData += dataGrid.HeaterProfile[0].Step7 + ",";
                cmdData += dataGrid.HeaterProfile[0].Step8 + ",";
                cmdData += dataGrid.HeaterProfile[0].Step9;
                cmdData += "],\"holdProf\":[";
                cmdData += dataGrid.HeaterProfile[1].Step0 + ",";
                cmdData += dataGrid.HeaterProfile[1].Step1 + ",";
                cmdData += dataGrid.HeaterProfile[1].Step2 + ",";
                cmdData += dataGrid.HeaterProfile[1].Step3 + ",";
                cmdData += dataGrid.HeaterProfile[1].Step4 + ",";
                cmdData += dataGrid.HeaterProfile[1].Step5 + ",";
                cmdData += dataGrid.HeaterProfile[1].Step6 + ",";
                cmdData += dataGrid.HeaterProfile[1].Step7 + ",";
                cmdData += dataGrid.HeaterProfile[1].Step8 + ",";
                cmdData += dataGrid.HeaterProfile[1].Step9;
                cmdData += "]}";
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " getHeaterProfileString(" + data.HeaterProfileName + ")" + e.Message + "\r\n\r\n" + e.StackTrace);
            }
            return (cmdData);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            // ----- ヒータープロファイルをファイルから読み込む
            try
            {
                String fileContent = "";
                var filePath = string.Empty;
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePath = openFileDialog.FileName;

                        //Read the contents of the file into a stream
                        var fileStream = openFileDialog.OpenFile();

                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            fileContent = reader.ReadToEnd();
                        }
                    }
                }
                int isProfile = fileContent.IndexOf("CMD:SETPROF");
                int startIndex = fileContent.IndexOf("{");
                int endIndex = fileContent.IndexOf("}");
                if ((isProfile < 0)||(startIndex <= 0)||(endIndex <= 0)||(endIndex <= startIndex)||(endIndex <= isProfile) || (startIndex <= isProfile))
                {
                    // ----- ヒータプロファイルデータではなかったのでエラー応答する
                    MessageBox.Show(
                        "Failed to load a Heater Profile\n\nThe specified file format is not supported as a heater profile.\n", 
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                String jsonString = fileContent.Substring(startIndex, (endIndex - startIndex + 1));
                Debug.WriteLine(DateTime.Now + " Heater Profile : " + jsonString);
                HeaterProfileJson? loadHeaterProfile = JsonSerializer.Deserialize<HeaterProfileJson>(jsonString);
                if ((loadHeaterProfile == null)||(loadHeaterProfile?.tempProf.Count < 10) || (loadHeaterProfile?.holdProf.Count < 10))
                {
                    // ----- ヒータプロファイルデータの解析に失敗したので、エラー応答する
                    MessageBox.Show(
                        "Failed to load a Heater Profile\n\nThe specified file format is not supported as a heater profile.\n",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }
                dataGrid.HeaterProfileName = loadHeaterProfile?.name ?? "";
                fldProfileName.Text = loadHeaterProfile?.name;
                dataGrid.HeaterProfile[0].Step0 = loadHeaterProfile?.tempProf.ElementAt(0) ?? 0;
                dataGrid.HeaterProfile[0].Step1 = loadHeaterProfile?.tempProf.ElementAt(1) ?? 0;
                dataGrid.HeaterProfile[0].Step2 = loadHeaterProfile?.tempProf.ElementAt(2) ?? 0;
                dataGrid.HeaterProfile[0].Step3 = loadHeaterProfile?.tempProf.ElementAt(3) ?? 0;
                dataGrid.HeaterProfile[0].Step4 = loadHeaterProfile?.tempProf.ElementAt(4) ?? 0;
                dataGrid.HeaterProfile[0].Step5 = loadHeaterProfile?.tempProf.ElementAt(5) ?? 0;
                dataGrid.HeaterProfile[0].Step6 = loadHeaterProfile?.tempProf.ElementAt(6) ?? 0;
                dataGrid.HeaterProfile[0].Step7 = loadHeaterProfile?.tempProf.ElementAt(7) ?? 0;
                dataGrid.HeaterProfile[0].Step8 = loadHeaterProfile?.tempProf.ElementAt(8) ?? 0;
                dataGrid.HeaterProfile[0].Step9 = loadHeaterProfile?.tempProf.ElementAt(9) ?? 0;

                dataGrid.HeaterProfile[1].Step0 = loadHeaterProfile?.holdProf.ElementAt(0) ?? 0;
                dataGrid.HeaterProfile[1].Step1 = loadHeaterProfile?.holdProf.ElementAt(1) ?? 0;
                dataGrid.HeaterProfile[1].Step2 = loadHeaterProfile?.holdProf.ElementAt(2) ?? 0;
                dataGrid.HeaterProfile[1].Step3 = loadHeaterProfile?.holdProf.ElementAt(3) ?? 0;
                dataGrid.HeaterProfile[1].Step4 = loadHeaterProfile?.holdProf.ElementAt(4) ?? 0;
                dataGrid.HeaterProfile[1].Step5 = loadHeaterProfile?.holdProf.ElementAt(5) ?? 0;
                dataGrid.HeaterProfile[1].Step6 = loadHeaterProfile?.holdProf.ElementAt(6) ?? 0;
                dataGrid.HeaterProfile[1].Step7 = loadHeaterProfile?.holdProf.ElementAt(7) ?? 0;
                dataGrid.HeaterProfile[1].Step8 = loadHeaterProfile?.holdProf.ElementAt(8) ?? 0;
                dataGrid.HeaterProfile[1].Step9 = loadHeaterProfile?.holdProf.ElementAt(9) ?? 0;

                gridHeaterProfile.Refresh();

                MessageBox.Show(
                    " Load Heater Profile : " + dataGrid.HeaterProfileName,
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " load profile from File  " + ex.Message + "\r\n\r\n" + ex.StackTrace);
                MessageBox.Show(
                    "Failed to load a Heater Profile : " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
