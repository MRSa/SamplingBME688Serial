namespace SamplingBME688Serial
{
    public class HeaterProfileJson
    {
        public string name {  get; set; }
        public List<int> tempProf { get; set; }
        public List<int> holdProf { get; set; }
    }
}
