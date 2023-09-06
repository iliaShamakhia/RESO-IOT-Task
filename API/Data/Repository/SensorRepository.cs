using API.Data.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace API.Data.Repository
{
    public class SensorRepository : ISensorRepository
    {
        private readonly AppDbContext _context;

        public SensorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddDataAsync(int deviceId, List<AddTelemetryDTO> data)
        {
            await _context.Telemetries.AddRangeAsync(data.Select(d => new Telemetry()
            {
                Illuminance = d.Illuminance,
                Time = d.Time,
                DeviceId = deviceId
            }));

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TelemetryDTO>> GetDataAsync(int deviceId)
        {
            var telemetries = await _context.Telemetries
                .Where(t => t.DeviceId == deviceId)
                .GroupBy(t => new DateTime(t.Time).ToString("yyyy-MM-dd"))
                .Select(t => new TelemetryDTO()
                {
                    Date = t.Key,
                    MaxIlluminance = t.Max(i => i.Illuminance)
                })
                .OrderByDescending(t => t.Date)
                .Take(30)
                .ToListAsync();

            return telemetries;
        }
    }
}
