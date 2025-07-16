namespace SamplingBME688Serial
{
    public class HeaterProfileDataGrid
    {
        public List<HeaterProfileData> HeaterProfile { get; }

        public HeaterProfileDataGrid()
        {
            // ----- デフォルトのヒータープロファイルデータ
            HeaterProfile = new List<HeaterProfileData> {
                new HeaterProfileData { Name = "Temperature (degC)", Step0 = 200, Step1 = 200, Step2 = 200, Step3 = 200, Step4 = 200, Step5 = 200, Step6 = 200, Step7 = 200, Step8 = 200, Step9 = 200 },
                new HeaterProfileData { Name = "Hold Time (ms)", Step0 = 200, Step1 = 200, Step2 = 200, Step3 = 200, Step4 = 200, Step5 = 200, Step6 = 200, Step7 = 200, Step8 = 200, Step9 = 200 },
            };
        }
    }
}
