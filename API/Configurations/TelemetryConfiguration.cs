using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations
{
    public class TelemetryConfiguration : IEntityTypeConfiguration<Telemetry>
    {
        public void Configure(EntityTypeBuilder<Telemetry> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Illuminance).IsRequired();
            builder.Property(t => t.Time).IsRequired();
            builder.HasOne(t => t.Device).WithMany(d => d.Telemetries).HasForeignKey(t => t.DeviceId);
            builder.HasData(
                new Telemetry { Id = 1, Illuminance = 105.5, Time = DateTime.UtcNow.AddDays(1).Ticks, DeviceId = 1 },
                new Telemetry { Id = 2, Illuminance = 115.5, Time = DateTime.UtcNow.AddDays(1).Ticks, DeviceId = 1 },
                new Telemetry { Id = 3, Illuminance = 98.5, Time = DateTime.UtcNow.AddDays(2).Ticks, DeviceId = 1 },
                new Telemetry { Id = 4, Illuminance = 120.5, Time = DateTime.UtcNow.AddDays(2).Ticks, DeviceId = 1 },
                new Telemetry { Id = 5, Illuminance = 180.5, Time = DateTime.UtcNow.AddDays(3).Ticks, DeviceId = 1 },
                new Telemetry { Id = 6, Illuminance = 145.5, Time = DateTime.UtcNow.AddDays(3).Ticks, DeviceId = 1 },
                new Telemetry { Id = 7, Illuminance = 102.5, Time = DateTime.UtcNow.AddDays(4).Ticks, DeviceId = 1 },
                new Telemetry { Id = 8, Illuminance = 170.5, Time = DateTime.UtcNow.AddDays(4).Ticks, DeviceId = 1 },
                new Telemetry { Id = 9, Illuminance = 111.5, Time = DateTime.UtcNow.AddDays(5).Ticks, DeviceId = 1 },
                new Telemetry { Id = 10, Illuminance = 100.5, Time = DateTime.UtcNow.AddDays(5).Ticks, DeviceId = 1 },
                new Telemetry { Id = 11, Illuminance = 209.5, Time = DateTime.UtcNow.AddDays(6).Ticks, DeviceId = 1 },
                new Telemetry { Id = 12, Illuminance = 139.5, Time = DateTime.UtcNow.AddDays(6).Ticks, DeviceId = 1 },
                new Telemetry { Id = 13, Illuminance = 409.5, Time = DateTime.UtcNow.AddDays(7).Ticks, DeviceId = 1 },
                new Telemetry { Id = 14, Illuminance = 509.5, Time = DateTime.UtcNow.AddDays(7).Ticks, DeviceId = 1 },
                new Telemetry { Id = 15, Illuminance = 709.5, Time = DateTime.UtcNow.AddDays(8).Ticks, DeviceId = 1 },
                new Telemetry { Id = 16, Illuminance = 609.5, Time = DateTime.UtcNow.AddDays(8).Ticks, DeviceId = 1 },
                new Telemetry { Id = 17, Illuminance = 209.5, Time = DateTime.UtcNow.AddDays(9).Ticks, DeviceId = 1 },
                new Telemetry { Id = 18, Illuminance = 89.5, Time = DateTime.UtcNow.AddDays(9).Ticks, DeviceId = 1 },
                new Telemetry { Id = 19, Illuminance = 109.5, Time = DateTime.UtcNow.AddDays(10).Ticks, DeviceId = 1 },
                new Telemetry { Id = 20, Illuminance = 119.5, Time = DateTime.UtcNow.AddDays(10).Ticks, DeviceId = 1 },
                new Telemetry { Id = 21, Illuminance = 118.5, Time = DateTime.UtcNow.AddDays(11).Ticks, DeviceId = 1 },
                new Telemetry { Id = 22, Illuminance = 109.5, Time = DateTime.UtcNow.AddDays(11).Ticks, DeviceId = 1 }
                );
        }
    }
}
