using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SamplingBME688Serial
{
    class TrainingKMeansModel : ICreatePredictionModel, IPredictionModel
    {
        private MLContext mlContext;
        private ICreateModelConsole console;
        private String inputDataFileName;
        private String featuresColumnName = "Features";
        private int numberOfClusters;

        private Microsoft.ML.Data.TransformerChain<Microsoft.ML.Data.ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters>> model;

        public TrainingKMeansModel(ref MLContext mlContext, String inputDataFileName, int nofClusters, ICreateModelConsole console)
        {
            this.mlContext = mlContext;
            this.inputDataFileName = inputDataFileName;
            this.numberOfClusters = nofClusters;
            this.console = console;
        }

        public Microsoft.ML.Data.TransformerChain<Microsoft.ML.Data.ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters>> getModel() { return model; }

        public bool executeTraining(SensorToUse usePort, String? outputFileName)
        {
            IDataView dataView;
            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters>> pipeline;
            try
            {
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

                console.appendText("executeTraining() : done.\r\n");
                return (true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] executeTraining() " + ex.Message);
            }
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
            return (clusterId.ToString());
        }
    }


}
