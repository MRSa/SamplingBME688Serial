using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommBME688
{
    internal class Bme688DataSetGroup
    {
        private int nofIndex;

        public string dataGroupName { get; private set; }
        private List<Bme688DataSet> collectedDataSet = new List<Bme688DataSet>();
        private Bme688DataSet? currentDataSet = null;

        public Bme688DataSetGroup(string groupName, int number_of_index)
        {
            this.dataGroupName = groupName;
            this.nofIndex = number_of_index;
        }

        public void startReceiveDataSet(double temperature, double humidity, double pressure, double gas_registance, double gas_registance_log, double gas_registance_diff)
        {
            try
            {
                finishReceivedDataSet();
                currentDataSet = new Bme688DataSet(nofIndex, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " startReceiveDataSet() : " + e.Message);
            }
        }

        public void setReceivedData(int index, double temperature, double humidity, double pressure, double gas_registance, double gas_registance_log, double gas_registance_diff)
        {
            try
            {
                if (currentDataSet != null)
                {
                    currentDataSet.setBme688Data(index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " setReceivedData() : " + e.Message);
            }
        }

        public void finishReceivedDataSet()
        {
            try
            {
                if (currentDataSet != null)
                {
                    collectedDataSet.Add(currentDataSet);
                }
                currentDataSet = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " finishReceivedDataSet() : " + e.Message);
            }
        }

        public List<Bme688DataSet> getCollectedDataSet()
        {
            return (collectedDataSet);
        }

    }
}
