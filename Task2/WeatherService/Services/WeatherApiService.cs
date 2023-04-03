using NCrontab;
using RestSharp;
using System.Text.Json;
using WeatherService.Data;
using WeatherService.Repositories;

namespace WeatherService.Services
{
    public class WeatherApiService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private IWeatherRepository? _weatherRepository;
        private readonly CrontabSchedule? _schedule;
        private DateTime _nextRun;

        private static string Schedule => "*/1 * * * *";
        private static List<string> Cities => new() { "London", "Stockholm", "Copenhagen", "Riga", "Oslo", "Berlin", "Madrid", "Paris", "Rome", "Helsingfors" };

        public WeatherApiService(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = false });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_scopeFactory is null || _schedule is null)
                return;

            using (var scope = _scopeFactory.CreateScope())
            {
                _weatherRepository = scope.ServiceProvider.GetRequiredService<IWeatherRepository>();

                do
                {
                    var now = DateTime.Now;
                    if (now > _nextRun)
                    {
                        await FetchWeather(cancellationToken);
                        _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                    }
                    await Task.Delay(5000, cancellationToken); //5 seconds delay
                }
                while (!cancellationToken.IsCancellationRequested);
            }
        }

        public async Task FetchWeather(CancellationToken cancellationToken)
        {
            if (_weatherRepository is null)
                return;

            var client = new RestClient("http://api.weatherapi.com/v1");
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            foreach (var city in Cities)
            {
                var request = new RestRequest($"current.json?key={_configuration["WeatherApiKey"]}&q={city}&aqi=no");
                var response = await client.GetAsync(request, cancellationToken);
                if (response.IsSuccessful && response.Content is not null)
                {
                    var data = JsonSerializer.Deserialize<Weather>(response.Content, options);
                    if (data is not null)
                        await _weatherRepository.Add(data);
                }
            }
        }
    }
}
