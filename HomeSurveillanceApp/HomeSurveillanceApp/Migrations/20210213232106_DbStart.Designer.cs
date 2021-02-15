﻿// <auto-generated />
using System;
using HomeSurveillanceApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HomeSurveillanceApp.Migrations
{
    [DbContext(typeof(HomeSurveillanceDBContext))]
    [Migration("20210213232106_DbStart")]
    partial class DbStart
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("HomeSurveillanceApp.Models.Device", b =>
                {
                    b.Property<int>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("MACAddress")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4");

                    b.HasKey("DeviceId");

                    b.ToTable("Devices");

                    b.HasData(
                        new
                        {
                            DeviceId = 1,
                            Location = "Entrance",
                            MACAddress = "F0:08:D1:C9:4B:D0",
                            Name = "ESP32"
                        },
                        new
                        {
                            DeviceId = 2,
                            Location = "Living Room",
                            MACAddress = "A0:88:D4:B1:F1:0F",
                            Name = "Arduino"
                        });
                });

            modelBuilder.Entity("HomeSurveillanceApp.Models.IOUnit", b =>
                {
                    b.Property<int>("IOUnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("IOUnitId");

                    b.HasIndex("DeviceId");

                    b.ToTable("IOUnits");

                    b.HasData(
                        new
                        {
                            IOUnitId = 1,
                            DeviceId = 1,
                            Enabled = true,
                            Name = "Motion Sensor"
                        },
                        new
                        {
                            IOUnitId = 2,
                            DeviceId = 2,
                            Enabled = true,
                            Name = "Motion Sensor"
                        });
                });

            modelBuilder.Entity("HomeSurveillanceApp.Models.Telemetry", b =>
                {
                    b.Property<int>("TelemetryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ActivityTimeStamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IOUnitId")
                        .HasColumnType("int");

                    b.HasKey("TelemetryId");

                    b.HasIndex("IOUnitId");

                    b.ToTable("Telemetrys");

                    b.HasData(
                        new
                        {
                            TelemetryId = 1,
                            ActivityTimeStamp = new DateTime(2021, 2, 14, 0, 21, 6, 295, DateTimeKind.Local).AddTicks(770),
                            IOUnitId = 1
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");

                    b.HasData(
                        new
                        {
                            UserId = "691fb7f8-66ce-4aca-b015-52082d23ad2b",
                            RoleId = "ce0df133-9c5e-4b18-9572-b44a5c405cd5"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("HomeSurveillanceApp.Models.Auth.Role", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityRole");

                    b.HasDiscriminator().HasValue("Role");

                    b.HasData(
                        new
                        {
                            Id = "ce0df133-9c5e-4b18-9572-b44a5c405cd5",
                            ConcurrencyStamp = "6e0d77d2-06b5-4413-a13f-616f15830a15",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "c061dcfd-62f4-4626-92b4-ae8deffeb964",
                            ConcurrencyStamp = "56504448-b018-46b7-8ea7-c67706ed1f1b",
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("HomeSurveillanceApp.Models.Auth.User", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.HasDiscriminator().HasValue("User");

                    b.HasData(
                        new
                        {
                            Id = "691fb7f8-66ce-4aca-b015-52082d23ad2b",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "943bfe7e-ad75-40bf-ace3-44f0c2fa21ce",
                            Email = "sysadmin@test.dk",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAEAACcQAAAAEA0OV/hKcb/L17spyONyZxijtMmvy3WJqD/7l/7ziir4GyQ3A8rBhUNzJaavcJAOtw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "d5258452-8bf2-4748-838e-11196f3378d3",
                            TwoFactorEnabled = false,
                            UserName = "Admin"
                        },
                        new
                        {
                            Id = "6308e3f7-8113-4eda-9890-4c159208afb3",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "443c23ff-47ea-4e51-bdba-94c24d8d8588",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedUserName = "DEVICEUSER",
                            PasswordHash = "AQAAAAEAACcQAAAAEK/TyHFBH2p71fHF/xIY5UN8JYgvwgsad5OpcnufzLYe3iw15WyAPiVGWptWkm81Wg==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "7d43cb60-5109-4c33-96c2-dd84f5932e10",
                            TwoFactorEnabled = false,
                            UserName = "DeviceUser"
                        });
                });

            modelBuilder.Entity("HomeSurveillanceApp.Models.IOUnit", b =>
                {
                    b.HasOne("HomeSurveillanceApp.Models.Device", "Device")
                        .WithMany("IOUnits")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("HomeSurveillanceApp.Models.Telemetry", b =>
                {
                    b.HasOne("HomeSurveillanceApp.Models.IOUnit", "IOUnit")
                        .WithMany("Telemetrys")
                        .HasForeignKey("IOUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IOUnit");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HomeSurveillanceApp.Models.Device", b =>
                {
                    b.Navigation("IOUnits");
                });

            modelBuilder.Entity("HomeSurveillanceApp.Models.IOUnit", b =>
                {
                    b.Navigation("Telemetrys");
                });
#pragma warning restore 612, 618
        }
    }
}