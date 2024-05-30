using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SamplingBME688Serial
{
    public partial class DbStatusDialog : Form
    {
        private ILoadDataFromDatabase loadDataFromDatabase;
        private List<LoadSensorDataInformation> categoryToLoad = new List<LoadSensorDataInformation>();
        public DbStatusDialog(DataTable dataToShow, ILoadDataFromDatabase loadDataFromDatabase)
        {
            InitializeComponent();

            this.loadDataFromDatabase = loadDataFromDatabase;
            gridDbStatus.DataSource = dataToShow;
            gridDbStatus.ReadOnly = true;
            gridDbStatus.RowHeadersVisible = false;

        }

        public void setLoadDataMode(bool isLoadData)
        {
            this.Text = isLoadData ? "Load data from database" : "Dabase Entry Status";
            btnLoad.Enabled = isLoadData;
            btnLoad.Visible = isLoadData;
         }

        public void setDataTable(DataTable dataToShow)
        {
            gridDbStatus.DataSource = dataToShow;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            categoryToLoad.Clear();

            loadDataFromDatabase.LoadDataFromDatabase(categoryToLoad);
        }
    }
}
