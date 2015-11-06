using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Slight.WeMo.DataAccess.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("SwitchEvent");
            migrationBuilder.CreateTable(
                name: "WeMoDeviceState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrentState = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    WeMoDeviceDeviceId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeMoDeviceState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeMoDeviceState_WeMoDevice_WeMoDeviceDeviceId",
                        column: x => x.WeMoDeviceDeviceId,
                        principalTable: "WeMoDevice",
                        principalColumn: "DeviceId");
                });
            migrationBuilder.AddColumn<bool>(
                name: "Disabled",
                table: "WeMoDevice",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Disabled", table: "WeMoDevice");
            migrationBuilder.DropTable("WeMoDeviceState");
            migrationBuilder.CreateTable(
                name: "SwitchEvent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrentState = table.Column<int>(nullable: false),
                    DeviceId = table.Column<string>(nullable: false),
                    OldState = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwitchEvent", x => x.Id);
                });
        }
    }
}
