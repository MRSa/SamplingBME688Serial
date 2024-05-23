using SerialCommBME688;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    interface IDataReceiveNotify
    {
       void finishReceivedData(ref Bme688DataSummary dataSummary);
    }

    public class GridDataSourceProvider : IDataReceiveNotify
    {
        private int minimumValidCount = -1;
        private DataTable dataSource = new DataTable("dataSummary");
        private Dictionary<string, Bme688DataSummary> dataSummaryMap = new Dictionary<string, Bme688DataSummary>();

        public DataTable getGridDataSource()
        {
            try
            {
                // データグリッドに表示するカラムのヘッダーを設定する
                dataSource.Columns.Add("Category");
                dataSource.Columns.Add("sensorId", Type.GetType("System.Int32"));
                dataSource.Columns.Add("dataCount", Type.GetType("System.Int32"));
                dataSource.Columns.Add("validCount", Type.GetType("System.Int32"));
                dataSource.Columns.Add("Temp. - Max", Type.GetType("System.Double"));
                dataSource.Columns.Add("Temp. - Min", Type.GetType("System.Double"));
                dataSource.Columns.Add("Humi. - Max", Type.GetType("System.Double"));
                dataSource.Columns.Add("Humi. - Min", Type.GetType("System.Double"));
                dataSource.Columns.Add("Pres. - Max", Type.GetType("System.Double"));
                dataSource.Columns.Add("Pres. - Min", Type.GetType("System.Double"));
                dataSource.Columns.Add("GasR. - Max", Type.GetType("System.Double"));
                dataSource.Columns.Add("GasR. - Min", Type.GetType("System.Double"));
                dataSource.Columns.Add("GasR.(log) - Max", Type.GetType("System.Double"));
                dataSource.Columns.Add("GasR.(log) - Min", Type.GetType("System.Double"));
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " getGridDataSource() : " + e.Message);

            }
            return (dataSource);
        }

        public void finishReceivedData(ref Bme688DataSummary dataSummary)
        {
            dataSource.Clear();
            try
            {
                string key = dataSummary.category + "-" + dataSummary.sensorId;
                dataSummaryMap[key] = dataSummary;

                foreach (KeyValuePair<string, Bme688DataSummary> item in dataSummaryMap)
                {
                    Bme688DataSummary dataSummaryItem = item.Value;

                    //  有効サンプル回数の最小値を決める
                    if (minimumValidCount < 0)
                    {
                        minimumValidCount = dataSummaryItem.validCount;
                    }
                    if (minimumValidCount > dataSummaryItem.validCount)
                    {
                        minimumValidCount = dataSummaryItem.validCount;
                    }
                    dataSource.Rows.Add(dataSummaryItem.category,
                                        dataSummaryItem.sensorId,
                                        dataSummaryItem.sampleCount,
                                        dataSummaryItem.validCount,
                                        dataSummaryItem.temperature_max,
                                        dataSummaryItem.temperature_min,
                                        dataSummaryItem.humidity_max,
                                        dataSummaryItem.humidity_min,
                                        dataSummaryItem.pressure_max,
                                        dataSummaryItem.pressure_min,
                                        dataSummaryItem.gas_registance_max,
                                        dataSummaryItem.gas_registance_min,
                                        dataSummaryItem.gas_registance_log_max,
                                        dataSummaryItem.gas_registance_log_min);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " updateDataTable() : " + e.Message);
            }
        }

        public int getValidCount()
        {
            if (minimumValidCount < 0)
            {
                // まだ収集していない場合...
                return (0);
            }
            return (minimumValidCount);
        }

        public void reset()
        {
            dataSummaryMap.Clear();
            dataSource.Clear();
            minimumValidCount = -1;
        }
    }
}
