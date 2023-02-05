using SerialCommBME688;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    internal class PostJsonData
    {
        private int maxIndexNumber;

        public PostJsonData(int maxIndex)
        {
            this.maxIndexNumber = maxIndex;
        }

        public void receivedData(
            String sendUrl,
            String category,
            int sensorId,
            int gas_index,
            int meas_index,
            Int64 serial_number,
            int data_status,
            int gas_wait,
            double temperature,
            double humidity,
            double pressure,
            double gas_registance,
            double gas_registance_log,
            double gas_registance_diff)
        {
            try
            {
                Debug.WriteLine(DateTime.Now + " ----- PostJsonData::receivedData(" + sendUrl + ") -----");
                Debug.WriteLine(DateTime.Now + "       " + category + " " + sensorId + " " + gas_index + " " + temperature);
                Debug.WriteLine(DateTime.Now + " ----- PostJsonData::receivedData  END -----");

            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " receivedData(" + sendUrl + " " + gas_index + ") : " + e.Message);
            }
        }

        private void postJson(String url)
        {
            // 受信したデータを　JSON形式で
            try
            {
                Thread postThread = new Thread(sendJson);

                postThread = new Thread(sendJson);
                postThread.Start();
                return;
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " postJson(" + url + ") : " + e.Message);
            }
        }

        private void sendJson()
        {
            //  データ登録本処理 (URLをたたく)
            try
            {

            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " Exception(" + " " + ") : " + e.Message);
            }
        }



    }
}
