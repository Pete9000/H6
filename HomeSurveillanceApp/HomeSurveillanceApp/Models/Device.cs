using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeSurveillanceApp.Models
{
    public class Device
    {
        public int DeviceId { get; set; }

        [Required(ErrorMessage = "MACAddress is required")]
        public string MACAddress { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name can't be longer than 20 characters")]
        public string Name { get; set; }
        public string Location { get; set; }
        public IList<IOUnit> IOUnits { get; set; }
    }
}
