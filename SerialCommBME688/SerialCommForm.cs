using SamplingBME688Serial;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
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
                String sendUrl = "";
                if (chkEntryDatabase.Checked)
                {
                    sendUrl = urlDatabaseToEntry.Text;
                }

                if (myReceiver.startReceive(txtPort.Text, category, sendUrl, txtConsole))
                {
                    // �f�[�^�̎�M�J�n
                    btnConnect.Enabled = false;
                    btnStop.Enabled = true;
                    btnExport.Enabled = false;
                    grpExportOption.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
                    btnReset.Enabled = false;
                    chkEntryDatabase.Enabled = false;
                    urlDatabaseToEntry.Enabled = false;
                }
                else
                {
                    // �f�[�^�̎�M�J�n�Ɏ��s
                    btnConnect.Enabled = true;
                    btnStop.Enabled = false;
                    btnExport.Enabled = true;
                    grpExportOption.Enabled = true;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = true;
                    btnReset.Enabled = true;
                    chkEntryDatabase.Enabled = true;
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
                    // �f�[�^�̎�M����
                    btnConnect.Enabled = true;
                    btnStop.Enabled = false;
                    btnExport.Enabled = true;
                    grpExportOption.Enabled = true;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = true;
                    btnReset.Enabled = true;
                    chkEntryDatabase.Enabled = true;
                    urlDatabaseToEntry.Enabled = true;
                    //txtConsole.AppendText("\r\n--- FINISH RECEIVE ---\r\n");
                }
                else
                {
                    // �f�[�^�̎�M�I���Ɏ��s
                    btnConnect.Enabled = false;
                    btnStop.Enabled = true;
                    btnExport.Enabled = false;
                    grpExportOption.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
                    btnReset.Enabled = false;
                    chkEntryDatabase.Enabled = false;
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
            // ���O�\���̃N���A
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

                    // CSV�փf�[�^�������o���B
                    if (!chkExportOnlyGasRegistanceLogarithm.Checked)
                    {
                        // �S�f�[�^�̃G�N�X�|�[�g
                        myReceiver.startExportAllDataToCsv(myStream, true);
                        myReceiver_2.startExportAllDataToCsv(myStream, false);
                    }
                    else
                    {
                        // �Z���T�̑ΐ��f�[�^�݂̂̃G�N�X�|�[�g����
                        if (chkCombineSensor.Checked)
                        {
                            // �Z���T�P�ƃZ���T�Q�̃f�[�^��������CSV�o�͂���
                            exportCsvEachSensorCombine(myStream);
                        }
                        else
                        {
                            // �Z���T�P�ƃZ���T�Q�̃f�[�^�����Ԃ�CSV�o�͂���
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

                // ----- �w�b�_�����̏o�� -----
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
                            if ((categoryList2.Contains(item))||(!myReceiver_2.isDataReceived()))
                            {
                                // �Z���T�P�ƃZ���T�Q�œ����J�e�S�����������ꍇ�����A�w�b�_�����ɏo�͂���
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
                    // �Z���T�P�̃f�[�^���Ȃ��ꍇ�́A�Z���T�Q�̂ݗ��p����B
                    foreach (String item in categoryList2)
                    {
                        writer.Write(item + ", ");
                        categories.Add(item);
                    }
                }
                writer.WriteLine(" ;");
                // ----- �w�b�_�����̏o�͂���� -----


                // �Z���T�P�ƃZ���T�Q�̃f�[�^�����Ԃɏo�͂���i�Z���T�P���Z���T�Q�̏��ɏo�͂���j
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
            // �f�[�^�O���b�h�ɕ\������J�����̃w�b�_�[��ݒ肷��
            dataGridView1.DataSource = dataSourceProvider.getGridDataSource();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // �f�[�^�����Z�b�g����
            myReceiver.reset();
            myReceiver_2.reset();
            dataSourceProvider.reset();
            txtDataCategory.Text = "";
        }

        private void btnShowGraph_Click(object sender, EventArgs e)
        {
            //  �f�[�^���P�ȏ�I������Ă������́A�O���t��\������
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        Debug.WriteLine(row.Index + " : : " + row.Cells[0].Value + " ");
                    }
                    // �f�[�^���I������Ă������́A�ڍ׃_�C�A���O��\������
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
                    sendUrl = urlDatabaseToEntry.Text;
                }
                txtConsole_2.Text = "--- START RECEIVE [" + txtPort_2.Text + "] (" + txtDataCategory.Text + ") ---\r\n";
                if (myReceiver_2.startReceive(txtPort_2.Text, category, sendUrl, txtConsole_2))
                {
                    // �f�[�^�̎�M�J�n
                    btnConnect_2.Enabled = false;
                    btnStop_2.Enabled = true;
                    btnExport.Enabled = false;
                    grpExportOption.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
                    btnReset.Enabled = false;
                    chkEntryDatabase.Enabled = false;
                    urlDatabaseToEntry.Enabled = false;
                }
                else
                {
                    // �f�[�^�̎�M�J�n�Ɏ��s
                    btnConnect_2.Enabled = true;
                    btnStop_2.Enabled = false;
                    btnExport.Enabled = true;
                    grpExportOption.Enabled = true;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = true;
                    btnReset.Enabled = true;
                    chkEntryDatabase.Enabled = true;
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
                    // �f�[�^�̎�M����
                    btnConnect_2.Enabled = true;
                    btnStop_2.Enabled = false;
                    btnExport.Enabled = true;
                    grpExportOption.Enabled = true;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = true;
                    btnReset.Enabled = true;
                    //txtConsole_2.AppendText("\r\n--- FINISH RECEIVE ---\r\n");
                    chkEntryDatabase.Enabled = true;
                    urlDatabaseToEntry.Enabled = true;
                }
                else
                {
                    // �f�[�^�̎�M�I���Ɏ��s
                    btnConnect_2.Enabled = false;
                    btnStop_2.Enabled = true;
                    btnExport.Enabled = false;
                    grpExportOption.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
                    btnReset.Enabled = false;
                    chkEntryDatabase.Enabled = false;
                    urlDatabaseToEntry.Enabled = false;
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
