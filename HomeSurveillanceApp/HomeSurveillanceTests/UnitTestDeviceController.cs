using HomeSurveillanceApp;
using HomeSurveillanceApp.Controllers.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using HomeSurveillanceApp.Models;
using System.Collections.Generic;
using Xunit;

namespace HomeSurveillanceTests
{
    public class UnitTestDeviceController
    {
        DeviceController DeviceController;
        HomeSurveillanceDBContext context;
        public static DbContextOptionsBuilder<HomeSurveillanceDBContext> optionsBuilder;
        private static string connectionString = "server=192.168.0.105; port=3306; database=SurveillanceDB; user=apiuser; password=pass1234; Persist Security Info=False; Connect Timeout=300";

        public UnitTestDeviceController()
        {
            optionsBuilder = new DbContextOptionsBuilder<HomeSurveillanceDBContext>().UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
            context = new HomeSurveillanceDBContext(optionsBuilder.Options);
            DeviceController = new DeviceController(context);
            context.Database.EnsureCreated();
        }

        [Fact]
        public async void GetById()
        {
            var result = await DeviceController.GetDevice(1);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetByIdReturnsNotFound()
        {
            var result = (await DeviceController.GetDevice(20));
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void PostPutDelete()
        {
            Device device = new Device { Name = "ESP32", Location = "Kitchen", MACAddress = "F0:AE:EA:EA:EA:EA" };

            var createdResult = (await DeviceController.PostDevice(device));


            Assert.IsType<CreatedAtActionResult>(createdResult.Result);

            device.Name = "UpdateESP32";
            device.Location = "Entrance";

            var putNoContentResult = await DeviceController.PutDevice(device.DeviceId, device);
            Assert.IsType<NoContentResult>(putNoContentResult);

            var deleteNoContentResult = await DeviceController.DeleteDevice(device.DeviceId);
            Assert.IsType<NoContentResult>(deleteNoContentResult);
        }

        [Fact]
        public async void Get()
        {
            var actionResult = await DeviceController.GetDevices();
            Assert.IsType<List<Device>>(actionResult.Value);
        }
    }
}
