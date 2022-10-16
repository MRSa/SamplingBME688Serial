using System.Diagnostics;
using System.IO.Ports;

namespace SerialCommBME688
{
    public partial class SerialCommForm : Form
    {
        private SerialReceiver myReceiver = new SerialReceiver();


        public SerialCommForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                String category = txtDataCategory.Text;
                txtConsole.Text = "--- START RECEIVE (" + txtDataCategory.Text + ") ---\r\n";
                if (myReceiver.startReceive(txtPort.Text, category, txtConsole))
                {
                    // �f�[�^�̎�M�J�n
                    btnConnect.Enabled = false;
                    btnStop.Enabled = true;
                    btnExport.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
                }
                else
                {
                    // �f�[�^�̎�M�J�n�Ɏ��s
                    btnConnect.Enabled = true;
                    btnStop.Enabled = false;
                    btnExport.Enabled = true;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = true;

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
                    chkExportOnlyGasRegistanceLogarithm.Enabled = true;
                    //txtConsole.AppendText("\r\n--- FINISH RECEIVE ---\r\n");
                }
                else
                {
                    // �f�[�^�̎�M�I���Ɏ��s
                    btnConnect.Enabled = false;
                    btnStop.Enabled = true;
                    btnExport.Enabled = false;
                    chkExportOnlyGasRegistanceLogarithm.Enabled = false;
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
                    myReceiver.startExportCsv(myStream, chkExportOnlyGasRegistanceLogarithm.Checked);
                    myStream.Close();
                }
            }
        }

        private void SerialCommForm_Load(object sender, EventArgs e)
        {
            // �f�[�^�O���b�h�ɕ\������J�����̃w�b�_�[��ݒ肷��
            dataGridView1.DataSource = myReceiver.getGridDataSource();
        }
    }
}
