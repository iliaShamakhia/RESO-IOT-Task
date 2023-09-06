using API.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Simulator.Sensors
{
    public class IlluminanceSensor : ISensor
    {
        private static System.Timers.Timer timer = new System.Timers.Timer(15 * 60 * 1000);

        private static List<AddTelemetryDTO> telemetries = new List<AddTelemetryDTO>();

        public int DeviceId { get; set; } = 1;

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

        private void MeasureIlluminance(Object? source, ElapsedEventArgs e)
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
                SendDataToApi(telemetries);
                telemetries.Clear();
            }
        }

        private double GetIlluminance()
        {
            var illuminance = new Random().NextDouble() * 1000;

            illuminance = Math.Round(illuminance * 2) / 2.0;

            return illuminance;
        }

        private void SendDataToApi(List<AddTelemetryDTO> data)
        {

        }

    }
}
