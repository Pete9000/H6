﻿// <auto-generated />
using System;
using HomeSurveillanceAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HomeSurveillanceAPI.Migrations
{
    [DbContext(typeof(HomeSurveillanceDBContext))]
    partial class HomeSurveillanceDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("HomeSurveillanceAPI.Models.Device", b =>
                {
                    b.Property<int>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MicroControllerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("DeviceId");

                    b.HasIndex("MicroControllerId");

                    b.ToTable("Devices");

                    b.HasData(
                        new
                        {
                            DeviceId = 1,
                            Enabled = true,
                            MicroControllerId = 1,
                            Name = "Motion Sensor"
                        },
                        new
                        {
                            DeviceId = 2,
                            Enabled = true,
                            MicroControllerId = 1,
                            Name = "Motion Sensor"
                        });
                });

            modelBuilder.Entity("HomeSurveillanceAPI.Models.MicroController", b =>
                {
                    b.Property<int>("MicroControllerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("MACAddress")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20);

                    b.HasKey("MicroControllerId");

                    b.ToTable("MicroControllers");

                    b.HasData(
                        new
                        {
                            MicroControllerId = 1,
                            Location = "Entrance",
                            MACAddress = "F0:08:D1:C9:4B:D0",
                            Name = "ESP32"
                        },
                        new
                        {
                            MicroControllerId = 2,
                            Location = "Living Room",
                            MACAddress = "A0:88:D4:B1:F1:0F",
                            Name = "Arduino"
                        });
                });

            modelBuilder.Entity("HomeSurveillanceAPI.Models.SensorData", b =>
                {
                    b.Property<int>("SensorDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ActivityTimeStamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.HasKey("SensorDataId");

                    b.HasIndex("DeviceId");

                    b.ToTable("SensorData");

                    b.HasData(
                        new
                        {
                            SensorDataId = 1,
                            ActivityTimeStamp = new DateTime(2021, 2, 7, 22, 42, 0, 267, DateTimeKind.Local).AddTicks(8740),
                            DeviceId = 1
                        });
                });

            modelBuilder.Entity("HomeSurveillanceAPI.Models.Device", b =>
                {
                    b.HasOne("HomeSurveillanceAPI.Models.MicroController", "MicroController")
                        .WithMany("Devices")
                        .HasForeignKey("MicroControllerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HomeSurveillanceAPI.Models.SensorData", b =>
                {
                    b.HasOne("HomeSurveillanceAPI.Models.Device", "Device")
                        .WithMany("SensorData")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
