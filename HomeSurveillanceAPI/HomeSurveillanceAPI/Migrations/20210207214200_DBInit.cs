using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSurveillanceAPI.Migrations
{
    public partial class DBInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MicroControllers",
                columns: table => new
                {
                    MicroControllerId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MACAddress = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MicroControllers", x => x.MicroControllerId);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MicroControllerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_Devices_MicroControllers_MicroControllerId",
                        column: x => x.MicroControllerId,
                        principalTable: "MicroControllers",
                        principalColumn: "MicroControllerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorData",
                columns: table => new
                {
                    SensorDataId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityTimeStamp = table.Column<DateTime>(nullable: false),
                    DeviceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorData", x => x.SensorDataId);
                    table.ForeignKey(
                        name: "FK_SensorData_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MicroControllers",
                columns: new[] { "MicroControllerId", "Location", "MACAddress", "Name" },
                values: new object[] { 1, "Entrance", "F0:08:D1:C9:4B:D0", "ESP32" });

            migrationBuilder.InsertData(
                table: "MicroControllers",
                columns: new[] { "MicroControllerId", "Location", "MACAddress", "Name" },
                values: new object[] { 2, "Living Room", "A0:88:D4:B1:F1:0F", "Arduino" });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "DeviceId", "Enabled", "MicroControllerId", "Name" },
                values: new object[] { 1, true, 1, "Motion Sensor" });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "DeviceId", "Enabled", "MicroControllerId", "Name" },
                values: new object[] { 2, true, 1, "Motion Sensor" });

            migrationBuilder.InsertData(
                table: "SensorData",
                columns: new[] { "SensorDataId", "ActivityTimeStamp", "DeviceId" },
                values: new object[] { 1, new DateTime(2021, 2, 7, 22, 42, 0, 267, DateTimeKind.Local).AddTicks(8740), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_MicroControllerId",
                table: "Devices",
                column: "MicroControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorData_DeviceId",
                table: "SensorData",
                column: "DeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorData");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "MicroControllers");
        }
    }
}
