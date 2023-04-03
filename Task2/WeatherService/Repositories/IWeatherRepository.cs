using WeatherService.Data;

namespace WeatherService.Repositories
{
    public interface IWeatherRepository
    {
        Task Add(Weather weatherData);
        Task<Lastupdated> GetLastUpdated();
        Task<List<City>> GetTemperatures();
        Task<List<City>> GetWind();
    }
}