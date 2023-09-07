using API.Data.DTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Simulator.Sensors
{
    public class IlluminanceSensor : ISensor
    {
        private static System.Timers.Timer timer = new System.Timers.Timer(1000);//15 * 60 * 1000);

        private static List<AddTelemetryDTO> telemetries = new List<AddTelemetryDTO>();

        public int DeviceId { get; set; } = 1;

        //private readonly IConfigurationRoot config;

        //public IlluminanceSensor()
        //{
        //    var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json");
        //    config = configuration.Build();
        //}

        public void Start()
        {
            timer.Elapsed += MeasureIlluminance;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Stop();
            timer.Dispose();
        }

        private async void MeasureIlluminance(Object? source, ElapsedEventArgs e)
        {
            double illuminance = GetIlluminance();
            long time = DateTime.UtcNow.Ticks;

            var telemetry = new AddTelemetryDTO
            {
                Time = time,
                Illuminance = illuminance
            };

            telemetries.Add(telemetry);

            if (telemetries.Count == 4)
            {
                await SendDataToApi(telemetries);
                telemetries.Clear();
            }
        }

        private double GetIlluminance()
        {
            var illuminance = new Random().NextDouble() * 1000;

            illuminance = Math.Round(illuminance * 2) / 2.0;

            return illuminance;
        }

        private async Task SendDataToApi(List<AddTelemetryDTO> data)
        {
            using (var httpClient = new HttpClient())
            {
                string url = $"https://localhost:7253/api/devices/{DeviceId}/telemetry";
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("X-ApiKey", "0f8fad5b-d9cb-469f-a165-70867728950e");
                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                throw new HttpRequestException(response.StatusCode.ToString());

            }
        }

        public async Task<string> GetDataFromApi()
        {
            using (var httpClient = new HttpClient())
            {
                string url = $"https://localhost:7253/api/devices/{DeviceId}/statistics";
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                throw new HttpRequestException(response.StatusCode.ToString());

            }
        }

    }
}
