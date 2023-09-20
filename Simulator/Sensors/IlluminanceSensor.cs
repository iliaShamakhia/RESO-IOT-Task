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

        private readonly List<AddTelemetryDTO> _telemetries;

        private readonly Random _random;

        public int DeviceId { get; set; } = 1;

        public IlluminanceSensor()
        {
            _telemetries = new List<AddTelemetryDTO>();
            _random = new Random();
        }

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

            _telemetries.Add(telemetry);

            if (_telemetries.Count == 4)
            {
                await SendDataToApi(_telemetries);
                _telemetries.Clear();
            }
        }

        private double GetIlluminance()
        {
            double minIlluminance = 0;
            double maxIlluminanceMorning = 1000;
            double maxIlluminanceEvening = 500;

            DateTime currentTime = DateTime.Now;

            double peakValue = _random.NextDouble() * (maxIlluminanceMorning - maxIlluminanceEvening) + maxIlluminanceEvening;

            double illuminance;

            if (currentTime.Hour < 12)
            {
                double morningPhase = (double)currentTime.Hour / 12;
                illuminance = minIlluminance + ((maxIlluminanceMorning - minIlluminance) / 2) * (1 + Math.Sin(morningPhase * Math.PI));
            }
            else
            {
                double eveningPhase = (double)(currentTime.Hour - 12) / 12;
                illuminance = minIlluminance + ((maxIlluminanceEvening - minIlluminance) / 2) * (1 - Math.Sin(eveningPhase * Math.PI));
            }

            illuminance += _random.NextDouble() * (peakValue / 5);

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
                httpClient.DefaultRequestHeaders.Add("X-ApiKey", "0f8fad5b-d9cb-469f-a165-70867728950e");
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
