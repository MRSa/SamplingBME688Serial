namespace SamplingBME688Serial
{
    public class HeaterProfileDataGrid
    {
        public List<HeaterProfileData> HeaterProfile { get; }
        public String HeaterProfileName { get; set; }

        public int MeasurementDuration { get; set; }

        public HeaterProfileDataGrid()
        {
            // ----- ヒーターの温度保持時間 (ms)
            MeasurementDuration = 140;

            // ----- デフォルトのヒータープロファイルデータ
            HeaterProfile = new List<HeaterProfileData> {
                new HeaterProfileData { Name = "Temperature (degC)", Step0 = 200, Step1 = 200, Step2 = 200, Step3 = 200, Step4 = 200, Step5 = 200, Step6 = 200, Step7 = 200, Step8 = 200, Step9 = 200 },
                new HeaterProfileData { Name = "Hold Time (ms)", Step0 = 420, Step1 = 280, Step2 = 320, Step3 = 280, Step4 = 280, Step5 = 280, Step6 = 420, Step7 = 140, Step8 = 280, Step9 = 280 },
            };

            // ----- ヒータープロファイル名
            HeaterProfileName = new String("default");

        }
    }
}
