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
                new HeaterProfileData { Name = "Temperature (degC)", Step0 = 320, Step1 = 100, Step2 = 100, Step3 = 100, Step4 = 200, Step5 = 200, Step6 = 200, Step7 = 320, Step8 = 320, Step9 = 320 },
                new HeaterProfileData { Name = "Hold Time (ms)", Step0 = 700, Step1 = 280, Step2 = 1400, Step3 = 4200, Step4 = 700, Step5 = 700, Step6 = 700, Step7 = 700, Step8 = 700, Step9 = 700 },
            };

            // ----- ヒータープロファイル名
            HeaterProfileName = new String("DEFAULT");

        }
    }
}
