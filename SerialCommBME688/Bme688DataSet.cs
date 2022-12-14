using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommBME688
{
    internal class Bme688DataSet
    {
        public int lack_data { get; private set; }
        public double temperature_max { get; private set; }
        public double temperature_min { get; private set; }
        public double humidity_max { get; private set; }
        public double humidity_min { get; private set; }
        public double pressure_max { get; private set; }
        public double pressure_min { get; private set; }
        public double gas_registance_max { get; private set; }
        public double gas_registance_min { get; private set; }
        public double gas_registance_log_max { get; private set; }
        public double gas_registance_log_min { get; private set; }
        public double gas_registance_diff_max { get; private set; }
        public double gas_registance_diff_min { get; private set; }

        private List<Bme688Data> observedData; 

        public Bme688DataSet(int nofIndex, double temperature, double humidity, double pressure, double gas_registance, double gas_registance_log, double gas_registance_diff)
        {
            // データ数の最大を設定し、エリアを初期化する
            if ((nofIndex < 0)||(nofIndex > 100))
            {
                //  入力値が変な時は、補正する。（ありえないはず..）
                nofIndex = 10;
            }

            observedData = new List<Bme688Data>(nofIndex);
            try
            {
                lack_data = nofIndex;
                for (int index = 0; index < nofIndex; index++)
                {
                    observedData.Add(new Bme688Data());
                }

                // 初回データ(の先頭データに)格納

                observedData[0].setBme688Data(temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                lack_data--;

                // 最大値・最小値のデータを入れておく
                temperature_max = temperature;
                temperature_min = temperature;

                humidity_max = humidity;
                humidity_min = humidity;

                pressure_max = pressure;
                pressure_min = pressure;

                gas_registance_max = gas_registance;
                gas_registance_min = gas_registance;

                gas_registance_log_max = gas_registance_log;
                gas_registance_log_min = gas_registance_log;

                gas_registance_diff_max = gas_registance_diff;
                gas_registance_diff_min = gas_registance_diff;

            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " Bme688DataSet(" + nofIndex + ") : " + e.Message);
            }
        }

        public void setBme688Data(int index, double temperature, double humidity, double pressure, double gas_registance, double gas_registance_log, double gas_registance_diff)
        {
            try
            {
                if (!observedData[index].isValid)
                {
                    // データ格納
                    observedData[index].setBme688Data(temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                    lack_data--;
                    if (lack_data < 0)
                    {
                        // 本来ありえないはずだが... 負の値にはしない。
                        lack_data = 0;
                    }
                    // 各データの最大値・最小値の更新を行う
                    if (temperature_max < temperature) { temperature_max = temperature; }
                    if (temperature_min > temperature) { temperature_min = temperature; }
                    if (humidity_max < humidity) { humidity_max = humidity; }
                    if (humidity_min > humidity) { humidity_min = humidity; }
                    if (pressure_max < pressure) { pressure_max = pressure; }
                    if (pressure_min > pressure) { pressure_min = pressure; }
                    if (gas_registance_max < gas_registance) { gas_registance_max = gas_registance;  }
                    if (gas_registance_min > gas_registance) { gas_registance_min = gas_registance;  }
                    if (gas_registance_log_max < gas_registance_log) { gas_registance_log_max = gas_registance_log; }
                    if (gas_registance_log_min > gas_registance_log) { gas_registance_log_min = gas_registance_log; }
                    if (gas_registance_diff_max < gas_registance_diff) { gas_registance_diff_max = gas_registance_diff;  }
                    if (gas_registance_diff_min > gas_registance_diff) { gas_registance_diff_min = gas_registance_diff;  }
                }
                else
                {
                    // すでにデータが格納されていた...
                    //  (本来、ありえない状況のはずだが...)
                    Debug.WriteLine(DateTime.Now + " setBme688Data( " + index + " " + ") : already entries");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " setBme688Data(" + index + ") : " + e.Message);
            }
        }

        public Bme688Data getBme688Data(int index)
        {
            try
            {
                return (observedData[index]);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " getBme688Data(" + index + ") : " + e.Message);
            }

            // 異常発生時には、先頭のデータ（index:0）を返す
            return (observedData[0]);
        }
    }
}
