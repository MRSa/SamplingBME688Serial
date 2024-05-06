using Microsoft.ML;
using SerialCommBME688;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace SamplingBME688Serial
{

    interface ICreateModelConsole
    {
        void appendText(String itemData);
    }

    public partial class CreateModelDialog : Form, ICreateModelConsole
    {

        private String _sourceDataFile = Path.Combine(System.IO.Path.GetTempPath(), "modelsrc.csv");
        private MLContext mlContext;
        private ICreateModelResult? callback = null;
        private IDataHolder? port1 = null;
        private IDataHolder? port2 = null;
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
            cmbModel.Items.Add("KMeans");
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

            // ----- 一時データファイルの削除
            deleteDataSourceFile();

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

        private void deleteDataSourceFile()
        {
            try
            {
                FileInfo file = new FileInfo(_sourceDataFile);
                if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    file.Attributes = FileAttributes.Normal;
                }
                file.Delete();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " deleteDataSourceFile : " + ex.Message);
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

            // モデルの作成
            TrainingKMeansModel training = new TrainingKMeansModel(ref mlContext, _sourceDataFile, categoryCount, this);
            training.executeTraining(usePort, null);

            // ----- ----- ----- -----
            callback?.createModelFinished(ret, usePort, training, "create model done.");
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
