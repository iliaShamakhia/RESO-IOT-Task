using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Telemetry
    {
        public int Id { get; set; }
        public double Illuminance { get; set; }
        public long Time { get; set; }
        public int DeviceId { get; set; }
        public Device? Device { get; set; }

    }
}
