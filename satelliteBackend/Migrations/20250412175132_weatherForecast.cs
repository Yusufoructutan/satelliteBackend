using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace satelliteBackend.Migrations
{
    /// <inheritdoc />
    public partial class weatherForecast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TemperatureC = table.Column<float>(type: "real", nullable: false),
                    Humidity = table.Column<int>(type: "int", nullable: false),
                    ConditionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WindKph = table.Column<float>(type: "real", nullable: false),
                    UVIndex = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherForecasts_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherForecasts_LocationId",
                table: "WeatherForecasts",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherForecasts");
        }
    }
}
