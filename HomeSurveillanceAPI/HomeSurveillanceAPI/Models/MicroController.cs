using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeSurveillanceAPI.Models
{
    public class MicroController
    {
        public int MicroControllerId { get; set; }

        [Required(ErrorMessage = "MACAddress is required")]
        public string MACAddress { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name can't be longer than 20 characters")]
        public string Name { get; set; }
        public string Location { get; set; }
        public IList<Device> Devices { get; set; }
    }
}
