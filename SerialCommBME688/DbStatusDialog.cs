using System.Data;
using System.Diagnostics;

namespace SamplingBME688Serial
{
    public partial class DbStatusDialog : Form
    {
        private bool loadData = false;
        private ILoadDataFromDatabase loadDataFromDatabase;
        private List<LoadSensorDataInformation> categoryToLoad = new List<LoadSensorDataInformation>();
        // private string urlToGetData;
        public DbStatusDialog(DataTable dataToShow, ILoadDataFromDatabase loadDataFromDatabase)
        {
            InitializeComponent();

            this.loadDataFromDatabase = loadDataFromDatabase;
            gridDbStatus.DataSource = dataToShow;
            gridDbStatus.ReadOnly = true;
            gridDbStatus.RowHeadersVisible = false;
            gridDbStatus.AllowUserToAddRows = false;

        }

        public void setLoadDataMode(bool isLoadData, string getUrl)
        {
            this.Text = isLoadData ? "Load data from database" : "Dabase Entry Status : " + getUrl;

            btnLoad.Enabled = isLoadData;
            btnLoad.Visible = isLoadData;

            btnCheckUncheckAll.Enabled = isLoadData;
            btnCheckUncheckAll.Visible = isLoadData;

            lblFrom.Enabled = isLoadData;
            lblFrom.Visible = isLoadData;
            fldFrom.Enabled = isLoadData;
            fldFrom.Visible = isLoadData;

            lblCount.Enabled = isLoadData;
            lblCount.Visible = isLoadData;
            fldCount.Enabled = isLoadData;
            fldCount.Visible = isLoadData;

            loadData = isLoadData;
            //urlToGetData = getUrl;
        }

        public void setDataTable(DataTable dataToShow)
        {
            gridDbStatus.DataSource = dataToShow;
            if (loadData)
            {
                // データ読み込みモードの時は、先頭列のみ編集可にする
                gridDbStatus.ReadOnly = false;
                gridDbStatus.Columns[0].ReadOnly = false;
                gridDbStatus.Columns[0].DisplayIndex = 0;
                for (int index = 1; index < gridDbStatus.ColumnCount; index++)
                {
                    gridDbStatus.Columns[index].ReadOnly = true;
                    gridDbStatus.Columns[index].DisplayIndex = index;
                }
            }
            else
            {
                // 表全体を Read Only にする
                gridDbStatus.ReadOnly = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            categoryToLoad.Clear();
            int dataCountFrom = 0;
            int getDataCount = 0;

            try
            {
                string fromStr = fldFrom.Text.ToString();
                dataCountFrom = int.Parse(fromStr);
                if (dataCountFrom < 0)
                {
                    dataCountFrom = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] btnLoad_Click() (get from) " + ex.Message);
                dataCountFrom = 0;
            }
            try
            {
                string countStr = fldCount.Text.ToString();
                getDataCount = int.Parse(countStr);
                if (getDataCount < 0)
                {
                    getDataCount = 1000000;
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(DateTime.Now + " [ERROR] btnLoad_Click() (get count) " + ex.Message);
                dataCountFrom = 0;
            }

            try
            {
                // ---------- チェックの入った データを引っ張る準備
                foreach (DataGridViewRow row in gridDbStatus.Rows)
                {
                    if ((row.Cells[0].ValueType == typeof(bool)) && (row.Cells[0].Value.ToString() == "True"))
                    {
                        string? sensorIdStr = row.Cells["sensor_id"].Value.ToString();
                        string? dataCountStr = row.Cells["count"].Value.ToString();
                        string? categoryName = row.Cells["category"].Value.ToString();

                        if (sensorIdStr != null && dataCountStr != null && categoryName != null)
                        {
                            int dataCount = int.Parse(dataCountStr);
                            Debug.WriteLine(DateTime.Now + " [INFO] btnLoad_Click() : Load  " + categoryName + " (" + sensorIdStr + "), count: " + dataCount + " " + getDataCount + " from " + dataCountFrom);

                            if (dataCount > (getDataCount + dataCountFrom))
                            {
                                // 取得データカウント数を制限する
                                dataCount = getDataCount;
                            }

                            if (dataCount > 0)
                            {
                                categoryToLoad.Add(new LoadSensorDataInformation(categoryName, int.Parse(sensorIdStr), dataCountFrom, dataCount));
                            }
                        }
                    }
                }

                if (categoryToLoad.Count > 0)
                {
                    DialogResult result = MessageBox.Show(" Data import " + categoryToLoad.Count + " categories from database, are you OK?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // ----- データベースからのデータ読み込みを開始する
                        Debug.WriteLine(DateTime.Now + " [INFO] btnLoad_Click() : confirmation OK : " + categoryToLoad.Count + " categories.");
                        loadDataFromDatabase.LoadDataFromDatabase(ref categoryToLoad);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] btnLoad_Click() " + ex.Message);
            }
        }

        private void btnCheckUncheckAll_Click(object sender, EventArgs e)
        {
            if (!loadData)
            {
                // ----- ステータス表示モードなので、何もしない (ボタンは表示されないはず)
                Debug.WriteLine(DateTime.Now + " [ERROR] btnCheckUncheckAll_Click() " + loadData);
                return;
            }
            try
            {
                if (gridDbStatus.Rows[0].Cells[0].ValueType != typeof(bool))
                {
                    // ----- データの先頭がチェックボックス出ないので、何もしない 
                    Debug.WriteLine(DateTime.Now + " [ERROR] btnCheckUncheckAll_Click() no chkBox.");
                    return;
                }

                // ----- チェックボックスのチェックを反転させる
                bool chkValue = (gridDbStatus.Rows[0].Cells[0].Value.ToString() == "True") ? false : true;

                // ---------- チェックボックスのチェックを更新する
                foreach (DataGridViewRow row in gridDbStatus.Rows)
                {
                    row.Cells[0].Value = chkValue;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] btnCheckUncheckAll_Click() " + ex.Message);
            }
        }
    }
}
