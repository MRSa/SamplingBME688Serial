using Microsoft.ML;
using SamplingBME688Serial;
using System.Diagnostics;
using System.Text;

namespace SerialCommBME688
{
    public partial class SerialCommForm : Form, IDataImportCallback, ICreateModelResult
    {
        private GridDataSourceProvider dataSourceProvider;
        private SerialReceiver myReceiver;
        private SerialReceiver myReceiver_2;
        private SerialReceiverForAnalysis analysisReceiver;
        private SerialReceiverForAnalysis analysisReceiver_2;
        private PredictAnalyzer predictAnalyzer;
        private MLContext mlContext;
        private DbEntryStatusView statusView;
        private SensorToUse predictModelType = SensorToUse.None;
        private IPredictionModel? predictionModel = null;

        public SerialCommForm()
        {
            dataSourceProvider = new GridDataSourceProvider();
            statusView = new DbEntryStatusView(this);
            predictAnalyzer = new PredictAnalyzer();
            myReceiver = new SerialReceiver(1, dataSourceProvider);
            myReceiver_2 = new SerialReceiver(2, dataSourceProvider);
            analysisReceiver = new SerialReceiverForAnalysis(1, predictAnalyzer);
            analysisReceiver_2 = new SerialReceiverForAnalysis(2, predictAnalyzer);
            mlContext = new MLContext(seed: 0);
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string category = txtDataCategory.Text;
                txtConsole.Text = "--- START RECEIVE [" + txtPort.Text + "] " + DateTime.Now + " (" + txtDataCategory.Text + ") ---\r\n";
                string sendUrl = "";
                if (chkEntryDatabase.Checked)
                {
                    sendUrl = urlDatabaseToEntry.Text + "sensor/entry";
                }

                if (myReceiver.startReceive(txtPort.Text, category, sendUrl, chkDbEntrySingle.Checked, txtConsole))
                {
                    // データの受信開始
                    controlButton1Enable(false);
                }
                else
                {
                    // データの受信開始に失敗
                    controlButton1Enable(true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (myReceiver.stopReceive())
                {
                    // データの受信完了
                    controlButton1Enable(true);
                    //txtConsole.AppendText("\r\n--- FINISH RECEIVE ---\r\n");
                }
                else
                {
                    // データの受信終了に失敗
                    controlButton1Enable(false);
                }
                predictModelType = SensorToUse.None;
                updateAnalysisMode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                txtConsole.AppendText(ex.Message);

                predictModelType = SensorToUse.None;
                updateAnalysisMode();
            }
        }

        private void controlButton1Enable(bool isEnable)
        {
            btnConnect.Enabled = isEnable;
            btnStop.Enabled = !isEnable;
            btnExport.Enabled = isEnable;
            btnImport.Enabled = isEnable;
            grpExportOption.Enabled = isEnable;
            chkExportOnlyGasRegistanceLogarithm.Enabled = isEnable;
            btnReset.Enabled = isEnable;
            btnDbStatus.Enabled = isEnable;
            chkEntryDatabase.Enabled = isEnable;
            chkDbEntrySingle.Enabled = isEnable;
            urlDatabaseToEntry.Enabled = isEnable;
            btnShowGraph.Enabled = isEnable;
            grpAnalysis.Enabled = isEnable;

            //btnCreateModel.Visible = isEnable;
            btnCreateModel.Enabled = isEnable;

            lblResult1.Enabled = isEnable;
            fldResult1.Enabled = isEnable;
            lblResult2.Enabled = isEnable;
            fldResult2.Enabled = isEnable;
            chkAnalysis.Enabled = isEnable;
        }

        private void controlButton2Enable(bool isEnable)
        {
            btnConnect_2.Enabled = isEnable;
            btnStop_2.Enabled = !isEnable;
            btnExport.Enabled = isEnable;
            btnImport.Enabled = isEnable;
            grpExportOption.Enabled = isEnable;
            chkExportOnlyGasRegistanceLogarithm.Enabled = isEnable;
            btnReset.Enabled = isEnable;
            btnDbStatus.Enabled = isEnable;
            chkEntryDatabase.Enabled = isEnable;
            chkDbEntrySingle.Enabled = isEnable;
            urlDatabaseToEntry.Enabled = isEnable;
            btnShowGraph.Enabled = isEnable;
            grpAnalysis.Enabled = isEnable;

            //btnCreateModel.Visible = isEnable;
            btnCreateModel.Enabled = isEnable;

            lblResult1.Enabled = isEnable;
            fldResult1.Enabled = isEnable;
            lblResult2.Enabled = isEnable;
            fldResult2.Enabled = isEnable;
            chkAnalysis.Enabled = isEnable;
        }

        private void changeStoreOrAnalysisMode(bool isEnable)
        {
            Debug.WriteLine(DateTime.Now + " changeStoreOrAnalysisMode : " + isEnable);

            grpDataCategory.Enabled = isEnable;
            grpPort.Enabled = isEnable;
            grpPort_2.Enabled = isEnable;
            grpEntryDatabase.Enabled = isEnable;

            grpAnalysis.Enabled = !isEnable;
            if (isEnable)
            {
                // 解析モードが無効になったときは、解析のチェックを必ず落とす
                chkAnalysis.Checked = false;
            }
        }

        private void updateAnalysisMode()
        {
            try
            {
                // ----- 解析モードと蓄積モードの切り替え
                if (predictModelType == SensorToUse.None)
                {
                    changeStoreOrAnalysisMode(true);
                }
                else
                {
                    changeStoreOrAnalysisMode(false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " dataImportProgress : " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // ログ表示のクリア
            txtConsole.Text = "";
            txtConsole_2.Text = "";
            predictModelType = SensorToUse.None;
            updateAnalysisMode();
        }

        private void SerialCommForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            myReceiver.stopReceive();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            int fromValue = decimal.ToInt32(importFromPercent.Value);
            int toValue = decimal.ToInt32(importToPercent.Value);


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    Debug.WriteLine("OpenFile : canWrite: " + myStream.CanWrite);

                    // CSVへデータを書き出す。
                    if (!chkExportOnlyGasRegistanceLogarithm.Checked)
                    {
                        // 全データのエクスポート
                        myReceiver.startExportAllDataToCsv(myStream, true, fromValue, toValue);
                        myReceiver_2.startExportAllDataToCsv(myStream, false, fromValue, toValue);
                    }
                    else
                    {
                        // センサの対数データのみのエクスポートする
                        if (chkCombineSensor.Checked)
                        {
                            // センサ１とセンサ２のデータを混ぜてCSV出力する
                            exportCsvEachSensorCombine(myStream);
                        }
                        else
                        {
                            // センサ１とセンサ２のデータを順番にCSV出力する
                            exportCsvEachSensorSerial(myStream);
                        }
                    }
                    myStream.Close();
                }
            }
        }

        private void exportCsvEachSensorSerial(Stream myStream)
        {
            try
            {
                int fromValue = decimal.ToInt32(importFromPercent.Value);
                int toValue = decimal.ToInt32(importToPercent.Value);

                StreamWriter writer = new StreamWriter(myStream, Encoding.UTF8);
                writer.AutoFlush = true;

                // ----- ヘッダ部分の出力 -----
                List<string> categoryList1 = myReceiver.getCollectedCategoryList();
                List<string> categoryList2 = myReceiver_2.getCollectedCategoryList();
                List<string> categories = new List<string>();
                writer.Write("; index, ");

                if (myReceiver.isDataReceived())
                {
                    foreach (string item in categoryList1)
                    {
                        try
                        {
                            if ((categoryList2.Contains(item)) || (!myReceiver_2.isDataReceived()))
                            {
                                // センサ１とセンサ２で同じカテゴリがあった場合だけ、ヘッダ部分に出力する
                                writer.Write(item + ", ");
                                categories.Add(item);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(DateTime.Now + " --- not found the key : " + item + " in sensor2... " + e.Message);
                        }
                    }
                }
                else
                {
                    // センサ１のデータがない場合は、センサ２のみ利用する。
                    foreach (string item in categoryList2)
                    {
                        writer.Write(item + ", ");
                        categories.Add(item);
                    }
                }
                writer.WriteLine(" ;");
                // ----- ヘッダ部分の出力おわり -----


                // センサ１とセンサ２のデータを順番に出力する（センサ１→センサ２の順に出力する）
                if (myReceiver.isDataReceived())
                {
                    myReceiver.startExportCsvOnlyGasRegistance(writer, categories, dataSourceProvider.getValidCount(), Decimal.ToInt32(numDuplicate.Value));
                }
                if (myReceiver_2.isDataReceived())
                {
                    myReceiver_2.startExportCsvOnlyGasRegistance(writer, categories, dataSourceProvider.getValidCount(), Decimal.ToInt32(numDuplicate.Value));
                }

                writer.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " exportCsvEachSensorSerial()" + ex.Message);
            }
        }

        private void exportCsvEachSensorCombine(Stream myStream)
        {
            try
            {
                int fromValue = decimal.ToInt32(importFromPercent.Value);
                int toValue = decimal.ToInt32(importToPercent.Value);
                CombinedSensorDataCsvExporter exporter = new CombinedSensorDataCsvExporter(myReceiver, myReceiver_2);
                exporter.startExportCsvCombine(myStream, dataSourceProvider.getValidCount(), Decimal.ToInt32(numDuplicate.Value));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " exportCsvEachSensorCombine()" + ex.Message);
            }
        }

        private void SerialCommForm_Load(object sender, EventArgs e)
        {
            // データグリッドに表示するカラムのヘッダーを設定する
            dataGridView1.DataSource = dataSourceProvider.getGridDataSource();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // データをリセットする
            myReceiver.reset();
            myReceiver_2.reset();
            dataSourceProvider.reset();
            txtDataCategory.Text = "";
            predictModelType = SensorToUse.None;
            this.predictionModel = null;
            updateAnalysisMode();
            try
            {
                // ----- ガベージコレクションの強制実行
                GC.Collect();
            }
            catch (Exception ex)
            {
                // ----- ガベコレで例外が発生したとき
                Debug.WriteLine(DateTime.Now + " btnReset_Click()" + ex.Message);
            }
        }

        private void btnShowGraph_Click(object sender, EventArgs e)
        {
            Dictionary<int, DataGridViewRow> selectedData = new Dictionary<int, DataGridViewRow>();

            GraphDataValue lowerLimit = new GraphDataValue(0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
            GraphDataValue upperLimit = new GraphDataValue(110000000.0f, 20.0f, double.MaxValue, double.MaxValue, double.MaxValue); ;

            //  データが１つ以上選択されていた時は、グラフを表示する
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    double lowerPressure    = Double.MaxValue;
                    double upperPressure    = Double.MinValue;
                    double lowerTemperature = Double.MaxValue;
                    double upperTemperature = Double.MinValue;
                    double lowerHumidity    = Double.MaxValue;
                    double upperHumidity    = Double.MinValue;

                    double lowerLimitZoomRLog = Double.MaxValue;
                    double upperLimitZoomRLog = Double.MinValue;
                    double lowerLimitZoomR = Double.MaxValue;
                    double upperLimitZoomR = Double.MinValue;
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        try
                        {
                            double minValueR = double.Parse(row.Cells[11].Value.ToString());
                            double minValueRLog = double.Parse(row.Cells[13].Value.ToString());
                            double maxValueR = double.Parse(row.Cells[10].Value.ToString());
                            double maxValueRLog = double.Parse(row.Cells[12].Value.ToString());

                            lowerLimitZoomRLog = (lowerLimitZoomRLog >= minValueRLog) ? minValueRLog : lowerLimitZoomRLog;
                            upperLimitZoomRLog = (upperLimitZoomRLog <= maxValueRLog) ? maxValueRLog : upperLimitZoomRLog;

                            lowerLimitZoomR = (lowerLimitZoomR >= minValueR) ? minValueR : lowerLimitZoomR;
                            upperLimitZoomR = (upperLimitZoomR <= maxValueR) ? maxValueR : upperLimitZoomR;
                        }
                        catch (Exception eex)
                        {
                            // --- 最大値・最小値のデータが取れなかった...
                            Debug.WriteLine(DateTime.Now + " ----- btnShowGraph_Click() : cannot get max/min value." + row.Cells[0].Value + " " + eex.Message);
                        }
                        Debug.WriteLine("[" + row.Index + "] : : " + row.Cells[0].Value + " ");
                        selectedData.Add(row.Index, row);
                    }
                    GraphDataValue lowerLimitZoom = new GraphDataValue(lowerLimitZoomR, lowerLimitZoomRLog, lowerPressure, lowerTemperature, lowerHumidity);
                    GraphDataValue upperLimitZoom = new GraphDataValue(upperLimitZoomR, upperLimitZoomRLog, upperPressure, upperTemperature, upperHumidity);
                    Debug.WriteLine(DateTime.Now + " ----- upperLimit(" + upperLimit + " " + upperLimitZoom + ") lowerLimit(" + lowerLimit + " " + lowerLimitZoom + ")");

                    // データが選択されていた時は、詳細ダイアログを表示する
                    DataDetailDialog dialog = new DataDetailDialog();
                    dialog.setSelectedData(ref selectedData, myReceiver.getGasRegDataSet(), myReceiver_2.getGasRegDataSet(), lowerLimit, upperLimit, lowerLimitZoom, upperLimitZoom);
                    dialog.Owner = this;
                    dialog.Show();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(DateTime.Now + " btnShowGraph_Click()" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show(
                    "Please select a row.",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void chkExportOnlyGasRegistanceLogarithm_CheckedChanged(object sender, EventArgs e)
        {

            if (chkExportOnlyGasRegistanceLogarithm.Checked)
            {
                numDuplicate.Enabled = true;
                chkCombineSensor.Enabled = true;
            }
            else
            {
                numDuplicate.Enabled = false;
                chkCombineSensor.Enabled = false;
            }
        }

        private void btnConnect_2_Click(object sender, EventArgs e)
        {
            try
            {
                string category = txtDataCategory.Text;
                string sendUrl = "";
                if (chkEntryDatabase.Checked)
                {
                    sendUrl = urlDatabaseToEntry.Text + "sensor/entry";
                }
                txtConsole_2.Text = "--- START RECEIVE [" + txtPort_2.Text + "] " + DateTime.Now + " (" + txtDataCategory.Text + ") ---\r\n";
                if (myReceiver_2.startReceive(txtPort_2.Text, category, sendUrl, chkDbEntrySingle.Checked, txtConsole_2))
                {
                    // データの受信開始
                    controlButton2Enable(false);
                }
                else
                {
                    // データの受信開始に失敗
                    controlButton2Enable(true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void btnStop_2_Click(object sender, EventArgs e)
        {
            try
            {
                if (myReceiver_2.stopReceive())
                {
                    // データの受信完了
                    controlButton2Enable(true);
                }
                else
                {
                    // データの受信終了に失敗
                    controlButton2Enable(false);
                }
                predictModelType = SensorToUse.None;
                updateAnalysisMode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                txtConsole.AppendText(ex.Message);
                predictModelType = SensorToUse.None;
                updateAnalysisMode();
            }
        }

        private void btnDbStatus_Click(object sender, EventArgs e)
        {
            // DBのステータスボタンを押したとき...センサのデータ登録情報を表示する
            try
            {
                statusView.showDbEntryStatus(urlDatabaseToEntry.Text + "sensor/list");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " btnDbStatus_Click() " + ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult choice = MessageBox.Show(
                    "This program is poorly structured and freezes while importing the data file. Please be aware of this notice.",
                    "Warning",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);
                if (choice != DialogResult.OK)
                {
                    // 処理を中止する
                    MessageBox.Show(
                        "Import canceled.",
                        "Canceled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);

                    return;
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(DateTime.Now + " btnImport_Click() : START " + ee.Message);
            }


            try
            {
                // CSVファイルを読み出す...
                Stream myStream;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog1.Filter = "CSV files (*.csv)|*.csv";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    string filePath = openFileDialog1.FileName;
                    myStream = openFileDialog1.OpenFile();

                    try
                    {
                        DataImporter importer = new DataImporter(myReceiver, myReceiver_2, this, openFileDialog1);
                        importer.importDataFromCsv();
                        /*
                        try
                        {
                            Thread thread = new Thread(new ThreadStart(importer.importDataFromCsv));
                            thread.IsBackground = true;
                            thread.Start();
                        }
                        catch (Exception ex1)
                        {
                            Debug.WriteLine(DateTime.Now + " btnImport_Click(): thread " + filePath + " " + ex1.Message);
                        }
                        */
                    }
                    catch (Exception ee)
                    {

                        Debug.WriteLine(DateTime.Now + " btnImport_Click(): Read " + filePath + " " + ee.Message);
                    }
                    myStream.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " btnImport_Click() " + ex.Message);
            }
        }

        public void dataImportFinished(bool isSuccess, string message)
        {
            myReceiver.stopReceive();
            myReceiver_2.stopReceive();
            Debug.WriteLine(DateTime.Now + " dataImportFinished : " + isSuccess + " " + message);
            if (isSuccess)
            {
                MessageBox.Show(
                    message,
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            predictModelType = SensorToUse.None;
            updateAnalysisMode();
        }

        public void dataImportProgress(int lineNumber, int totalLines)
        {
            string message = " Read " + lineNumber + " lines.";
            if (totalLines > 0)
            {
                double percentage = ((double)lineNumber / (double)totalLines) * 100.0d;
                message = message + $" ({percentage:F1} %)";
            }
            //Debug.WriteLine(DateTime.Now + " dataImportProgress : " + message);
        }

        private void btnCreateModel_Click(object sender, EventArgs e)
        {
            // ----- モデル作成ダイアログを表示して、モデルを作成する
            CreateModelDialog createModelDialog = new CreateModelDialog(ref mlContext);
            createModelDialog.setup(this, myReceiver.getDataHolder(), myReceiver_2.getDataHolder());
            createModelDialog.ShowDialog();
        }

        public void createModelFinished(bool isSuccess, SensorToUse modelType, IPredictionModel? predictionModel, string message)
        {
            Debug.WriteLine(DateTime.Now + "  ===== createModelFinished() " + isSuccess + " " + modelType + " " + message);
            if (isSuccess)
            {
                predictModelType = modelType;
                this.predictionModel = predictionModel;
            }
            else
            {
                predictModelType = SensorToUse.None;
                this.predictionModel = null;
            }
            updateAnalysisMode();
        }

        private void chkAnalysis_CheckedChanged(object sender, EventArgs e)
        {
            // ----- 解析の開始 / 終了が切り替えられたとき
            try
            {
                string message1 = "";
                string message2 = "";
                if (chkAnalysis.Checked)
                {
                    // ----- 解析の開始
                    message1 = "START";
                    switch (predictModelType)
                    {
                        case SensorToUse.port1and2:
                        case SensorToUse.port1or2:
                            analysisReceiver.startReceive(txtPort.Text, chkAnLog.Checked, txtConsole);
                            analysisReceiver_2.startReceive(txtPort_2.Text, chkAnLog.Checked, txtConsole_2);
                            message2 = "" + txtPort.Text + " " + txtPort_2.Text;
                            break;
                        case SensorToUse.port1:
                            analysisReceiver.startReceive(txtPort.Text, chkAnLog.Checked, txtConsole);
                            message2 = "Port1[" + txtPort.Text + "]";
                            break;
                        case SensorToUse.port2:
                            analysisReceiver_2.startReceive(txtPort_2.Text, chkAnLog.Checked, txtConsole_2);
                            message2 = "Port2[" + txtPort_2.Text + "]";
                            break;
                        case SensorToUse.None:
                        default:
                            message2 = "(Illegal)";
                            chkAnalysis.Checked = false;
                            break;
                    }
                    predictAnalyzer.startPredict(predictModelType, predictionModel, chkWithPresTempHumi.Checked, ref fldResult1, ref fldResult2);
                    chkWithPresTempHumi.Enabled = false;
                    chkAnLog.Enabled = false;
                }
                else
                {
                    // ----- 解析の終了
                    switch (predictModelType)
                    {
                        case SensorToUse.port1and2:
                        case SensorToUse.port1or2:
                            analysisReceiver.stopReceive();
                            analysisReceiver_2.stopReceive();
                            break;
                        case SensorToUse.port1:
                            analysisReceiver.stopReceive();
                            break;
                        case SensorToUse.port2:
                            analysisReceiver_2.stopReceive();
                            break;
                        case SensorToUse.None:
                        default:
                            chkAnalysis.Checked = false;
                            break;
                    }
                    predictAnalyzer.stopPredict();
                }
                fldResult1.Text = message1;
                fldResult2.Text = message2;
                chkWithPresTempHumi.Enabled = true;
                chkAnLog.Enabled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " chkAnalysis_CheckedChanged : " + ex.Message);
                fldResult1.Text = "Error";
                fldResult2.Text = ex.Message;
                chkAnalysis.Checked = false;
                chkWithPresTempHumi.Enabled = true;
                chkAnLog.Enabled = true;
                predictAnalyzer.stopPredict();
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            // ---------- Database から データをロードする処理
            try
            {

            }
            catch (Exception ex)
            {
                // ---- エラーが発生した...終了する
                Debug.WriteLine(DateTime.Now + "  btnLoadData_Click() " + ex.Message);
            }
        }
    }
}
