using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Slight.WeMo.DataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("SwitchEvent");
        }
    }
}
