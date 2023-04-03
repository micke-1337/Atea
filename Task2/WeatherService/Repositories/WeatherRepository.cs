using Microsoft.EntityFrameworkCore;
using WeatherService.Data;

namespace WeatherService.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly double MeterPerSecond = 0.277778;

        public async Task Add(Weather weatherData)
        {
            using (var context = new ApplicationDbContext())
            {
                //await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                weatherData.Id = Guid.NewGuid();
                weatherData.PartitionKey = "1";
                context.Add(weatherData);

                await context.SaveChangesAsync();
            }
        }

        public async Task<Lastupdated> GetLastUpdated()
        {
            using (var context = new ApplicationDbContext())
            {
                var lastUpdated = await context.Weather.Select(g => g.current.last_updated).Distinct().ToListAsync();
                var result = new Lastupdated { last_updated = lastUpdated };

                return result;
            }
        }

        public async Task<List<City>> GetTemperatures()
        {
            using (var context = new ApplicationDbContext())
            {
                var locations = await context.Weather.Select(g => g.location.name).Distinct().ToListAsync();
                var result = new List<City>();
                foreach (var location in locations)
                {
                    var temperature = await context.Weather.Where(g => g.location.name == location).Select(g => g.current.temp_c).ToListAsync();
                    var city = new City { name = location, data = temperature };
                    result.Add(city);
                }

                return result;
            }
        }

        public async Task<List<City>> GetWind()
        {
            using (var context = new ApplicationDbContext())
            {
                var locations = await context.Weather.Select(g => g.location.name).Distinct().ToListAsync();
                var result = new List<City>();
                foreach (var location in locations)
                {
                    var wind = await context.Weather.Where(g => g.location.name == location).Select(g => (float)Math.Round(g.current.wind_kph * MeterPerSecond, 1)).ToListAsync();
                    var city = new City { name = location, data = wind };
                    result.Add(city);
                }

                return result;
            }
        }
    }
}
