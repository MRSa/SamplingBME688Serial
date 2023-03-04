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
    public partial class SerialCommForm : Form
    {
        private GridDataSourceProvider dataSourceProvider;// = new GridDataSourceProvider();
        private SerialReceiver myReceiver;// = new SerialReceiver(1, dataSourceProvider);
        private SerialReceiver myReceiver_2;// = new SerialReceiver(2, dataSourceProvider);
        private DataTable sensorDataList = new DataTable();
        private DbStatusDialog? dbStatusDialog = null;

        public SerialCommForm()
        {
            dataSourceProvider = new GridDataSourceProvider();
            myReceiver = new SerialReceiver(1, dataSourceProvider);
            myReceiver_2 = new SerialReceiver(2, dataSourceProvider);
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                String category = txtDataCategory.Text;
                txtConsole.Text = "--- START RECEIVE [" + txtPort.Text + "] (" + txtDataCategory.Text + ") ---\r\n";
                String sendUrl = "";
                if (chkEntryDatabase.Checked)
                {
                    sendUrl = urlDatabaseToEntry.Text + "sensor/entry";
                }

                if (myReceiver.startReceive(txtPort.Text, category, sendUrl, chkDbEntrySingle.Checked, txtConsole))
                {
                    // データの受信開始
                    btnConnect.Enabled = false;
                    btnStop.Enabled = true;
                    btnExport.Enabled = false;
                    grpExportOption.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
                    btnReset.Enabled = false;
                    btnDbStatus.Enabled = false;
                    chkEntryDatabase.Enabled = false;
                    chkDbEntrySingle.Enabled = false;
                    urlDatabaseToEntry.Enabled = false;
                }
                else
                {
                    // データの受信開始に失敗
                    btnConnect.Enabled = true;
                    btnStop.Enabled = false;
                    btnExport.Enabled = true;
                    grpExportOption.Enabled = true;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = true;
                    btnReset.Enabled = true;
                    btnDbStatus.Enabled = true;
                    chkEntryDatabase.Enabled = true;
                    chkDbEntrySingle.Enabled = true;
                    urlDatabaseToEntry.Enabled = true;
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
                    btnConnect.Enabled = true;
                    btnStop.Enabled = false;
                    btnExport.Enabled = true;
                    grpExportOption.Enabled = true;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = true;
                    btnReset.Enabled = true;
                    btnDbStatus.Enabled = true;
                    chkEntryDatabase.Enabled = true;
                    chkDbEntrySingle.Enabled = true;
                    urlDatabaseToEntry.Enabled = true;
                    //txtConsole.AppendText("\r\n--- FINISH RECEIVE ---\r\n");
                }
                else
                {
                    // データの受信終了に失敗
                    btnConnect.Enabled = false;
                    btnStop.Enabled = true;
                    btnExport.Enabled = false;
                    grpExportOption.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
                    btnReset.Enabled = false;
                    btnDbStatus.Enabled = false;
                    chkEntryDatabase.Enabled = false;
                    chkDbEntrySingle.Enabled = false;
                    urlDatabaseToEntry.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                txtConsole.AppendText(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // ログ表示のクリア
            txtConsole.Text = "";
            txtConsole_2.Text = "";
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

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    Debug.WriteLine("OpenFile : canWrite: " + myStream.CanWrite);

                    // CSVへデータを書き出す。
                    if (!chkExportOnlyGasRegistanceLogarithm.Checked)
                    {
                        // 全データのエクスポート
                        myReceiver.startExportAllDataToCsv(myStream, true);
                        myReceiver_2.startExportAllDataToCsv(myStream, false);
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
        }

        private void btnShowGraph_Click(object sender, EventArgs e)
        {
            //  データが１つ以上選択されていた時は、グラフを表示する
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        Debug.WriteLine(row.Index + " : : " + row.Cells[0].Value + " ");
                    }
                    // データが選択されていた時は、詳細ダイアログを表示する
                    DataDetailDialog dialog = new DataDetailDialog();
                    dialog.Owner = this;
                    dialog.Show();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(DateTime.Now + " btnShowGraph_Click()" + ex.Message);
                }
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
                txtConsole_2.Text = "--- START RECEIVE [" + txtPort_2.Text + "] (" + txtDataCategory.Text + ") ---\r\n";
                if (myReceiver_2.startReceive(txtPort_2.Text, category, sendUrl, chkDbEntrySingle.Checked, txtConsole_2))
                {
                    // データの受信開始
                    btnConnect_2.Enabled = false;
                    btnStop_2.Enabled = true;
                    btnExport.Enabled = false;
                    grpExportOption.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
                    btnReset.Enabled = false;
                    btnDbStatus.Enabled = false;
                    chkEntryDatabase.Enabled = false;
                    chkDbEntrySingle.Enabled = false;
                    urlDatabaseToEntry.Enabled = false;
                }
                else
                {
                    // データの受信開始に失敗
                    btnConnect_2.Enabled = true;
                    btnStop_2.Enabled = false;
                    btnExport.Enabled = true;
                    grpExportOption.Enabled = true;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = true;
                    btnReset.Enabled = true;
                    btnDbStatus.Enabled = true;
                    chkEntryDatabase.Enabled = true;
                    chkDbEntrySingle.Enabled = true;
                    urlDatabaseToEntry.Enabled = true;
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
                    btnConnect_2.Enabled = true;
                    btnStop_2.Enabled = false;
                    btnExport.Enabled = true;
                    grpExportOption.Enabled = true;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = true;
                    btnReset.Enabled = true;
                    btnDbStatus.Enabled = true;
                    //txtConsole_2.AppendText("\r\n--- FINISH RECEIVE ---\r\n");
                    chkEntryDatabase.Enabled = true;
                    chkDbEntrySingle.Enabled = true;
                    urlDatabaseToEntry.Enabled = true;
                }
                else
                {
                    // データの受信終了に失敗
                    btnConnect_2.Enabled = false;
                    btnStop_2.Enabled = true;
                    btnExport.Enabled = false;
                    grpExportOption.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
                    btnReset.Enabled = false;
                    btnDbStatus.Enabled = false;
                    chkEntryDatabase.Enabled = false;
                    chkDbEntrySingle.Enabled = false;
                    urlDatabaseToEntry.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                txtConsole.AppendText(ex.Message);
            }
        }

        private void btnDbStatus_Click(object sender, EventArgs e)
        {
            // DBのステータスボタンを押したとき...
            try
            {
                // 一覧情報を取得する
                //sensorDataList.Clear();
                String listUrl = urlDatabaseToEntry.Text + "sensor/list";
                getSensorDataList(listUrl);

                // ダイアログ（？）を表示する
                if ((dbStatusDialog == null)||(dbStatusDialog.IsDisposed))
                {
                    dbStatusDialog = new DbStatusDialog(sensorDataList);
                }
                dbStatusDialog.Owner = this;
                if (!dbStatusDialog.Visible)
                {
                    //dbStatusDialog.Show();
                    dbStatusDialog.ShowDialog();
                }
                else
                {
                    dbStatusDialog.Activate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " btnDbStatus_Click() " + ex.Message);
            }
        }

        private async void getSensorDataList(String listUrl)
        {
            try
            {
                var result = await new HttpClient().GetAsync(listUrl, HttpCompletionOption.ResponseHeadersRead);
                String response = await result.Content.ReadAsStringAsync();

                // 応答に合わせてデータを設定する
                sensorDataList = new DataTable();
                sensorDataList.Clear();
                sensorDataList.Columns.Add("category");
                sensorDataList.Columns.Add("sensor_id");
                sensorDataList.Columns.Add("count");

                try
                {
                    // Debug.WriteLine(DateTime.Now + " getSensorDataList() : " + response);
                    var dataList = JsonSerializer.Deserialize<SensorDataResult>(response);
                    if ((dataList != null)&&(dataList.result != null))
                    {
                        foreach (SensorData data in dataList.result)
                        {
                            sensorDataList.Rows.Add(data.category, data.sensor_id, data.count);
                        }
                    }
                    if (dbStatusDialog != null)
                    {
                        dbStatusDialog.setDataTable(sensorDataList);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(DateTime.Now + " getSensorDataList() " + e.Message);
                }
                GC.Collect();
            }
            catch (Exception ex)
            { 
                Debug.WriteLine(DateTime.Now + " getSensorDataList() " + ex.Message);
            }
        }

        private class SensorDataResult
        {
            public SensorData[]? result { get; set;  }
        }

        private class SensorData
        {
            public String? category { get; set;  }
            public int sensor_id { get; set;  }
            public int count { get; set; }
        }

    }
}
