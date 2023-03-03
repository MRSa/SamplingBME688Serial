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
        public DbStatusDialog(DataTable dataToShow)
        {
            InitializeComponent();

            gridDbStatus.DataSource = dataToShow;
            gridDbStatus.ReadOnly = true;
            gridDbStatus.RowHeadersVisible = false;
        }

    }
}
