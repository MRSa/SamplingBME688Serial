using System.Diagnostics;
using System.Text;

namespace SamplingBME688Serial
{
    class PredictAnalyzer : IReceivedSmellDataForAnalysis
    {
        private SensorToUse predictModelType = SensorToUse.None;
        private IPredictionModel? predictionModel = null;
        private bool withPresTempHum;
        private OdorOrData? analysisData01 = null;
        private OdorOrData? analysisData02 = null;
        private SmellOrData? analysisData11 = null;
        private SmellOrData? analysisData12 = null;
        private TextBox? fldResult1 = null;
        private TextBox? fldResult2 = null;
        private List<CheckResult> resultList = new List<CheckResult>();

        public void startPredict(SensorToUse predictModelType, IPredictionModel? predictionModel, bool withPresTempHum, ref TextBox fldResult1, ref TextBox fldResult2)
        {
            this.predictModelType = predictModelType;
            this.predictionModel = predictionModel;
            this.withPresTempHum = withPresTempHum;
            this.fldResult1 = fldResult1;
            this.fldResult2 = fldResult2;
            resultList.Clear();
            Debug.WriteLine(DateTime.Now + " = = = = startPredict() " + this.predictModelType + " = = = = =\r\n");
        }

        public void stopPredict()
        {
            predictModelType = SensorToUse.None;
            predictionModel = null;
            Debug.WriteLine(DateTime.Now + " = = = = = stopPredict() = = = = =\r\n");
        }

        public void receivedSmellDataForAnalysis(SmellOrData receivedData)
        {
            // ------ データを受信した。モデルタイプに合わせて予測処理を実行する
            Debug.WriteLine(DateTime.Now + "  receivedSmellDataForAnalysis() RX[" + receivedData.sensorId + "]");
            if (predictionModel == null)
            {
                // ---- モデルがない...終了する
                showResult(1, "-- no model --");
                showResult(2, "-- no model --");
                return;
            }

            try
            {
                if (withPresTempHum)
                {
                    receivedSmellDataForAnalysisImpl(receivedData);
                }
                else
                {
                    receivedOdorDataForAnalysisImpl(receivedData);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " showResult() : " + ex.Message + "  --- " + receivedData.sensorId);
            }
        }

        private void receivedSmellDataForAnalysisImpl(SmellOrData receivedData)
        {
            // ------ データを受信した。モデルタイプに合わせて予測処理を実行する
            try
            {
                Debug.WriteLine(DateTime.Now + " receivedSmellDataForAnalysisImpl()  " + 
                                "temp: " + receivedData.averageTemperatureValue + 
                                " pres: " + receivedData.averagePressureValue + 
                                " Hum: " + receivedData.averageHumidityValue +
                                " sensorId: " + receivedData.sensorId + " ");

                string result = "";
                switch (predictModelType)
                {
                    case SensorToUse.port1and2:
                        // ----- センサデータ２つを使う予測処理
                        result = receivedSmell1and2DataForAnalysis(receivedData);
                        break;
                    case SensorToUse.port1or2:
                        // ----- センサデータを1 or 2を使用する予測処理
                        result = receivedSmell1or2DataForAnalysis(receivedData);
                        break;
                    case SensorToUse.port1:
                    case SensorToUse.port2:
                        // ----- センサデータ１つしか使用しない予測処理
                        result = receivedSmellSingleDataForAnalysis(receivedData);
                        break;
                    case SensorToUse.None:
                    default:
                        // ---------- モデルタイプが異常なので、予測処理は実行しない
                        showResult(1, " unknown ");
                        showResult(2, " unknown ");
                        break;
                }
                if (result.Length > 0)
                {
                    resultList.Add(new CheckResult(result));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " showResult() : " + ex.Message + "  --- " + receivedData.sensorId);
            }
        }

        private void receivedOdorDataForAnalysisImpl(SmellOrData receivedData)
        {
            // ------ データを受信した。モデルタイプに合わせて予測処理を実行する
            try
            {
                string result = "";
                switch (predictModelType)
                {
                    case SensorToUse.port1and2:
                        // ----- センサデータ２つを使う予測処理
                        result = receivedOdor1and2DataForAnalysis(receivedData);
                        break;
                    case SensorToUse.port1or2:
                        // ----- センサデータを1 or 2を使用する予測処理
                        result = receivedOdor1or2DataForAnalysis(receivedData);
                        break;
                    case SensorToUse.port1:
                    case SensorToUse.port2:
                        // ----- センサデータ１つしか使用しない予測処理
                        result = receivedOdorSingleDataForAnalysis(receivedData);
                        break;
                    case SensorToUse.None:
                    default:
                        // ---------- モデルタイプが異常なので、予測処理は実行しない
                        showResult(1, " unknown ");
                        showResult(2, " unknown ");
                        break;
                }
                if (result.Length > 0)
                {
                    resultList.Add(new CheckResult(result));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " showResult(odor) : " + ex.Message + "  --- " + receivedData.sensorId);
            }
        }

        private string receivedSmell1and2DataForAnalysis(SmellOrData receivedData)
        {
            string result = "";

            // ----- sensor1 と sensor2 のデータを同時に使用して解析する
            Debug.WriteLine(DateTime.Now + "  receiveSmell1and2DataForAnalysis() RX[" + receivedData.sensorId + "]");

            if (receivedData.sensorId == 1.0f)
            {
                // ----- センサ１のデータを受信
                analysisData11 = new SmellOrData(receivedData);
                Debug.WriteLine(DateTime.Now + "  receiveSmell1and2DataForAnalysis() sensorId 1");
                showResult(1, "");
            }
            else if (receivedData.sensorId == 2.0f)
            {
                // ----- センサ２のデータを受信
                analysisData12 = new SmellOrData(receivedData);
                Debug.WriteLine(DateTime.Now + "  receiveSmell1and2DataForAnalysis() sensorId 2");
                showResult(1, "");
            }

            // -----ひとそろえのデータを受信した！ 解析する
            if ((analysisData11 != null) && (analysisData12 != null) && ((predictionModel != null)))
            {
                // ----- 解析を行う
                Debug.WriteLine(DateTime.Now + "  receiveSmell1and2DataForAnalysis() analysis Both");
                SmellBothData testData = new SmellBothData(analysisData11, analysisData12);
                result = predictionModel.predictBothData(testData);
                Debug.WriteLine(DateTime.Now + "  receiveSmell1and2DataForAnalysis() Result: " + result);

                // ----- 解析結果を表示する
                showResult(1, "---");
                showResult(1, result);

                // ---- 次のデータを待つために表示をクリアする
                analysisData11 = null;
                analysisData12 = null;
            }
            return result;
        }

        private string receivedSmell1or2DataForAnalysis(SmellOrData receivedData)
        {
            // ----- 受信したデータを使って予測を実行する
            Debug.WriteLine(DateTime.Now + "  receivedSmell1or2DataForAnalysis() RX[" + receivedData.sensorId + "]");
            int id = 1;
            string result = "";
            try
            {
                if (receivedData.sensorId == 1.0f)
                {
                    // ----- センサ１のデータを受信
                    id = 1;
                    showResult(id, "");
                }
                else if (receivedData.sensorId == 2.0f)
                {
                    // ----- センサ２のデータを受信
                    id = 2;
                    showResult(id, "");
                }
                else
                {
                    // ----- センサのIDが異常な場合...
                    showResult(1, "");
                    showResult(2, "");
                    Debug.WriteLine(DateTime.Now + "  receivedSmell1or2DataForAnalysis() ID: " + receivedData.sensorId + " (Wrong ID)");
                }
                if ((predictionModel != null) && (receivedData != null))
                {
                    // ----- 解析(データの予測)を行う
                    Debug.WriteLine(DateTime.Now + "  receivedSmell1or2DataForAnalysis() START");
                    result = predictionModel.predictOrData(receivedData);
                    Debug.WriteLine(DateTime.Now + "  receivedSmell1or2DataForAnalysis() Result: " + result);
                    showResult(id, result);
                }
            }
            catch (Exception ex)
            {
                // ---- 解析時にエラーが発生した...終了する
                Debug.WriteLine(DateTime.Now + "  receivedSmell1or2DataForAnalysis() " + ex.Message);
                showResult(id, "[ERROR]");
                result = "";
            }
            return result;
        }

        private string receivedSmellSingleDataForAnalysis(SmellOrData receivedData)
        {
            // ----- 受信したデータを使って予測を実行する
            Debug.WriteLine(DateTime.Now + "  receivedSmellSingleDataForAnalysis() RX[" + receivedData.sensorId + "]");
            int id = 1;
            string result = "";
            try
            {
                SmellData? smellData = null;
                if (receivedData.sensorId == 1.0f)
                {
                    // ----- センサ１のデータを受信
                    smellData = new SmellData(receivedData);
                    Debug.WriteLine(DateTime.Now + "  receivedSmellSingleDataForAnalysis() [1]");
                    id = 1;
                    showResult(id, "");
                }
                else if (receivedData.sensorId == 2.0f)
                {
                    // ----- センサ２のデータを受信
                    smellData = new SmellData(receivedData);
                    Debug.WriteLine(DateTime.Now + "  receivedSmellSingleDataForAnalysis() [2]");
                    id = 2;
                    showResult(id, "");
                }
                else
                {
                    // ----- センサのIDが異常な場合...
                    showResult(1, "");
                    showResult(2, "");
                    Debug.WriteLine(DateTime.Now + "  receivedSmellSingleDataForAnalysis() ID: " + receivedData.sensorId + " (Wrong ID)");
                }
                if ((predictionModel != null) && (smellData != null))
                {
                    // ----- 解析(データの予測)を行う
                    result = "";
                    Debug.WriteLine(DateTime.Now + "  receivedSmellSingleDataForAnalysis() START");
                    if (id == 1)
                    {
                        result = predictionModel.predictSingle1Data(smellData);
                    }
                    else
                    {
                        result = predictionModel.predictSingle2Data(smellData);
                    }
                    Debug.WriteLine(DateTime.Now + "  receivedSmellSingleDataForAnalysis() Result: " + result);
                    showResult(id, result);
                }
            }
            catch (Exception ex)
            {
                // ---- 解析時にエラーが発生した...終了する
                Debug.WriteLine(DateTime.Now + "  receivedSmellSingleDataForAnalysis() " + ex.Message);
                showResult(id, "[ERROR]");
                result = "";
            }
            return result;
        }

        private string receivedOdor1and2DataForAnalysis(SmellOrData receivedData)
        {
            string result = "";

            // ----- sensor1 と sensor2 のデータを同時に使用して解析する
            Debug.WriteLine(DateTime.Now + "  receivedOdor1and2DataForAnalysis() RX[" + receivedData.sensorId + "]");

            if (receivedData.sensorId == 1.0f)
            {
                // ----- センサ１のデータを受信
                analysisData01 = new OdorOrData(receivedData);
                Debug.WriteLine(DateTime.Now + "  receivedOdor1and2DataForAnalysis() sensorId 1");
                showResult(1, "");
            }
            else if (receivedData.sensorId == 2.0f)
            {
                // ----- センサ２のデータを受信
                analysisData02 = new OdorOrData(receivedData);
                Debug.WriteLine(DateTime.Now + "  receivedOdor1and2DataForAnalysis() sensorId 2");
                showResult(1, "");
            }

            // -----ひとそろえのデータを受信した！ 解析する
            if ((analysisData01 != null) && (analysisData02 != null) && ((predictionModel != null)))
            {
                // ----- 解析を行う
                Debug.WriteLine(DateTime.Now + "  receivedOdor1and2DataForAnalysis() analysis Both");
                OdorBothData testData = new OdorBothData(analysisData01, analysisData02);
                result = predictionModel.predictBothData(testData);
                Debug.WriteLine(DateTime.Now + "  receivedOdor1and2DataForAnalysis() Result: " + result);

                // ----- 解析結果を表示する
                showResult(1, "---");
                showResult(1, result);

                // ---- 次のデータを待つために表示をクリアする
                analysisData01 = null;
                analysisData02 = null;
            }
            return result;
        }

        private string receivedOdor1or2DataForAnalysis(SmellOrData receivedData)
        {
            // ----- 受信したデータを使って予測を実行する
            Debug.WriteLine(DateTime.Now + "  receivedOdor1or2DataForAnalysis() RX[" + receivedData.sensorId + "]");
            int id = 1;
            string result = "";
            try
            {
                if (receivedData.sensorId == 1.0f)
                {
                    // ----- センサ１のデータを受信
                    id = 1;
                    showResult(id, "");
                }
                else if (receivedData.sensorId == 2.0f)
                {
                    // ----- センサ２のデータを受信
                    id = 2;
                    showResult(id, "");
                }
                else
                {
                    // ----- センサのIDが異常な場合...
                    showResult(1, "");
                    showResult(2, "");
                    Debug.WriteLine(DateTime.Now + "  receivedOdor1or2DataForAnalysis() ID: " + receivedData.sensorId + " (Wrong ID)");
                }
                if ((predictionModel != null) && (receivedData != null))
                {
                    // ----- 解析(データの予測)を行う
                    OdorOrData odorOrData = new OdorOrData(receivedData);
                    Debug.WriteLine(DateTime.Now + "  receivedOdor1or2DataForAnalysis() START");
                    result = predictionModel.predictOrData(odorOrData);
                    Debug.WriteLine(DateTime.Now + "  receivedOdor1or2DataForAnalysis() Result: " + result);
                    showResult(id, result);
                }
            }
            catch (Exception ex)
            {
                // ---- 解析時にエラーが発生した...終了する
                Debug.WriteLine(DateTime.Now + "  receivedOdor1or2DataForAnalysis() " + ex.Message);
                showResult(id, "[ERROR]");
                result = "";
            }
            return result;
        }

        private string receivedOdorSingleDataForAnalysis(SmellOrData receivedData)
        {
            // ----- 受信したデータを使って予測を実行する
            Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() RX[" + receivedData.sensorId + "]");
            int id = 1;
            string result = "";
            try
            {
                OdorData? odorData = null;
                if (receivedData.sensorId == 1.0f)
                {
                    // ----- センサ１のデータを受信
                    odorData = new OdorData(receivedData);
                    Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() [1]");
                    id = 1;
                    showResult(id, "");
                }
                else if (receivedData.sensorId == 2.0f)
                {
                    // ----- センサ２のデータを受信
                    odorData = new OdorData(receivedData);
                    Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() [2]");
                    id = 2;
                    showResult(id, "");
                }
                else
                {
                    // ----- センサのIDが異常な場合...
                    showResult(1, "");
                    showResult(2, "");
                    Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() ID: " + receivedData.sensorId + " (Wrong ID)");
                }
                if ((predictionModel != null) && (odorData != null))
                {
                    // ----- 解析(データの予測)を行う
                    result = "";
                    Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() START");
                    if (id == 1)
                    {
                        result = predictionModel.predictSingle1Data(odorData);
                    }
                    else
                    {
                        result = predictionModel.predictSingle2Data(odorData);
                    }
                    Debug.WriteLine(DateTime.Now + "  receivedOdorSingleDataForAnalysis() Result: " + result);
                    showResult(id, result);
                }
            }
            catch (Exception ex)
            {
                // ---- 解析時にエラーが発生した...終了する
                Debug.WriteLine(DateTime.Now + "  receivedSmellSingleDataForAnalysis() " + ex.Message);
                showResult(id, "[ERROR]");
                result = "";
            }
            return result;
        }

        private void showResult(int area, string itemData)
        {
            TextBox? field = (area == 1) ? fldResult1 : fldResult2;
            try
            {
                if ((field != null) && (itemData.Length > 0))
                {
                    if (field.InvokeRequired)
                    {
                        Action safeWrite = delegate { field.Clear(); field.AppendText(itemData); };
                        field.Invoke(safeWrite);
                    }
                    else
                    {
                        field.Clear();
                        field.AppendText(itemData);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " showResult() : " + e.Message + "  --- " + itemData);
            }
        }

        public int getResultCount()
        {
            return resultList.Count;
        }

        public int exportResultList(string outputFileName)
        {
            if (resultList.Count <= 0)
            {
                // -----  データが入っていないので、エラー応答(負の値を応答)する
                Debug.WriteLine(DateTime.Now + " [ERROR] exportResultList() : " + outputFileName + "  result data is nothing. ");
                return -1;
            }

            int index = 1;
            using (StreamWriter writer = new StreamWriter(outputFileName, false, Encoding.UTF8))
            {
                writer.WriteLine("index,time,result");
                string oneLine = "";
                foreach (CheckResult result in resultList)
                {
                    oneLine = index + "," + result.resultLabel + "," + result.sampleTime.ToString();
                    writer.WriteLine(oneLine);
                    index++;
                }
            }

            // 出力データの個数を返す
            Debug.WriteLine(DateTime.Now + " [INFO] exportResultList() : "  +  index + "/" + resultList.Count + " " + outputFileName);
            return resultList.Count;
        }
    }
}
