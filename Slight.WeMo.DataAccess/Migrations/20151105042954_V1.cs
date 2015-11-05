using System;

using Microsoft.Data.Entity.Migrations;

namespace Slight.WeMo.DataAccess.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeMoDevice",
                columns: table => new
                {
                    DeviceId = table.Column<string>(nullable: false),
                    DeviceType = table.Column<string>(nullable: true),
                    FirmwareVersion = table.Column<string>(nullable: true),
                    FriendlyName = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    LastDetected = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    MacAddress = table.Column<string>(nullable: true),
                    ModelName = table.Column<string>(nullable: true),
                    ModelNumber = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    SerialNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeMoDevice", x => x.DeviceId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("WeMoDevice");
        }
    }
}
