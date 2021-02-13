using Microsoft.EntityFrameworkCore;
using HomeSurveillanceApp.Models;
using HomeSurveillanceApp.Authentication.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HomeSurveillanceApp
{
    public class HomeSurveillanceDBContext : IdentityDbContext<User>
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<IOUnit> IOUnits { get; set; }
        public DbSet<Telemetry> Telemetrys { get; set; }
        public HomeSurveillanceDBContext(DbContextOptions<HomeSurveillanceDBContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Set admin user and seed some testData
            #region SeedData
            modelBuilder.Entity<Device>().HasData(
                new Device { DeviceId = 1, MACAddress = "F0:08:D1:C9:4B:D0", Name = "ESP32", Location = "Entrance"},
                new Device { DeviceId = 2, MACAddress = "A0:88:D4:B1:F1:0F", Name = "Arduino", Location = "Living Room" });

            modelBuilder.Entity<IOUnit>().HasData(
                new IOUnit { IOUnitId = 1, Enabled = true, Name = "Motion Sensor", DeviceId = 1 },
                new IOUnit { IOUnitId = 2, Enabled = true, Name = "Motion Sensor", DeviceId = 2 }
            );

            modelBuilder.Entity<Telemetry>().HasData(new Telemetry { TelemetryId = 1, ActivityTimeStamp = DateTime.Now, IOUnitId = 1 });

            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(
                new User { UserName = "Admin", NormalizedUserName = "ADMIN", Email = "sysadmin@test.dk", SecurityStamp = Guid.NewGuid().ToString(), PasswordHash = hasher.HashPassword(null, "admin") },
                new User { UserName = "DeviceUser", NormalizedUserName = "DeviceUser", SecurityStamp = Guid.NewGuid().ToString(), PasswordHash = hasher.HashPassword(null, "test123!") }
            );

            #endregion
        }
    }
}
