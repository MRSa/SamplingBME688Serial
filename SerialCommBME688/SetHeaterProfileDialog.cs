
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace SamplingBME688Serial
{
    class SetHeaterProfileDialog : System.Windows.Forms.Form, IHeaterProfileNotify
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
        private Button btnAbort;
        private Label lblTotalCycleTime;
        private double totalCycleTime;
        private SetSerialHeaterProfile setSerial = new SetSerialHeaterProfile();

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
            setSerial.stopReadHeaterProfile();
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
            btnAbort = new Button();
            lblTotalCycleTime = new Label();
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
            btnLoadProfile.Size = new Size(79, 30);
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
            gridHeaterProfile.CellEndEdit += gridHeaterProfile_CellEndEdit;
            // 
            // lblHeaterProfile
            // 
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
            // btnAbort
            // 
            btnAbort.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAbort.Image = (Image)resources.GetObject("btnAbort.Image");
            btnAbort.Location = new Point(97, 519);
            btnAbort.Name = "btnAbort";
            btnAbort.Size = new Size(27, 30);
            btnAbort.TabIndex = 13;
            btnAbort.UseVisualStyleBackColor = true;
            btnAbort.Click += btnAbort_Click;
            // 
            // lblTotalCycleTime
            // 
            lblTotalCycleTime.AutoSize = true;
            lblTotalCycleTime.Location = new Point(97, 9);
            lblTotalCycleTime.Name = "lblTotalCycleTime";
            lblTotalCycleTime.Size = new Size(42, 15);
            lblTotalCycleTime.TabIndex = 14;
            lblTotalCycleTime.Text = "XXXXX";
            // 
            // SetHeaterProfileDialog
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(974, 561);
            Controls.Add(lblTotalCycleTime);
            Controls.Add(btnAbort);
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

            // ----- 画面上のボタンの有効・無効を制御
            btnTransfer.Enabled = false;
            btnLoad.Enabled = false;
            btnSave.Enabled = false;
            btnLoadProfile.Enabled = false;

            // ----- インフォメーションエリアをクリア
            fldInformation.Text = "";
            lblTotalCycleTime.Text = "";

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
            int totalTimeMs = dataGrid.HeaterProfile[1].Step0 + dataGrid.HeaterProfile[1].Step1 + dataGrid.HeaterProfile[1].Step2 +
                dataGrid.HeaterProfile[1].Step3 + dataGrid.HeaterProfile[1].Step4 + dataGrid.HeaterProfile[1].Step5 + dataGrid.HeaterProfile[1].Step6 +
                dataGrid.HeaterProfile[1].Step7 + dataGrid.HeaterProfile[1].Step8 + dataGrid.HeaterProfile[1].Step9;
            graphDrawer.drawAixs(g, drawArea, totalTimeMs);

            // グラフの描画
            graphDrawer.drawGraph(g, drawArea, dataGrid);
        }

        private void reloadHeaterProfileFromDevice(String portNoString)
        {
            try
            {
                btnTransfer.Enabled = false;
                btnLoad.Enabled = false;
                btnSave.Enabled = false;
                btnLoadProfile.Enabled = false;
                btnAbort.Enabled = true;
                Debug.WriteLine(DateTime.Now + " reloadHeaterProfileFromDevice(" + portNoString + ") : TRY");
                setSerial.getCurrentHeaterProfile(portNoString, this);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " reloadHeaterProfileFromDevice(" + portNoString + ")" + e.Message + "\r\n\r\n" + e.StackTrace);
            }
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
            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            string message = " Transfer a heater profile : " + fldProfileName.Text;
            DialogResult result = MessageBox.Show(message, "Information", buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (transferHeaterProfile(fldPortNo.Text))
                {
                    MessageBox.Show(
                        " Transferred a Heater Profile : " + dataGrid.HeaterProfileName,
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    reloadHeaterProfileFromDevice(fldPortNo.Text);
                }
                else
                {
                    MessageBox.Show(
                        "Failed to transfer a Heater Profile : " + dataGrid.HeaterProfileName,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private bool transferHeaterProfile(String portNoString)
        {
            try
            {
                // ==== データを転送する
                String sendData = getHeaterProfileString(dataGrid);
                fldInformation.Text += "\nSEND[" + portNoString + "] " + sendData;
                return (setSerial.transferHeaterProfile(portNoString, sendData));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " transferHeaterProfile() " + ex.Message + "\r\n\r\n" + ex.StackTrace);
            }
            return (false);
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
                cmdData += ",\"measDur\":" + data.MeasurementDuration;
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
                cmdData += dataGrid.HeaterProfile[1].Step0 / data.MeasurementDuration + ",";
                cmdData += dataGrid.HeaterProfile[1].Step1 / data.MeasurementDuration + ",";
                cmdData += dataGrid.HeaterProfile[1].Step2 / data.MeasurementDuration + ",";
                cmdData += dataGrid.HeaterProfile[1].Step3 / data.MeasurementDuration + ",";
                cmdData += dataGrid.HeaterProfile[1].Step4 / data.MeasurementDuration + ",";
                cmdData += dataGrid.HeaterProfile[1].Step5 / data.MeasurementDuration + ",";
                cmdData += dataGrid.HeaterProfile[1].Step6 / data.MeasurementDuration + ",";
                cmdData += dataGrid.HeaterProfile[1].Step7 / data.MeasurementDuration + ",";
                cmdData += dataGrid.HeaterProfile[1].Step8 / data.MeasurementDuration + ",";
                cmdData += dataGrid.HeaterProfile[1].Step9 / data.MeasurementDuration;
                cmdData += "]}";
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " getHeaterProfileString(" + data.HeaterProfileName + ")" + e.Message + "\r\n\r\n" + e.StackTrace);
            }
            return (cmdData);
        }

        private void applyDurationTime()
        {
            int totalTime = dataGrid.HeaterProfile[1].Step0 + dataGrid.HeaterProfile[1].Step1 + dataGrid.HeaterProfile[1].Step2 +
                dataGrid.HeaterProfile[1].Step3 + dataGrid.HeaterProfile[1].Step4 + dataGrid.HeaterProfile[1].Step5 + dataGrid.HeaterProfile[1].Step6 +
                dataGrid.HeaterProfile[1].Step7 + dataGrid.HeaterProfile[1].Step8 + dataGrid.HeaterProfile[1].Step9;

            totalCycleTime = Convert.ToDouble(totalTime);
            lblTotalCycleTime.Text = "(Duration : " + (totalCycleTime / 1000.0d).ToString("F2") + " sec.)";
        }

        private bool applyHeaterProfileFromJsonString(String data)
        {
            try
            {
                int isProfile = data.IndexOf("CMD:SETPROF");
                int startIndex = data.IndexOf("{");
                int endIndex = data.IndexOf("}");
                if ((isProfile < 0) || (startIndex <= 0) || (endIndex <= 0) || (endIndex <= startIndex) || (endIndex <= isProfile) || (startIndex <= isProfile))
                {
                    // ----- ヒータプロファイルデータではなかったのでエラー応答する
                    return (false);
                }

                String jsonString = data.Substring(startIndex, (endIndex - startIndex + 1));
                Debug.WriteLine(DateTime.Now + " Heater Profile : " + jsonString);
                HeaterProfileJson? loadHeaterProfile = JsonSerializer.Deserialize<HeaterProfileJson>(jsonString);
                if ((loadHeaterProfile == null) || (loadHeaterProfile?.tempProf.Count < 10) || (loadHeaterProfile?.holdProf.Count < 10))
                {
                    // ----- ヒータプロファイルデータの解析に失敗したので、エラー応答する
                    return (false);
                }

                // ----- 画面（GridView）にデータを反映させる
                //Debug.WriteLine(DateTime.Now + " load profile from File  " + loadHeaterProfile?.name + " [ " + loadHeaterProfile?.measDur + " ] " + (loadHeaterProfile?.holdProf.ElementAt(0) * loadHeaterProfile?.measDur));
                dataGrid.HeaterProfileName = loadHeaterProfile?.name ?? "";
                fldProfileName.Text = loadHeaterProfile?.name;
                dataGrid.MeasurementDuration = loadHeaterProfile?.measDur ?? 140;
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

                dataGrid.HeaterProfile[1].Step0 = (loadHeaterProfile?.holdProf.ElementAt(0) ?? 0) * (loadHeaterProfile?.measDur ?? 140);
                dataGrid.HeaterProfile[1].Step1 = (loadHeaterProfile?.holdProf.ElementAt(1) ?? 0) * (loadHeaterProfile?.measDur ?? 140);
                dataGrid.HeaterProfile[1].Step2 = (loadHeaterProfile?.holdProf.ElementAt(2) ?? 0) * (loadHeaterProfile?.measDur ?? 140);
                dataGrid.HeaterProfile[1].Step3 = (loadHeaterProfile?.holdProf.ElementAt(3) ?? 0) * (loadHeaterProfile?.measDur ?? 140);
                dataGrid.HeaterProfile[1].Step4 = (loadHeaterProfile?.holdProf.ElementAt(4) ?? 0) * (loadHeaterProfile?.measDur ?? 140);
                dataGrid.HeaterProfile[1].Step5 = (loadHeaterProfile?.holdProf.ElementAt(5) ?? 0) * (loadHeaterProfile?.measDur ?? 140);
                dataGrid.HeaterProfile[1].Step6 = (loadHeaterProfile?.holdProf.ElementAt(6) ?? 0) * (loadHeaterProfile?.measDur ?? 140);
                dataGrid.HeaterProfile[1].Step7 = (loadHeaterProfile?.holdProf.ElementAt(7) ?? 0) * (loadHeaterProfile?.measDur ?? 140);
                dataGrid.HeaterProfile[1].Step8 = (loadHeaterProfile?.holdProf.ElementAt(8) ?? 0) * (loadHeaterProfile?.measDur ?? 140);
                dataGrid.HeaterProfile[1].Step9 = (loadHeaterProfile?.holdProf.ElementAt(9) ?? 0) * (loadHeaterProfile?.measDur ?? 140);

                // ----- トータルの時間を更新
                applyDurationTime();

                // ----- 画面の更新
                gridHeaterProfile.Refresh();
                this.Invalidate();
                return (true);
            }
            catch (Exception ex)
            {
                // ----- 例外発生
                Debug.WriteLine(DateTime.Now + " applyHeaterProfileFromJsonString() " + data + " " + ex.Message + "\r\n\r\n" + ex.StackTrace);
            }
            return (false);
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
                if ((isProfile >= 0) && (applyHeaterProfileFromJsonString(fileContent.Substring(isProfile))))
                {
                    // ----- データを読み込み、画面に反映した
                    MessageBox.Show(
                        " Load Heater Profile : " + dataGrid.HeaterProfileName,
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    // ----- ヒータプロファイルデータではなかったのでエラー応答する
                    MessageBox.Show(
                        "Failed to load a Heater Profile\n\nThe specified file format is not supported as a heater profile.\n",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
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

        public void notifyHeaterProfile(bool isSuccess, string heaterProfile)
        {
            Debug.WriteLine(DateTime.Now + " notifyHeaterProfile() " + isSuccess + " " + heaterProfile);
            if (isSuccess)
            {
                String receivedProfile = "CMD:SETPROF " + heaterProfile;
                this.Invoke(new Action(() =>
                {
                    // --- 受信したヒータープロファイルを画面に反映させる
                    fldInformation.Text += heaterProfile;

                    btnTransfer.Enabled = true;
                    btnLoad.Enabled = true;
                    btnSave.Enabled = true;
                    btnLoadProfile.Enabled = true;
                    btnAbort.Enabled = true;

                    applyHeaterProfileFromJsonString(receivedProfile);

                }));
            }
            else
            {
                // ----- メッセージだけ画面に表示する。
                this.Invoke(new Action(() =>
                {
                    fldInformation.Text += heaterProfile;
                }));
            }
        }

        public void abortReadHeaterProfile(bool isClear = false)
        {
            Debug.WriteLine(DateTime.Now + " abortReadHeaterProfile)");
            if (isClear)
            {
                this.Invoke(new Action(() =>
                {
                    fldInformation.Text = "";
                    btnAbort.Enabled = true;
                    btnLoadProfile.Enabled = true;
                }));
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            setSerial.stopReadHeaterProfile();
        }

        private void gridHeaterProfile_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // データの編集が終わったとき...画面をリフレッシュ
                applyDurationTime();
                this.Invalidate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " gridHeaterProfile_CellEndEdit() " + ex.Message + "\r\n\r\n" + ex.StackTrace);

            }
        }
    }
}
