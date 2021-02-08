using Microsoft.EntityFrameworkCore;
using HomeSurveillanceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSurveillanceAPI
{
    public class HomeSurveillanceDBContext : DbContext
    {
        public DbSet<MicroController> MicroControllers { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<SensorData> SensorData { get; set; }

        public HomeSurveillanceDBContext(DbContextOptions<HomeSurveillanceDBContext> options)
            : base(options) { }


        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region SeedData
            modelBuilder.Entity<MicroController>().HasData(new MicroController { MicroControllerId = 1, MACAddress = "F0:08:D1:C9:4B:D0", Name = "ESP32", Location = "Entrance"});
            modelBuilder.Entity<MicroController>().HasData(new MicroController { MicroControllerId = 2, MACAddress = "A0:88:D4:B1:F1:0F", Name = "Arduino", Location = "Living Room" });
            
            modelBuilder.Entity<Device>().HasData(new Device { DeviceId = 1, Enabled = true, Name = "Motion Sensor", MicroControllerId = 1,});
            modelBuilder.Entity<Device>().HasData(new Device { DeviceId = 2, Enabled = true, Name = "Motion Sensor", MicroControllerId = 1, });

            modelBuilder.Entity<SensorData>().HasData(new SensorData {SensorDataId = 1, ActivityTimeStamp = DateTime.Now, DeviceId = 1});
            #endregion
        }
    }
}
