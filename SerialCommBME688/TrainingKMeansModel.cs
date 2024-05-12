using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SamplingBME688Serial
{

    internal class DataLabelHolder
    {
        public uint clusterNo { get; private set; }
        public String clusterName { get; private set; }

        public DataLabelHolder(uint clusterNo, String clusterName)
        {
            this.clusterNo = clusterNo;
            this.clusterName = clusterName;
        }
    }

    class TrainingKMeansModel : ICreatePredictionModel, IPredictionModel
    {
        private MLContext mlContext;
        private ICreateModelConsole console;
        private String inputDataFileName;
        private String featuresColumnName = "Features";
        private int numberOfClusters;
        private List<DataLabelHolder> labelHolders = new List<DataLabelHolder>();
        private bool doneTraining = false;
        private Microsoft.ML.Data.TransformerChain<Microsoft.ML.Data.ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters>>? model = null;

        public TrainingKMeansModel(ref MLContext mlContext, String inputDataFileName, int nofClusters, ICreateModelConsole console)
        {
            this.mlContext = mlContext;
            this.inputDataFileName = inputDataFileName;
            this.numberOfClusters = nofClusters;
            this.console = console;
        }

        // public Microsoft.ML.Data.TransformerChain<Microsoft.ML.Data.ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters>>? getModel() { return model; }

        public bool executeTraining(SensorToUse usePort, String? outputFileName, ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData)
        {
            IDataView dataView;
            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters>> pipeline;
            try
            {
                Debug.WriteLine(DateTime.Now + " ---------- executeTraining() START " + "K-Means" + " ----------");
                console.appendText("executeTraining() K-Means cluster: " + numberOfClusters + " Output File: " + outputFileName + "\r\n");

                // ----- データの読み込みの設定
                if (usePort == SensorToUse.port1and2)
                {
                    dataView = mlContext.Data.LoadFromTextFile<OdorBothData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                    pipeline = mlContext.Transforms.Concatenate(featuresColumnName,
                                                                "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                                "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value",
                                                                "sequence10Value", "sequence11Value", "sequence12Value", "sequence13Value", "sequence14Value",
                                                                "sequence15Value", "sequence16Value", "sequence17Value", "sequence18Value", "sequence19Value")
                                                    .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: numberOfClusters));
                }
                else if (usePort == SensorToUse.port1)
                {
                    dataView = mlContext.Data.LoadFromTextFile<OdorData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                    pipeline = mlContext.Transforms.Concatenate(featuresColumnName,
                                                                "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                                "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value")
                                                    .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: numberOfClusters));
                }
                else if (usePort == SensorToUse.port2)
                {
                    dataView = mlContext.Data.LoadFromTextFile<OdorData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                    pipeline = mlContext.Transforms.Concatenate(featuresColumnName,
                                                                "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                                "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value")
                                                    .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: numberOfClusters));
                }
                else // if (usePort == SensorToUse.port1or2)
                {
                    int clusters = numberOfClusters + numberOfClusters;
                    dataView = mlContext.Data.LoadFromTextFile<OdorOrData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                    pipeline = mlContext.Transforms.Concatenate(featuresColumnName, "sensorId",
                                                                "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                                "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value")
                                                    .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: clusters));
                }

                // ----- 学習パイプラインを作成する
                model = pipeline.Fit(dataView);

                // ----- モデルを保存する
                if (outputFileName != null)
                {
                    using (var fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        mlContext.Model.Save(model, dataView.Schema, fileStream);
                    }
                }
                doneTraining = true;

                // ----- 分類したクラスタにラベルを割り付ける
                labelHolders.Clear();
                console.appendText("CREATE LABEL\r\n");
                switch (usePort)
                {
                    case SensorToUse.port1and2:
                        // ----- 両方のポートを同時に使用する場合
                        decideBothDataLabel(ref port1, ref port2, isLogData);
                        break;
                    case SensorToUse.port1or2:
                        // ----- 両方のポートを片方づつ使用する場合
                        decideOrDataLabel(ref port1, ref port2, isLogData);
                        break;
                    case SensorToUse.port1:
                        // ----- ポート１のみ使用する場合
                        decideSingleDataLabel(ref port1, isLogData);
                        break;
                    case SensorToUse.port2:
                        // ----- ポート２のみ使用する場合
                        decideSingleDataLabel(ref port2, isLogData);
                        break;
                    case SensorToUse.None:
                    default:
                        // ----- 使用ポートが不明...ラベルなしとする
                        console.appendText("  UNKNOWN sensor Type : LABEL is NONE.\r\n");
                        break;
                }
                console.appendText(" ----- executeTraining() : done.\r\n");
                Debug.WriteLine(DateTime.Now + " ---------- executeTraining() END  ----------");
                return (true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] executeTraining() " + ex.Message);
            }
            doneTraining = false;
            return (false);
        }

        public String predictBothData(OdorBothData targetData)
        {
            uint clusterId = uint.MaxValue;
            try
            {
                var predictor = mlContext.Model.CreatePredictionEngine<OdorBothData, ClusterPrediction>(model);
                var prediction = predictor.Predict(targetData);

                clusterId = prediction.PredictedClusterId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] predictBothData() " + ex.Message);
            }

            // ---- データラベルを探す..
            foreach (DataLabelHolder label in labelHolders)
            {
                if (label.clusterNo == clusterId)
                {
                    return (label.clusterName);
                }
            }
            return (clusterId.ToString());
        }

        public String predictOrData(OdorOrData targetData)
        {
            uint clusterId = uint.MaxValue;
            try
            {
                var predictor = mlContext.Model.CreatePredictionEngine<OdorOrData, ClusterPrediction>(model);
                var prediction = predictor.Predict(targetData);

                clusterId = prediction.PredictedClusterId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] predictOrData() " + ex.Message);
            }

            // データラベルを探す..
            foreach (DataLabelHolder label in labelHolders)
            {
                if (label.clusterNo == clusterId)
                {
                    return (label.clusterName);
                }
            }
            return (clusterId.ToString());
        }

        public String predictSingleData(OdorData targetData)
        {
            uint clusterId = uint.MaxValue;
            try
            {
                var predictor = mlContext.Model.CreatePredictionEngine<OdorData, ClusterPrediction>(model);
                var prediction = predictor.Predict(targetData);
                clusterId = prediction.PredictedClusterId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] predictSingleData() " + ex.Message);
            }

            // データラベルを探す..
            foreach (DataLabelHolder label in labelHolders)
            {
                if (label.clusterNo == clusterId)
                {
                    return (label.clusterName);
                }
            }
            return (clusterId.ToString());
        }

        private OdorBothData? getBothData(List<GraphDataValue> port1Data, List<GraphDataValue> port2Data, bool isLogData)
        {
            try
            {
                OdorBothData bothData = new OdorBothData
                {
                    sequence0Value = (float)((isLogData) ? port1Data[0].gas_registance_log : port1Data[0].gas_registance),
                    sequence1Value = (float)((isLogData) ? port1Data[1].gas_registance_log : port1Data[1].gas_registance),
                    sequence2Value = (float)((isLogData) ? port1Data[2].gas_registance_log : port1Data[2].gas_registance),
                    sequence3Value = (float)((isLogData) ? port1Data[3].gas_registance_log : port1Data[3].gas_registance),
                    sequence4Value = (float)((isLogData) ? port1Data[4].gas_registance_log : port1Data[4].gas_registance),
                    sequence5Value = (float)((isLogData) ? port1Data[5].gas_registance_log : port1Data[5].gas_registance),
                    sequence6Value = (float)((isLogData) ? port1Data[6].gas_registance_log : port1Data[6].gas_registance),
                    sequence7Value = (float)((isLogData) ? port1Data[7].gas_registance_log : port1Data[7].gas_registance),
                    sequence8Value = (float)((isLogData) ? port1Data[8].gas_registance_log : port1Data[8].gas_registance),
                    sequence9Value = (float)((isLogData) ? port1Data[9].gas_registance_log : port1Data[9].gas_registance),
                    sequence10Value = (float)((isLogData) ? port2Data[0].gas_registance_log : port2Data[0].gas_registance),
                    sequence11Value = (float)((isLogData) ? port2Data[1].gas_registance_log : port2Data[1].gas_registance),
                    sequence12Value = (float)((isLogData) ? port2Data[2].gas_registance_log : port2Data[2].gas_registance),
                    sequence13Value = (float)((isLogData) ? port2Data[3].gas_registance_log : port2Data[3].gas_registance),
                    sequence14Value = (float)((isLogData) ? port2Data[4].gas_registance_log : port2Data[4].gas_registance),
                    sequence15Value = (float)((isLogData) ? port2Data[5].gas_registance_log : port2Data[5].gas_registance),
                    sequence16Value = (float)((isLogData) ? port2Data[6].gas_registance_log : port2Data[6].gas_registance),
                    sequence17Value = (float)((isLogData) ? port2Data[7].gas_registance_log : port2Data[7].gas_registance),
                    sequence18Value = (float)((isLogData) ? port2Data[8].gas_registance_log : port2Data[8].gas_registance),
                    sequence19Value = (float)((isLogData) ? port2Data[9].gas_registance_log : port2Data[9].gas_registance)
                };
                return (bothData);
            }
            catch (Exception ee)
            {

                Debug.WriteLine(DateTime.Now + " [ERROR] getBothData() : " + ee.Message);
                return (null);
            }
        }

        private OdorData? getSingleData(List<GraphDataValue> data, bool isLogData)
        {
            try
            {
                OdorData odorData = new OdorData
                {
                    sequence0Value = (float)((isLogData) ? data[0].gas_registance_log : data[0].gas_registance),
                    sequence1Value = (float)((isLogData) ? data[1].gas_registance_log : data[1].gas_registance),
                    sequence2Value = (float)((isLogData) ? data[2].gas_registance_log : data[2].gas_registance),
                    sequence3Value = (float)((isLogData) ? data[3].gas_registance_log : data[3].gas_registance),
                    sequence4Value = (float)((isLogData) ? data[4].gas_registance_log : data[4].gas_registance),
                    sequence5Value = (float)((isLogData) ? data[5].gas_registance_log : data[5].gas_registance),
                    sequence6Value = (float)((isLogData) ? data[6].gas_registance_log : data[6].gas_registance),
                    sequence7Value = (float)((isLogData) ? data[7].gas_registance_log : data[7].gas_registance),
                    sequence8Value = (float)((isLogData) ? data[8].gas_registance_log : data[8].gas_registance),
                    sequence9Value = (float)((isLogData) ? data[9].gas_registance_log : data[9].gas_registance)
                };
                return (odorData);
            }
            catch (Exception ee)
            {

                Debug.WriteLine(DateTime.Now + " [ERROR] getSingleData() : " + ee.Message);
                return (null);
            }
        }

        private OdorOrData? getOrData(int port, List<GraphDataValue> data, bool isLogData)
        {
            try
            {
                OdorOrData odorData = new OdorOrData
                {
                    sensorId = port,
                    sequence0Value = (float)((isLogData) ? data[0].gas_registance_log : data[0].gas_registance),
                    sequence1Value = (float)((isLogData) ? data[1].gas_registance_log : data[1].gas_registance),
                    sequence2Value = (float)((isLogData) ? data[2].gas_registance_log : data[2].gas_registance),
                    sequence3Value = (float)((isLogData) ? data[3].gas_registance_log : data[3].gas_registance),
                    sequence4Value = (float)((isLogData) ? data[4].gas_registance_log : data[4].gas_registance),
                    sequence5Value = (float)((isLogData) ? data[5].gas_registance_log : data[5].gas_registance),
                    sequence6Value = (float)((isLogData) ? data[6].gas_registance_log : data[6].gas_registance),
                    sequence7Value = (float)((isLogData) ? data[7].gas_registance_log : data[7].gas_registance),
                    sequence8Value = (float)((isLogData) ? data[8].gas_registance_log : data[8].gas_registance),
                    sequence9Value = (float)((isLogData) ? data[9].gas_registance_log : data[9].gas_registance)
                };
                return (odorData);
            }
            catch (Exception ee)
            {

                Debug.WriteLine(DateTime.Now + " [ERROR] getOrData() : [" + port + "] " + ee.Message);
                return (null);
            }
        }

        private void decideBothDataLabel(ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData)
        {
            if ((!doneTraining)||(port1 == null)||(port2 == null))
            {
                // モデル作成が行われていない...終了する
                return;
            }

            try
            {
                // ----- クラスタ番号から、サンプルデータのデータラベルを決定する
                Dictionary<String, List<List<GraphDataValue>>> dataSetMap1 = port1.getGasRegDataSet();
                Dictionary<String, List<List<GraphDataValue>>> dataSetMap2 = port2.getGasRegDataSet();

                foreach (KeyValuePair<String, List<List<GraphDataValue>>> item1 in dataSetMap1)
                {
                    // --------- ラベルデータの初期化
                    uint[] clusterLabel = new uint[dataSetMap1.Count + 1];
                    for (int i = 0; i <= dataSetMap1.Count; i++)
                    {
                        clusterLabel[i] = 0;
                    }

                    List<List<GraphDataValue>> item1data = item1.Value;
                    List<List<GraphDataValue>> item2data = dataSetMap2[item1.Key];

                    int limit = Math.Min(item1data.Count, item2data.Count);
                    //for (int index = 0; index < limit; index += 2)
                    for (int index = 0; index < limit; index ++)
                    {
                        List<GraphDataValue> sample1data = item1data[index];
                        List<GraphDataValue> sample2data = item2data[index];
                        OdorBothData? targetData = getBothData(sample1data, sample2data, isLogData);
                        if (targetData == null)
                        {
                            //  --- データ取得失敗...ラベルを作らない
                            Debug.WriteLine(DateTime.Now + " data get failure. [" + index + "]");
                        }
                        else
                        {
                            var predictor = mlContext.Model.CreatePredictionEngine<OdorBothData, ClusterPrediction>(model);
                            var prediction = predictor.Predict(targetData);
                            if (prediction != null)
                            {
                                uint labelIndex = prediction.PredictedClusterId;
                                clusterLabel[labelIndex]++;
                            }
                        }
                    }

                    // ---------- ラベルを決める
                    uint maxLabelCount = 0;
                    uint maxLabelId = 0;
                    for (uint i = 1; i <= dataSetMap1.Count; i++)
                    {
                        if (maxLabelCount < clusterLabel[i])
                        {
                            maxLabelId = i;
                            maxLabelCount = clusterLabel[i];
                        }
                    }

                    uint totalCount = 0;
                    for (int i = 0; i <= dataSetMap1.Count; i++)
                    {
                        Debug.WriteLine(DateTime.Now + " " + item1.Key + " (" + i + ") " + clusterLabel[i]);
                        totalCount += clusterLabel[i];
                        clusterLabel[i] = 0;
                    }

                    DataLabelHolder dataLabel = new DataLabelHolder(maxLabelId, item1.Key);
                    labelHolders.Add(dataLabel);
                    Debug.WriteLine(DateTime.Now + " data label (" + dataLabel.clusterNo + ") (" + dataLabel.clusterName + ")" + " [" + item1.Key + "] " + maxLabelCount + "/" + limit + " " + ((float) maxLabelCount) / ((float) limit));
                    console.appendText(" [" + maxLabelId + "] " + " LABEL: " + item1.Key + "(" + maxLabelCount + "/" + totalCount + ") " + ((int) ((float)maxLabelCount / (float) totalCount * 100.0f)) + " % \r\n");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] decideBothDataLabel() " + ex.Message);
                labelHolders.Clear();
            }
        }


        private void decideOrDataLabel(ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData)
        {
            if ((!doneTraining) || (port1 == null) || (port2 == null))
            {
                // モデル作成が行われていない...終了する
                return;
            }

            try
            {
                // ----- クラスタ番号から、サンプルデータのデータラベルを決定する
                Dictionary<String, List<List<GraphDataValue>>> dataSetMap1 = port1.getGasRegDataSet();
                Dictionary<String, List<List<GraphDataValue>>> dataSetMap2 = port2.getGasRegDataSet();

                int nofLabels = dataSetMap1.Count + dataSetMap2.Count;
                decideOrLabels(1, nofLabels, ref dataSetMap1, isLogData);
                decideOrLabels(2, nofLabels, ref dataSetMap2, isLogData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] decideOrDataLabel() " + ex.Message);
                labelHolders.Clear();
            }
        }

        private void decideOrLabels(int portNumber, int nofLabels, ref Dictionary<String, List<List<GraphDataValue>>> dataSetMap, bool isLogData)
        {
            try
            {
                foreach (KeyValuePair<String, List<List<GraphDataValue>>> item in dataSetMap)
                {
                    // --------- ラベルデータの初期化
                    uint[] clusterLabel = new uint[nofLabels + 1];
                    for (int i = 0; i <= nofLabels; i++)
                    {
                        clusterLabel[i] = 0;
                    }
                    List<List<GraphDataValue>> itemData = item.Value;
                    for (int index = 0; index < itemData.Count; index += 4)
                    {
                        List<GraphDataValue> sampleData = itemData[index];
                        OdorOrData? targetData = getOrData(1, sampleData, isLogData);
                        if (targetData == null)
                        {
                            //  --- データ取得失敗...
                            Debug.WriteLine(DateTime.Now + " data get failure. [" + index + "]");
                        }
                        else
                        {
                            var predictor = mlContext.Model.CreatePredictionEngine<OdorOrData, ClusterPrediction>(model);
                            var prediction = predictor.Predict(targetData);
                            if (prediction != null)
                            {
                                uint labelIndex = prediction.PredictedClusterId;
                                clusterLabel[labelIndex]++;
                            }
                        }
                    }

                    // ---------- ラベルを決める
                    uint maxLabelCount = 0;
                    uint maxLabelId = 0;
                    for (uint i = 1; i <= nofLabels; i++)
                    {
                        if (maxLabelCount < clusterLabel[i])
                        {
                            maxLabelId = i;
                            maxLabelCount = clusterLabel[i];
                        }
                    }

                    uint totalCount = 0;
                    for (int i = 0; i <= nofLabels; i++)
                    {
                        Debug.WriteLine(DateTime.Now + " " + item.Key + " (" + i + ") " + clusterLabel[i]);
                        totalCount += clusterLabel[i];
                        clusterLabel[i] = 0;
                    }

                    DataLabelHolder dataLabel = new DataLabelHolder(maxLabelId, item.Key + "(" + portNumber + ")");
                    labelHolders.Add(dataLabel);
                    Debug.WriteLine(DateTime.Now + " data label (" + dataLabel.clusterNo + ") (" + dataLabel.clusterName + ")" + " [" + item.Key + "] " + maxLabelCount + "/" + itemData.Count + " " + ((float)maxLabelCount) / ((float)itemData.Count));
                    console.appendText(" [" + maxLabelId + "] " + " LABEL: " + item.Key + "(" + maxLabelCount + "/" + totalCount + ") " + ((int)((float)maxLabelCount / (float)totalCount * 100.0f)) + " % \r\n");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] decideOrLabels() " + ex.Message);
                labelHolders.Clear();
            }
        }

        private void decideSingleDataLabel(ref IDataHolder? port, bool isLogData)
        {
            if ((!doneTraining) || (port == null))
            {
                // モデル作成が行われていない...終了する
                return;
            }

            try
            {
                // ----- クラスタ番号から、サンプルデータのデータラベルを決定する
                Dictionary<String, List<List<GraphDataValue>>> dataSetMap = port.getGasRegDataSet();
                foreach (KeyValuePair<String, List<List<GraphDataValue>>> item in dataSetMap)
                {
                    // --------- ラベルデータの初期化
                    uint[] clusterLabel = new uint[dataSetMap.Count + 1];
                    for (int i = 0; i <= dataSetMap.Count; i++)
                    {
                        clusterLabel[i] = 0;
                    }

                    List<List<GraphDataValue>> itemData = item.Value;
                    for (int index = 0; index < itemData.Count; index += 4)
                    {
                        List<GraphDataValue> sampleData = itemData[index];
                        OdorData? targetData = getSingleData(sampleData, isLogData);
                        if (targetData == null)
                        {
                            //  --- データ取得失敗...
                            Debug.WriteLine(DateTime.Now + " data get failure. [" + index + "]");
                        }
                        else
                        {
                            var predictor = mlContext.Model.CreatePredictionEngine<OdorData, ClusterPrediction>(model);
                            var prediction = predictor.Predict(targetData);
                            if (prediction != null)
                            {
                                uint labelIndex = prediction.PredictedClusterId;
                                clusterLabel[labelIndex]++;
                            }
                        }
                    }
                    // ---------- ラベルを決める
                    uint maxLabelCount = 0;
                    uint maxLabelId = 0;
                    for (uint i = 1; i <= dataSetMap.Count; i++)
                    {
                        if (maxLabelCount < clusterLabel[i])
                        {
                            maxLabelId = i;
                            maxLabelCount = clusterLabel[i];
                        }
                    }

                    uint totalCount = 0;
                    for (int i = 0; i <= dataSetMap.Count; i++)
                    {
                        Debug.WriteLine(DateTime.Now + " " + item.Key + " (" + i + ") " + clusterLabel[i]);
                        totalCount += clusterLabel[i];
                        clusterLabel[i] = 0;
                    }

                    DataLabelHolder dataLabel = new DataLabelHolder(maxLabelId, item.Key);
                    labelHolders.Add(dataLabel);
                    Debug.WriteLine(DateTime.Now + " data label (" + dataLabel.clusterNo + ") (" + dataLabel.clusterName + ")" + " [" + item.Key + "] " + maxLabelCount + "/" + itemData.Count + " " + ((float)maxLabelCount) / ((float)itemData.Count));
                    console.appendText(" [" + maxLabelId + "] " + " LABEL: " + item.Key + "(" + maxLabelCount + "/" + totalCount + ")  " + ((int)((float)maxLabelCount / (float)totalCount * 100.0f)) + " % \r\n");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] decideSingleDataLabel() " + ex.Message);
                labelHolders.Clear();
            }
        }
    }
}
