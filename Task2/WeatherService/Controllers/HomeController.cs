using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherService.Models;
using WeatherService.Repositories;

namespace WeatherService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherRepository _weatherRepository;

        public HomeController(ILogger<HomeController> logger, IWeatherRepository weatherRepository)
        {
            _logger = logger;
            _weatherRepository = weatherRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<JsonResult> GetLastupdated()
        {
            var lastupdated = await _weatherRepository.GetLastUpdated();
            return Json(lastupdated);
        }

        public async Task<JsonResult> GetTemperature()
        {
            var temperatures = await _weatherRepository.GetTemperatures();
            return Json(temperatures);
        }

        public async Task<JsonResult> GetWind()
        {
            var wind = await _weatherRepository.GetWind();
            return Json(wind);
        }
    }
}