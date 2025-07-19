using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SamplingBME688Serial
{
    interface IHeaterProfileNotify
    {
        void notifyHeaterProfile(bool isSuccess, String heaterProfile);
        void abortReadHeaterProfile(bool isClear);
    }

    class SetSerialHeaterProfile
    {
        private System.IO.Ports.SerialPort mySerialPort = new System.IO.Ports.SerialPort(new System.ComponentModel.Container());
        private Thread? readThread = null;  // new Thread(ReadSerial);
        private bool _isReceiving = true;
        private IHeaterProfileNotify? callback = null;

        public SetSerialHeaterProfile()
        {

        }

        public bool getCurrentHeaterProfile(String comPort, IHeaterProfileNotify callback)
        {
            try
            {
                this.callback = callback;

                /*****/
                mySerialPort.BaudRate = 115200;
                mySerialPort.Parity = System.IO.Ports.Parity.None;
                mySerialPort.DataBits = 8;
                mySerialPort.StopBits = System.IO.Ports.StopBits.One;
                mySerialPort.Handshake = System.IO.Ports.Handshake.None;
                mySerialPort.PortName = comPort;
                mySerialPort.Open();
                /*****/

                if (mySerialPort.IsOpen)
                {
                    mySerialPort.WriteLine("CMD:GETPROF");
                }
                else
                {
                    throw new Exception("NOT OPEN COM PORT :" + comPort);
                }
                readThread = new Thread(ReadSerial);
                _isReceiving = true;
                readThread.Start();
                return (true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                String message = "=== Open ERROR (" + comPort + ") === : " + DateTime.Now + "\r\n";
                message = message + " " + e.Message + "\r\n" + e.StackTrace;
                callback.notifyHeaterProfile(false, message);
                callback.abortReadHeaterProfile(false);
                _isReceiving = false;
            }
            return (false);
        }

        public void stopReadHeaterProfile()
        {
            // ---- ヒータープロファイルの取得を強制停止させる
            if (callback != null)
            {
                callback.abortReadHeaterProfile(true);
            }
            _isReceiving = false;
        }

        public void ReadSerial()
        {
            Debug.WriteLine("  ----- START ReadSerial() ----- : " + DateTime.Now);
            while (_isReceiving)
            {
                try
                {
                    string receivedData = mySerialPort.ReadLine();
                    int startIndex = receivedData.IndexOf("{");
                    int finishIndex = receivedData.IndexOf("}");
                    if ((startIndex >= 0)&&(finishIndex >= 0))
                    {
                        // ----- ヒータープロファイルが読み出せた！ 終了する
                        string heaterProfile = receivedData.Substring(startIndex, (finishIndex - startIndex + 1));
                        if (callback != null)
                        {
                            callback.notifyHeaterProfile(true, heaterProfile);
                        }
                        _isReceiving = false;
                    }
                    else
                    {
                        if (callback != null)
                        {
                            callback.notifyHeaterProfile(false, receivedData);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(DateTime.Now + " ReadSerial()" + ") : " + e.Message);
                }
            }
            try
            {
                mySerialPort.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " ReadSerial() : Close " + e.Message);
            }
            Debug.WriteLine("  ----- FINISH ReadSerial() ----- : " + DateTime.Now);
            if (callback != null)
            {
                callback.abortReadHeaterProfile(false);
            }
        }
    }
}
