using Microsoft.ML;
using SamplingBME688Serial;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SerialCommBME688
{
    public partial class SerialCommForm : Form, IDataImportCallback, ICreateModelResult, IReceivedOdorDataForAnalysis
    {
        private bool isTestDebug = false;
        private GridDataSourceProvider dataSourceProvider;
        private SerialReceiver myReceiver;
        private SerialReceiver myReceiver_2;
        private SerialReceiverForAnalysis analysisReceiver;
        private SerialReceiverForAnalysis analysisReceiver_2;
        private MLContext mlContext;
        private DbEntryStatusView statusView;
        private SensorToUse predictModelType = SensorToUse.None; // 
        private IPredictionModel? predictionModel;

        private OdorOrData? analysisData01 = null;
        private OdorOrData? analysisData02 = null;

        public SerialCommForm()
        {
            dataSourceProvider = new GridDataSourceProvider();
            statusView = new DbEntryStatusView(this);
            myReceiver = new SerialReceiver(1, dataSourceProvider);
            myReceiver_2 = new SerialReceiver(2, dataSourceProvider);
            analysisReceiver = new SerialReceiverForAnalysis(1, this);
            analysisReceiver_2 = new SerialReceiverForAnalysis(2, this);
            mlContext = new MLContext(seed: 0);
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                String category = txtDataCategory.Text;
                txtConsole.Text = "--- START RECEIVE [" + txtPort.Text + "] " + DateTime.Now + " (" + txtDataCategory.Text + ") ---\r\n";
                String sendUrl = "";
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
                List<String> categoryList1 = myReceiver.getCollectedCategoryList();
                List<String> categoryList2 = myReceiver_2.getCollectedCategoryList();
                List<String> categories = new List<String>();
                writer.Write("; index, ");

                if (myReceiver.isDataReceived())
                {
                    foreach (String item in categoryList1)
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
                    foreach (String item in categoryList2)
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

            GraphDataValue lowerLimit = new GraphDataValue(0.0f, 0.0f);
            GraphDataValue upperLimit = new GraphDataValue(110000000.0f, 20.0f); ;

            //  データが１つ以上選択されていた時は、グラフを表示する
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
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
                    GraphDataValue lowerLimitZoom = new GraphDataValue(lowerLimitZoomR, lowerLimitZoomRLog);
                    GraphDataValue upperLimitZoom = new GraphDataValue(upperLimitZoomR, upperLimitZoomRLog);
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
                String category = txtDataCategory.Text;
                String sendUrl = "";
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

                    String filePath = openFileDialog1.FileName;
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

        public void dataImportFinished(bool isSuccess, String message)
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
            String message = " Read " + lineNumber + " lines.";
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

        public void createModelFinished(bool isSuccess, SensorToUse modelType, IPredictionModel? predictionModel, String message)
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

        private void executePredictionTest()
        {
            // --------- テスト用データで予測実行
            fldResult1.Text = "DEBUG TEST";
            String message = "";
            TestSampleData testData = new TestSampleData();
            if (predictionModel != null)
            {
                // ----- 予測の実行
                switch (predictModelType)
                {
                    case SensorToUse.port1and2:
                        String result1 = predictionModel.predictBothData(testData.getBothData1()); // Ristretto
                        String result2 = predictionModel.predictBothData(testData.getBothData2()); // GreenTea
                        String result3 = predictionModel.predictBothData(testData.getBothData3()); // Ristretto
                        String result4 = predictionModel.predictBothData(testData.getBothData4()); // Guatemala
                        message = result1 + " " + result2 + " " + result3 + " " + result4;
                        break;
                    case SensorToUse.port1or2:
                        break;
                    case SensorToUse.port1:
                        break;
                    case SensorToUse.port2:
                        break;
                    case SensorToUse.None:
                    default:
                        break;
                }
            }
            // ----- 予測結果の表示
            fldResult2.Text = message;
        }

        private void chkAnalysis_CheckedChanged(object sender, EventArgs e)
        {
            // ----- 解析の開始 / 終了が切り替えられたとき
            try
            {
                String message1 = "";
                String message2 = "";
                if (chkAnalysis.Checked)
                {
                    if (isTestDebug)
                    {
                        // ----------- テストの実行処理
                        executePredictionTest();
                        chkAnalysis.Checked = false;
                        return;
                    }

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
                }
                fldResult1.Text = message1;
                fldResult2.Text = message2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " chkAnalysis_CheckedChanged : " + ex.Message);
                fldResult1.Text = "Error";
                fldResult2.Text = ex.Message;
                chkAnalysis.Checked = false;
            }
        }

        private void showResult(int area, String itemData)
        {
            System.Windows.Forms.TextBox field = (area == 1) ? fldResult1 : fldResult2;
            try
            {
                if ((field != null) && (itemData.Length > 0))
                {
                    if (field.InvokeRequired)
                    {
                        Action safeWrite = delegate { field.Clear(); field.AppendText(itemData); };
                        field.Invoke(safeWrite);
                    }
                    else
                    {
                        field.Clear();
                        field.AppendText(itemData);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " showResult() : " + e.Message + "  --- " + itemData);
            }
        }


        public void receivedOdorDataForAnalysis(OdorOrData receivedData)
        {
            // ------ データを受信した。モデルタイプに合わせて予測処理を実行する
            Debug.WriteLine(DateTime.Now + "  receivedOdorDataForAnalysis() RX[" + receivedData.sensorId + "]");
            try
            {
                if (predictionModel == null)
                {
                    // ---- モデルがない...終了する
                    showResult(1, "-- no model --");
                    showResult(2, "-- no model --");
                    return;
                }

                switch (predictModelType)
                {
                    case SensorToUse.port1and2:
                        // ----- センサデータ２つを使う予測処理
                        receivedOdor1and2DataForAnalysis(receivedData);
                        break;
                    case SensorToUse.port1or2:
                        // ----- センサデータを1 or 2を使用する予測処理
                        receivedOdor1or2DataForAnalysis(receivedData);
                        break;
                    case SensorToUse.port1:
                    case SensorToUse.port2:
                        // ----- センサデータ１つしか使用しない予測処理
                        receivedOdorSingleDataForAnalysis(receivedData);
                        break;
                    case SensorToUse.None:
                    default:
                        // ---------- モデルタイプが異常なので、予測処理は実行しない
                        showResult(1, " unknown ");
                        showResult(2, " unknown ");
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " showResult() : " + ex.Message + "  --- " + receivedData.sensorId);
            }
        }

        private void receivedOdor1and2DataForAnalysis(OdorOrData receivedData)
        {
            // ----- sensor1 と sensor2 のデータを同時に使用して解析する
            Debug.WriteLine(DateTime.Now + "  receivedOdorDataForAnalysis() RX[" + receivedData.sensorId + "]");

            if (receivedData.sensorId == 1.0f)
            {
                // ----- センサ１のデータを受信
                analysisData01 = new OdorOrData(receivedData);
                Debug.WriteLine(DateTime.Now + "  receivedOdorDataForAnalysis() sensorId 1");
                showResult(1, "");
            }
            else if (receivedData.sensorId == 2.0f)
            {
                // ----- センサ２のデータを受信
                analysisData02 = new OdorOrData(receivedData);
                Debug.WriteLine(DateTime.Now + "  receivedOdorDataForAnalysis() sensorId 2");
                showResult(1, "");
            }

            // -----ひとそろえのデータを受信した！ 解析する
            if ((analysisData01 != null) && (analysisData02 != null) && ((predictionModel != null)))
            {
                // ----- 解析を行う
                Debug.WriteLine(DateTime.Now + "  receivedOdorDataForAnalysis() analysis Both");
                OdorBothData testData = new OdorBothData(analysisData01, analysisData02);
                String result = predictionModel.predictBothData(testData);
                Debug.WriteLine(DateTime.Now + "  receivedOdorDataForAnalysis() Result: " + result);

                // ----- 解析結果を表示する
                showResult(1, "---");
                showResult(1, result);

                // ---- 次のデータを待つために表示をクリアする
                analysisData01 = null;
                analysisData02 = null;
            }
        }

        private void receivedOdor1or2DataForAnalysis(OdorOrData receivedData)
        {
            // ----- 受信したデータを使って予測を実行する
            Debug.WriteLine(DateTime.Now + "  receivedOdor1or2DataForAnalysis() RX[" + receivedData.sensorId + "]");
            int id = 1;
            try
            {
                if (receivedData.sensorId == 1.0f)
                {
                    // ----- センサ１のデータを受信
                    id = 1;
                    showResult(id, "");
                }
                else if (receivedData.sensorId == 2.0f)
                {
                    // ----- センサ２のデータを受信
                    id = 2;
                    showResult(id, "");
                }
                else
                {
                    // ----- センサのIDが異常な場合...
                    showResult(1, "");
                    showResult(2, "");
                    Debug.WriteLine(DateTime.Now + "  receivedOdor1or2DataForAnalysis() ID: " + receivedData.sensorId + " (Wrong ID)");
                }
                if ((predictionModel != null) && (receivedData != null))
                {
                    // ----- 解析(データの予測)を行う
                    Debug.WriteLine(DateTime.Now + "  receivedOdor1or2DataForAnalysis() START");
                    String result = predictionModel.predictOrData(receivedData);
                    Debug.WriteLine(DateTime.Now + "  receivedOdor1or2DataForAnalysis() Result: " + result);
                    showResult(id, result);
                }
            }
            catch (Exception ex)
            {
                // ---- 解析時にエラーが発生した...終了する
                Debug.WriteLine(DateTime.Now + "  receivedOdor1or2DataForAnalysis() " + ex.Message);
                showResult(id, "[ERROR]");
            }
        }

        private void receivedOdorSingleDataForAnalysis(OdorOrData receivedData)
        {
            // ----- 受信したデータを使って予測を実行する
            Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() RX[" + receivedData.sensorId + "]");
            int id = 1;
            try
            {
                OdorData? odorData = null;
                if (receivedData.sensorId == 1.0f)
                {
                    // ----- センサ１のデータを受信
                    odorData = new OdorData(receivedData);
                    Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() [1]");
                    id = 1;
                    showResult(id, "");
                }
                else if (receivedData.sensorId == 2.0f)
                {
                    // ----- センサ２のデータを受信
                    odorData = new OdorData(receivedData);
                    Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() [2]");
                    id = 2;
                    showResult(id, "");
                }
                else
                {
                    // ----- センサのIDが異常な場合...
                    showResult(1, "");
                    showResult(2, "");
                    Debug.WriteLine(DateTime.Now + "  receivedOdor1or2DataForAnalysis() ID: " + receivedData.sensorId + " (Wrong ID)");
                }
                if ((predictionModel != null)&&(odorData != null))
                {
                    // ----- 解析(データの予測)を行う
                    Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() START");
                    String result = predictionModel.predictSingleData(odorData);
                    Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() Result: " + result);
                    showResult(id, result);
                }
            }
            catch (Exception ex)
            {
                // ---- 解析時にエラーが発生した...終了する
                Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() " + ex.Message);
                showResult(id, "[ERROR]");
            }
        }
    }
}
