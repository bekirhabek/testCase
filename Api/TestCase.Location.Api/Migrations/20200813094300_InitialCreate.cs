using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestCase.Location.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SourceLatitude = table.Column<double>(nullable: false),
                    SourceLongitude = table.Column<double>(nullable: false),
                    DestinationLatitude = table.Column<double>(nullable: false),
                    DestinationLongitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Location");
        }
    }
}
