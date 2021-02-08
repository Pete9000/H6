using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSurveillanceAPI.Models
{
    public class Device
    {
        public int DeviceId { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public int MicroControllerId { get; set; }
        public MicroController MicroController { get; set; }
        public IList<SensorData> SensorData { get; set; }
    }
}
