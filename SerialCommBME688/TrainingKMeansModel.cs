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

        private Microsoft.ML.Data.TransformerChain<Microsoft.ML.Data.ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters>> model;

        public TrainingKMeansModel(ref MLContext mlContext, String inputDataFileName, int nofClusters, ICreateModelConsole console)
        {
            this.mlContext = mlContext;
            this.inputDataFileName = inputDataFileName;
            this.numberOfClusters = nofClusters;
            this.console = console;
        }

        public Microsoft.ML.Data.TransformerChain<Microsoft.ML.Data.ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters>> getModel() { return model; }

        public bool executeTraining(SensorToUse usePort, String? outputFileName, ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData)
        {
            IDataView dataView;
            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters>> pipeline;
            try
            {
                Debug.WriteLine(DateTime.Now + " ---------- executeTraining() START  ----------");
                console.appendText("executeTraining() : " + outputFileName + "\r\n");

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
                    dataView = mlContext.Data.LoadFromTextFile<OdorOrData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                    pipeline = mlContext.Transforms.Concatenate(featuresColumnName, "sensorId",
                                                                "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                                "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value")
                                                    .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: numberOfClusters));
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
                decideBothDataLabel(ref port1, ref port2, isLogData);

                console.appendText("executeTraining() : done.\r\n");
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
                Debug.WriteLine(DateTime.Now + " [ERROR] executeTraining() " + ex.Message);
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
                    // Ristretto
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

                Debug.WriteLine(DateTime.Now + " [ERROR] getBothData() rack data. " + ee.Message);
                return (null);
            }
        }

        private void decideBothDataLabel(ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData)
        {
            labelHolders.Clear();
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
                    List<List<GraphDataValue>> item1data = item1.Value;
                    List<List<GraphDataValue>> item2data = dataSetMap2[item1.Key];

                    int sample1count = item1data.Count / 2;
                    int sample2count = item2data.Count / 2;

                    List<GraphDataValue> sample1data = item1data[sample1count];
                    List<GraphDataValue> sample2data = item1data[sample2count];
                    OdorBothData? targetData = getBothData(sample1data, sample2data, isLogData);
                    if (targetData == null)
                    {
                        //  --- データ取得失敗...ラベルを作らない
                        Debug.WriteLine(DateTime.Now + " data get failure. (" + sample1count + ") (" + sample2count + ")");
                    }
                    else
                    {
                        var predictor0 = mlContext.Model.CreatePredictionEngine<OdorBothData, ClusterPrediction>(model);
                        var prediction0 = predictor0.Predict(targetData);

                        DataLabelHolder dataLabel = new DataLabelHolder(prediction0.PredictedClusterId, item1.Key);
                        labelHolders.Add(dataLabel);
                        Debug.WriteLine(DateTime.Now + " data label (" + dataLabel.clusterNo + ") (" + dataLabel.clusterName + ")");

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] executeTraining() " + ex.Message);
                labelHolders.Clear();
            }
        }
    }


}
