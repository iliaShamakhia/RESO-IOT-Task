using API.Data;
using API.Data.DTOs;
using API.Extensions;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AppDbContext _dbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        [HttpGet(Name = "Telemetries")]
        public async Task<IEnumerable<TelemetryDTO>> GetStatisticsAsync(int deviceId)
        {
            var telemetries = await _dbContext.Telemetries
                .Where(t => t.DeviceId == deviceId)
                .OrderByDescending(i => i.Time)
                .GroupBy(t => new DateTime(t.Time).ToString())
                .Take(30)
                .Select(g => new TelemetryDTO()
                {
                    Date = DateTime.Parse(g.Key).ToString("yyyy-MM-dd",CultureInfo.InvariantCulture),
                    MaxIlluminance = g.Max(i => i.Illuminance)
                })
                .ToListAsync();

            return telemetries;

        }


        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<Device> Get()
        //{
        //    var list = _dbContext.Devices
        //        .Include(d => d.Telemetries)
        //        .ToList();
        //        return list;

        //    //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    //{
        //    //    Date = DateTime.Now.AddDays(index),
        //    //    TemperatureC = Random.Shared.Next(-20, 55),
        //    //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    //})
        //    //.ToArray();
        //}
    }
}