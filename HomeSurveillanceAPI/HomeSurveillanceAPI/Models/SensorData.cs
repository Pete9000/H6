using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSurveillanceAPI.Models
{
    public class SensorData
    {
        public int SensorDataId { get; set; }
        public DateTime ActivityTimeStamp { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
