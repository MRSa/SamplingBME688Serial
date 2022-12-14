using SamplingBME688Serial;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows.Forms;

namespace SerialCommBME688
{
    public partial class SerialCommForm : Form
    {
        private GridDataSourceProvider dataSourceProvider;// = new GridDataSourceProvider();
        private SerialReceiver myReceiver;// = new SerialReceiver(1, dataSourceProvider);
        private SerialReceiver myReceiver_2;// = new SerialReceiver(2, dataSourceProvider);


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
                if (myReceiver.startReceive(txtPort.Text, category, txtConsole))
                {
                    // データの受信開始
                    btnConnect.Enabled = false;
                    btnStop.Enabled = true;
                    btnExport.Enabled = false;
                    grpExportOption.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
                    btnReset.Enabled = false;
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
                    myReceiver.startExportCsv(myStream, chkExportOnlyGasRegistanceLogarithm.Checked, Decimal.ToInt32(numDuplicate.Value));
                    myStream.Close();
                }
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
            }
            else
            {
                numDuplicate.Enabled = false;
            }
        }

        private void btnConnect_2_Click(object sender, EventArgs e)
        {
            try
            {
                String category = txtDataCategory.Text;
                txtConsole_2.Text = "--- START RECEIVE [" + txtPort_2.Text + "] (" + txtDataCategory.Text + ") ---\r\n";
                if (myReceiver_2.startReceive(txtPort_2.Text, category, txtConsole_2))
                {
                    // データの受信開始
                    btnConnect_2.Enabled = false;
                    btnStop_2.Enabled = true;
                    btnExport.Enabled = false;
                    grpExportOption.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
                    btnReset.Enabled = false;
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
                    //txtConsole_2.AppendText("\r\n--- FINISH RECEIVE ---\r\n");
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

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                txtConsole.AppendText(ex.Message);
            }
        }
    }
}
