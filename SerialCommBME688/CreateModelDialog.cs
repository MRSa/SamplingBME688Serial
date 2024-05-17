using Microsoft.ML;
using System.Diagnostics;

namespace SamplingBME688Serial
{

    interface ICreateModelConsole
    {
        void appendText(String itemData);
    }

    public partial class CreateModelDialog : Form, ICreateModelConsole
    {
        //  forDebugTest を true にすると、モデルを作成したときにテストデータで予測する
        private bool forDebugTest = true;

        private String _sourceDataFile = Path.Combine(System.IO.Path.GetTempPath(), "modelsrc.csv");
        private MLContext mlContext;
        private ICreateModelResult? callback = null;
        private IDataHolder? port1 = null;
        private IDataHolder? port2 = null;
        private List<String> categoryList = new List<String>();
        private int minimumDataCount = int.MaxValue;
        private int outputDataCount = 0;
        private int startPosition = 0;
        private int fromValue = 0;
        private int toValue = 100;
        private int port1CategoryCount = 0;
        private int port2CategoryCount = 0;
        private int categoryCount = 0;

        public CreateModelDialog(ref MLContext mlContext)
        {
            this.mlContext = mlContext;

            InitializeComponent();

            // ----- 
            cmbModel.Items.Clear();
            cmbModel.Items.Add("K-Means");               // index: 0
            cmbModel.Items.Add("LbfgsMaximumEntropy");   // index: 1
            cmbModel.Items.Add("SdcaMaximumEntropy");    // index: 2
            cmbModel.Items.Add("SdcaNonCalibrated");     // index: 3
            cmbModel.Items.Add("Naive Bayes");         // index: 4
            //cmbModel.Items.Add("LightGbm");            // index: 5
            cmbModel.SelectedIndex = 0;

            // ----- 
            selDuplicate.Items.Clear();
            selDuplicate.Items.Add("x1");
            selDuplicate.Items.Add("x5");
            selDuplicate.Items.Add("x10");
            selDuplicate.Items.Add("x20");
            selDuplicate.Items.Add("x30");
            selDuplicate.Items.Add("x50");
            selDuplicate.Items.Add("x100");
            selDuplicate.SelectedIndex = 0;
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
                    Dictionary<String, List<List<GraphDataValue>>>  dataSetMap = port1.getGasRegDataSet();
                    foreach (KeyValuePair <String, List<List<GraphDataValue>>> item in dataSetMap)
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
                    Dictionary<String, List<List<GraphDataValue>>> dataSetMap = port2.getGasRegDataSet();
                    foreach (KeyValuePair<String, List<List<GraphDataValue>>> item in dataSetMap)
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
                    if ((port2CategoryCount > 0)&&(port2CategoryCount < port1CategoryCount))
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

        public void appendText(String itemData)
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

        private void deleteDataSourceFile(String fileName)
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
                if ((range <= 0)||(range >= 100))
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
            String message = "Create Model, READY?";

            // ----- モデルの作成を開始するか、確認する
            DialogResult result = MessageBox.Show(message, "Create Model", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if ((result == DialogResult.No)||(result == DialogResult.Cancel))
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
                ret = csvExporter.outputDataSourceCSVFile1and2(startPosition, outputDataCount, duplicateTimes, chkDataLog.Checked);
                usePort = SensorToUse.port1and2;
            }
            else if (selSensor1.Checked)
            {
                ret = csvExporter.outputDataSourceCSVFileSingle(1, startPosition, outputDataCount, duplicateTimes, chkDataLog.Checked);
                usePort = SensorToUse.port1;
            }
            else if (selSensor2.Checked)
            {
                ret = csvExporter.outputDataSourceCSVFileSingle(2, startPosition, outputDataCount, duplicateTimes, chkDataLog.Checked);
                usePort = SensorToUse.port2;
            }
            else // if (selSensor1or2.Checked)
            {
                ret = csvExporter.outputDataSourceCSVFile1or2(startPosition, outputDataCount, duplicateTimes, chkDataLog.Checked);
                usePort = SensorToUse.port1or2;
            }

            // ----- モデルの作成
            IPredictionModel? trainingModel = null;
            switch (cmbModel.SelectedIndex)
            {
                case 1:
                    IEstimator<ITransformer> estimator1 = mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy("Label", "Features");
                    TrainingMulticlassClassificationBase training1 = new TrainingMulticlassClassificationBase(ref mlContext, "LbfgsMaximumEntropy", ref estimator1, _sourceDataFile, this);
                    ret = training1.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked);
                    trainingModel = training1;
                    break;

                case 2:
                    IEstimator<ITransformer> estimator2 = mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features");
                    TrainingMulticlassClassificationBase training2 = new TrainingMulticlassClassificationBase(ref mlContext, "SdcaMaximumEntropy", ref estimator2, _sourceDataFile, this);
                    ret = training2.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked);
                    trainingModel = training2;
                    break;

                case 3:
                    IEstimator<ITransformer> estimator3 = mlContext.MulticlassClassification.Trainers.SdcaNonCalibrated("Label", "Features");
                    TrainingMulticlassClassificationBase training3 = new TrainingMulticlassClassificationBase(ref mlContext, "SdcaNonCalibrated", ref estimator3, _sourceDataFile, this);
                    ret = training3.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked);
                    trainingModel = training3;
                    break;

                case 4:
                    IEstimator<ITransformer> estimator4 = mlContext.MulticlassClassification.Trainers.NaiveBayes("Label", "Features");
                    TrainingMulticlassClassificationBase training4 = new TrainingMulticlassClassificationBase(ref mlContext, "NaiveBayes", ref estimator4, _sourceDataFile, this);
                    ret = training4.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked);
                    trainingModel = training4;
                    break;
/*
                 case 5:
                    IEstimator<ITransformer> estimator5 = mlContext.MulticlassClassification.Trainers("Label", "Features");
                    TrainingMulticlassClassificationBase training5 = new TrainingMulticlassClassificationBase(ref mlContext, "LightGbm", ref estimator5, _sourceDataFile, this);
                    ret = training5.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked);
                    trainingModel = training5;
                    break;
*/
                case 0:
                default:
                    TrainingKMeansModel training0 = new TrainingKMeansModel(ref mlContext, _sourceDataFile, categoryCount, this);
                    ret = training0.executeTraining(usePort, null, ref port1, ref port2, chkDataLog.Checked);
                    trainingModel = training0;
                    break;
            }

            // ----------------------------------------
            if (forDebugTest)
            {
                // ---------- テストデータで予測処理を実行 ----------
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
                TestSamplePrediction testSamplePrediction = new TestSamplePrediction();
                appendText(" - - - test training - - -\r\n");
                String reply = testSamplePrediction.testPrediction(usePortType, ref trainingModel, chkDataLog.Checked); 
                appendText(reply);
                appendText(" - - -      done     - - -\r\n");
            }

            // ----- モデル作成の完了を通知 -----
            callback?.createModelFinished(ret, usePort, trainingModel, "create model done.");
        }

        private void btnLoadModel_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveModel_Click(object sender, EventArgs e)
        {

        }

        private void btnSelectCategory_Click(object sender, EventArgs e)
        {

        }

        private void cmbModel_SelectedIndexChanged(object sender, EventArgs e)
        {

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
    }
}
