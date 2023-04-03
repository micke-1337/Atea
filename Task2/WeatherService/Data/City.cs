namespace WeatherService.Data
{
    public class City
    {
        public string name { get; set; } = default!;

        public List<float> data { get; set; } = default!;
    }
}
