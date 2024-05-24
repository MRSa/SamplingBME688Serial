using Microsoft.ML;
using System.Diagnostics;
//using System.Windows.Forms;

namespace SamplingBME688Serial
{

    interface ICreateModelConsole
    {
        void appendText(string itemData);
    }

    public partial class CreateModelDialog : Form, ICreateModelConsole
    {
        // ----- forDebugTest を true にすると、モデルを作成したときにテストデータで予測する
        private bool forDebugTest = false;

        private string _sourceDataFile = Path.Combine(System.IO.Path.GetTempPath(), "modelsrc.csv");
        private MLContext mlContext;
        private ICreateModelResult? callback = null;
        private IPredictionModel? trainedModel = null;
        private IDataHolder? port1 = null;
        private IDataHolder? port2 = null;
        private List<string> categoryList = new List<string>();
        private int minimumDataCount = int.MaxValue;
        private int outputDataCount = 0;
        private int startPosition = 0;
        private int fromValue = 0;
        private int toValue = 100;
        private int port1CategoryCount = 0;
        private int port2CategoryCount = 0;
        private int categoryCount = 0;

        public CreateModelDialog(ref MLContext mlContext, bool forDebug = false)
        {
            this.mlContext = mlContext;
            this.forDebugTest = forDebug;

            InitializeComponent();

            // ----- 
            cmbModel.Items.Clear();
            cmbModel.Items.Add("LightGbm");              // index: 0
            cmbModel.Items.Add("LbfgsMaximumEntropy");   // index: 1
            cmbModel.Items.Add("SdcaMaximumEntropy");    // index: 2
            cmbModel.Items.Add("SdcaNonCalibrated");     // index: 3
            cmbModel.Items.Add("Naive Bayes");           // index: 4
            cmbModel.Items.Add("PairwiseCoupling");      // index: 5
            cmbModel.Items.Add("OneVersusAll");          // index: 6
            cmbModel.Items.Add("K-Means");               // index: 7
            cmbModel.SelectedIndex = 0;

            // ----- 
            cmbBinaryModel.Enabled = false;
            cmbBinaryModel.Visible = false;
            cmbBinaryModel.Items.Clear();
            cmbBinaryModel.Items.Add("---");
            cmbBinaryModel.SelectedIndex = 0;

            // ----- 
            selDuplicate.Items.Clear();
            selDuplicate.Items.Add("x1");
            selDuplicate.Items.Add("x5");
            selDuplicate.Items.Add("x10");
            selDuplicate.Items.Add("x20");
            selDuplicate.Items.Add("x30");
            selDuplicate.Items.Add("x50");
            selDuplicate.Items.Add("x100");
            selDuplicate.Items.Add("x1000");
            selDuplicate.SelectedIndex = 0;

            // ----- モデル保存ボタンを無効化する
            btnSaveModel.Enabled = false;
            btnSaveModel.Visible = false;
        }

        public void setup(ICreateModelResult callback, IDataHolder port1, IDataHolder port2)
        {
            // --- ダイアログデータのセットアップ
            this.callback = callback;
            this.port1 = port1;
            this.port2 = port2;
            this.categoryList.Clear();

            // ----- 一時データファイルの削除
            deleteDataSourceFile(_sourceDataFile);

            // ----- データの最小値を取得する 
            checkMinimumDataCounts();

            // ----- 出力時のパラメータを取得する
            prepareExportParameter();
        }

        private void checkMinimumDataCounts()
        {
            try
            {
                appendText("\r\n");
                if (port1 != null)
                {
                    appendText("Port1: ");
                    Dictionary<string, List<List<GraphDataValue>>> dataSetMap = port1.getGasRegDataSet();
                    foreach (KeyValuePair<string, List<List<GraphDataValue>>> item in dataSetMap)
                    {
                        int count = item.Value.Count;
                        if (count < minimumDataCount)
                        {
                            minimumDataCount = count;
                        }
                        appendText(item.Key + " ");
                        categoryList.Add(item.Key);
                    }
                    port1CategoryCount = dataSetMap.Count;
                }
                appendText("\r\n");
                if (port2 != null)
                {
                    appendText("Port2: ");
                    Dictionary<string, List<List<GraphDataValue>>> dataSetMap = port2.getGasRegDataSet();
                    foreach (KeyValuePair<string, List<List<GraphDataValue>>> item in dataSetMap)
                    {
                        int count = item.Value.Count;
                        if (count < minimumDataCount)
                        {
                            minimumDataCount = count;
                        }
                        appendText(item.Key + " ");
                    }
                    port2CategoryCount = dataSetMap.Count;
                }
                appendText("\r\n minimum count: " + minimumDataCount + " category count: " + port1CategoryCount + " " + port2CategoryCount + "\r\n");
                if (port1CategoryCount > 0)
                {
                    categoryCount = port1CategoryCount;
                    if ((port2CategoryCount > 0) && (port2CategoryCount < port1CategoryCount))
                    {
                        categoryCount = port2CategoryCount;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " checkDataCounts : " + ex.Message);
            }
        }

        public void appendText(string itemData)
        {
            try
            {
                if ((txtMessage != null) && (itemData.Length > 0))
                {
                    if (txtMessage.InvokeRequired)
                    {
                        Action safeWrite = delegate { appendText(itemData); };
                        txtMessage.Invoke(safeWrite);
                    }
                    else
                    {
                        txtMessage.AppendText(itemData);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " appendText() : " + e.Message + "  --- " + itemData);
            }
        }

        private void deleteDataSourceFile(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    // ----- ファイルが存在しない場合は、ファイルの削除は行わない
                    return;
                }

                FileInfo file = new FileInfo(fileName);
                if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    file.Attributes = FileAttributes.Normal;
                }
                file.Delete();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " deleteDataSourceFile : " + fileName + "  " + ex.Message);
            }
        }

        private void prepareExportParameter()
        {
            try
            {
                // ===== 区間データ
                fromValue = decimal.ToInt32(rangeFromPercent.Value);
                toValue = decimal.ToInt32(rangeToPercent.Value);
                int range = Math.Abs(fromValue - toValue);
                if ((range <= 0) || (range >= 100))
                {
                    // ===== 指定が適切でない場合は 100% にする
                    range = 100;
                }
                outputDataCount = minimumDataCount * range / 100 - 1;
                if (outputDataCount <= 0)
                {
                    // ===== データカウントが異常な場合は補正する
                    outputDataCount = minimumDataCount;
                }

                startPosition = minimumDataCount * fromValue / 100;

                if ((startPosition + outputDataCount) > minimumDataCount)
                {
                    // ===== 開始位置と終了位置の計算がずれていた場合は、先頭から出力する
                    startPosition = 0;
                    Debug.WriteLine(DateTime.Now + " [WARN] output data to start. (0, " + outputDataCount + ")");
                }
                // ----- 出力区間を表示する
                txtDataCount.Text = startPosition + " - " + (startPosition + outputDataCount) + " (" + outputDataCount + ")";
            }
            catch (Exception ee)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] prepareExportParameter() " + ee.Message);
            }
        }

        private void btnCreateModel_Click(object sender, EventArgs e)
        {
            string message = "Create Model, READY?";

            // ----- モデルの作成を開始するか、確認する
            DialogResult result = MessageBox.Show(message, "Create Model", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if ((result == DialogResult.No) || (result == DialogResult.Cancel))
            {
                // 何も実行せずに終了する
                Debug.WriteLine(DateTime.Now + " Cancelled create model. ");
                return;
            }

            // ----- モデル作成に入る
            Debug.WriteLine(DateTime.Now + " :::: CREATE MODEL : " + _sourceDataFile + " :::::");

            // ----- データ増殖
            int duplicateTimes = 1;
            switch (selDuplicate.SelectedIndex)
            {
                case 1:
                    duplicateTimes = 5;
                    break;
                case 2:
                    duplicateTimes = 10;
                    break;
                case 3:
                    duplicateTimes = 20;
                    break;
                case 4:
                    duplicateTimes = 30;
                    break;
                case 5:
                    duplicateTimes = 50;
                    break;
                case 6:
                    duplicateTimes = 100;
                    break;
                case 7:
                    duplicateTimes = 1000;
                    break;
                case 0:
                default:
                    // duplicateTimes = 1;
                    break;
            }

            // ===== CSVファイルへの出力 ... チェックボックスの選択によって、出力内容を変更する
            bool ret = false;
            SensorToUse usePort;
            TrainCsvDataExporter csvExporter = new TrainCsvDataExporter(_sourceDataFile, port1, port2, this);
            if (selSensor1and2.Checked)
            {
                csvExporter.outputDataSourceCSVFile1and2(startPosition, outputDataCount, duplicateTimes, chkDataLog.Checked, chkPresTempHumidity.Checked);
                usePort = SensorToUse.port1and2;
            }
            else if (selSensor1.Checked)
            {
                csvExporter.outputDataSourceCSVFileSingle(1, startPosition, outputDataCount, duplicateTimes, chkDataLog.Checked, chkPresTempHumidity.Checked);
                usePort = SensorToUse.port1;
            }
            else if (selSensor2.Checked)
            {
                csvExporter.outputDataSourceCSVFileSingle(2, startPosition, outputDataCount, duplicateTimes, chkDataLog.Checked, chkPresTempHumidity.Checked);
                usePort = SensorToUse.port2;
            }
            else // if (selSensor1or2.Checked)
            {
                csvExporter.outputDataSourceCSVFile1or2(startPosition, outputDataCount, duplicateTimes, chkDataLog.Checked, chkPresTempHumidity.Checked);
                usePort = SensorToUse.port1or2;
            }

            // ----- モデルの作成
            switch (cmbModel.SelectedIndex)
            {
                case 1:
                    // L-BFGS
                    IEstimator<ITransformer> estimator1 = mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy("Label", "Features");
                    TrainingMultiClassification training1 = new TrainingMultiClassification(ref mlContext, "LbfgsMaximumEntropy", ref estimator1, _sourceDataFile, this);
                    ret = training1.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked, chkPresTempHumidity.Checked);
                    trainedModel = training1;
                    break;

                case 2:
                    // 確率的双対座標上昇法(1)
                    IEstimator<ITransformer> estimator2 = mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features");
                    TrainingMultiClassification training2 = new TrainingMultiClassification(ref mlContext, "SdcaMaximumEntropy", ref estimator2, _sourceDataFile, this);
                    ret = training2.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked, chkPresTempHumidity.Checked);
                    trainedModel = training2;
                    break;

                case 3:
                    // 確率的双対座標上昇法(2)
                    IEstimator<ITransformer> estimator3 = mlContext.MulticlassClassification.Trainers.SdcaNonCalibrated("Label", "Features");
                    TrainingMultiClassification training3 = new TrainingMultiClassification(ref mlContext, "SdcaNonCalibrated", ref estimator3, _sourceDataFile, this);
                    ret = training3.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked, chkPresTempHumidity.Checked);
                    trainedModel = training3;
                    break;

                case 4:
                    // Naive Bayes
                    IEstimator<ITransformer> estimator4 = mlContext.MulticlassClassification.Trainers.NaiveBayes("Label", "Features");
                    TrainingMultiClassification training4 = new TrainingMultiClassification(ref mlContext, "NaiveBayes", ref estimator4, _sourceDataFile, this);
                    ret = training4.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked, chkPresTempHumidity.Checked);
                    trainedModel = training4;
                    break;

                case 0:
                    // 軽勾配ブースト マシン (LightGbm)
                    IEstimator<ITransformer> estimator5 = mlContext.MulticlassClassification.Trainers.LightGbm("Label", "Features");
                    TrainingMultiClassification training5 = new TrainingMultiClassification(ref mlContext, "LightGbm", ref estimator5, _sourceDataFile, this);
                    ret = training5.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked, chkPresTempHumidity.Checked);
                    trainedModel = training5;
                    break;

                case 5:
                    // Pairwise coupling
                    TrainingMultiBinaryClassification training6 = new TrainingMultiBinaryClassification(ref mlContext, MultiClassMethod.PairwiseCoupling, getBinaryClassificationMethod(), _sourceDataFile, this);
                    ret = training6.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked, chkPresTempHumidity.Checked);
                    trainedModel = training6;
                    break;

                case 6:
                    // One versus all
                    TrainingMultiBinaryClassification training7 = new TrainingMultiBinaryClassification(ref mlContext, MultiClassMethod.OneVersusAll, getBinaryClassificationMethod(), _sourceDataFile, this);
                    ret = training7.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked, chkPresTempHumidity.Checked);
                    trainedModel = training7;
                    break;

                case 7:
                default:
                    // K-Means
                    TrainingKMeansModel training0 = new TrainingKMeansModel(ref mlContext, _sourceDataFile, categoryCount, this);
                    ret = training0.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked, chkPresTempHumidity.Checked);
                    trainedModel = training0;
                    break;
            }
            if (trainedModel != null)
            {
                // モデルが作成された(保存ボタンを有効にする)
                btnSaveModel.Enabled = true;
                btnSaveModel.Visible = true;

                // ----- 作成したモデルをチェックする
                checkTheTrainedModel();

                txtResult.Text = " Created : " + trainedModel.getMethodName();
            }
            else
            {
                // モデルは作成できなかった(保存ボタンを無効にする)
                btnSaveModel.Enabled = false;
                btnSaveModel.Visible = false;
                txtResult.Text = "";
            }

            // ----- モデル作成の完了を通知 -----
            callback?.createModelFinished(ret, usePort, trainedModel, "create model done.");
        }

        private SensorToUse getPortType()
        {
            SensorToUse usePortType;
            if (selSensor1and2.Checked)
            {
                usePortType = SensorToUse.port1and2;
            }
            else if (selSensor1.Checked)
            {
                usePortType = SensorToUse.port1;
            }
            else if (selSensor2.Checked)
            {
                usePortType = SensorToUse.port2;
            }
            else // if (selSensor1or2.Checked)
            {
                usePortType = SensorToUse.port1or2;
            }
            return (usePortType);
        }

        private void checkTheTrainedModel()
        {
            // ----------------------------------------
            if (forDebugTest)
            {
                // ---------- テストデータで予測処理を実行 ----------
                SensorToUse usePortType = getPortType();
                TestSamplePrediction testSamplePrediction = new TestSamplePrediction();
                appendText(" - - - test training - - -\r\n");
                string reply = testSamplePrediction.testPrediction(usePortType, ref trainedModel, chkDataLog.Checked);
                appendText(reply);
                appendText(" - - -      done     - - -\r\n");
            }
        }


        private BinaryClassificationMethod getBinaryClassificationMethod()
        {
            BinaryClassificationMethod method;
            switch (cmbBinaryModel.SelectedIndex)
            {
                case 1:
                    method = BinaryClassificationMethod.Gam;
                    break;
                case 2:
                    method = BinaryClassificationMethod.FastTree;
                    break;
                case 3:
                    method = BinaryClassificationMethod.FastForest;
                    break;
                case 4:
                    method = BinaryClassificationMethod.AveragedPerceptron;
                    break;
                case 5:
                    method = BinaryClassificationMethod.LbfgsLogisticRegression;
                    break;
                case 6:
                    method = BinaryClassificationMethod.SymbolicSgdLogisticRegression;
                    break;
                case 0:
                default:
                    method = BinaryClassificationMethod.LightGbm;
                    break;
            }
            return (method);
        }

        private void btnLoadModel_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "zip files (*.zip)|*.zip|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 0;
                    openFileDialog.RestoreDirectory = true;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = openFileDialog.FileName;
                        appendText(" ---- Load pre-trained Model " + fileName + "----\r\n");
                        TrainingPreTrainedMultiClassification model = new TrainingPreTrainedMultiClassification(ref mlContext, this);
                        model.loadPreTrainedModel(fileName);
                        trainedModel = model;

                        // ----- 作成したモデルをチェックする
                        checkTheTrainedModel();

                        // モデルをロードしたときには、モデルは保存できない(保存ボタンを無効にする)
                        btnSaveModel.Enabled = false;
                        btnSaveModel.Visible = false;

                        // ----- モデル作成の完了を通知 -----
                        callback?.createModelFinished(true, getPortType(), trainedModel, "load pre-trained model done.");
                        txtResult.Text = " Load Model : " + fileName;
                        appendText(" ---- Load pre-trained Model : DONE. ----\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                trainedModel = null;
                Debug.WriteLine(DateTime.Now + " [Error] Load Pre-Trained Model : " + ex.Message);
                txtResult.Text = "[Error] Load pre-trained Model: " + ex.Message;
            }
        }

        private void btnSaveModel_Click(object sender, EventArgs e)
        {
            // ----- モデル保存ボタンを押したとき... モデルを保存する
            if (trainedModel == null)
            {
                // --------- モデルがないので保存しない。
                MessageBox.Show("The prediction model is not created yet.", "Save cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ----- 保存ダイアログを表示する
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "zip files (*.zip)|*.zip|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.OverwritePrompt = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;

                // ----- モデルを保存し、結果をダイアログで表示する
                if (trainedModel.savePredictionModel(fileName))
                {
                    string informationToShow = "Model saved : " + fileName;
                    MessageBox.Show("Model saved : " + fileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtResult.Text = informationToShow;
                }
                else
                {
                    string warningToShow = "[ERROR] The file did not saved : " + fileName;
                    MessageBox.Show(warningToShow, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtResult.Text = warningToShow;
                }
            }
        }

        private void btnSelectCategory_Click(object sender, EventArgs e)
        {

        }

        private void cmbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // モデルの選択を変更した時...
            if ((cmbModel.SelectedIndex == 5) || (cmbModel.SelectedIndex == 6))
            {
                // ----- 二項分類を選択できるようにする
                cmbBinaryModel.Enabled = true;
                cmbBinaryModel.Visible = true;
                cmbBinaryModel.Items.Clear();
                cmbBinaryModel.Items.Add("LightGbm");                       // index: 0
                cmbBinaryModel.Items.Add("Gam");                            // index: 1
                cmbBinaryModel.Items.Add("FastTree");                       // index: 2
                cmbBinaryModel.Items.Add("FastForest");                     // index: 3
                cmbBinaryModel.Items.Add("AveragedPerceptron");             // index: 4
                cmbBinaryModel.Items.Add("LbfgsLogisticRegression");        // index: 5
                cmbBinaryModel.Items.Add("SymbolicSgdLogisticRegression");  // index: 6
                cmbBinaryModel.SelectedIndex = 0;
            }
            else
            {
                // ----- 二項分類は選択しないようにする
                cmbBinaryModel.Items.Clear();
                cmbBinaryModel.Items.Add("---");
                cmbBinaryModel.SelectedIndex = 0;
                cmbBinaryModel.Visible = false;
                cmbBinaryModel.Enabled = false;
            }
        }

        private void selDuplicate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkDataLog_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rangeFromPercent_ValueChanged(object sender, EventArgs e)
        {
            prepareExportParameter();
        }

        private void rangeToPercent_ValueChanged(object sender, EventArgs e)
        {
            prepareExportParameter();
        }

        private void cmbBinaryModel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnClearMessage_Click(object sender, EventArgs e)
        {
            // ----- 表示エリアの内容をクリアする
            txtMessage.Text = "";
        }
    }
}
