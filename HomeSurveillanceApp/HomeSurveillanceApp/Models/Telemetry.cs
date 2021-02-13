using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSurveillanceApp.Models
{
    public class Telemetry
    {
        public int TelemetryId { get; set; }
        public DateTime ActivityTimeStamp { get; set; }
        public int IOUnitId { get; set; }
        public IOUnit IOUnit { get; set; }
    }
}
