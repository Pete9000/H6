using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSurveillanceApp.Models
{
    public class IOUnit
    {
        public int IOUnitId { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public IList<Telemetry> Telemetrys { get; set; }
    }
}
