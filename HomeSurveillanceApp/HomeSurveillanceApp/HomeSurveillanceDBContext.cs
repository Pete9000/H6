using Microsoft.EntityFrameworkCore;
using HomeSurveillanceApp.Models;
using HomeSurveillanceApp.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HomeSurveillanceApp
{
    public class HomeSurveillanceDBContext : IdentityDbContext
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


            //Add Users to identity.

            string adminId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(
                new User { Id = adminId, UserName = "Admin", NormalizedUserName = "ADMIN", Email = "sysadmin@test.dk", SecurityStamp = Guid.NewGuid().ToString(), PasswordHash = hasher.HashPassword(null, "admin") },
                new User { UserName = "DeviceUser", NormalizedUserName = "DEVICEUSER", SecurityStamp = Guid.NewGuid().ToString(), PasswordHash = hasher.HashPassword(null, "test123!") }
            );


            //Add Admin role
            string roleId = Guid.NewGuid().ToString();

            modelBuilder.Entity<Role>().HasData(
                new Role { ConcurrencyStamp = Guid.NewGuid().ToString(), Id = roleId, Name = "Admin", NormalizedName = "ADMIN" },
                new Role { ConcurrencyStamp = Guid.NewGuid().ToString(), Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER" }
            );
            //Add Role to Admin user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = roleId, UserId = adminId }
            );

            #endregion
        }
    }
}
