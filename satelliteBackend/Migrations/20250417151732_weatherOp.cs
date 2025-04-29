using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace satelliteBackend.Migrations
{
    /// <inheritdoc />
    public partial class weatherOp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Humidity",
                table: "WeatherForecasts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Precipitation",
                table: "WeatherForecasts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UvIndex",
                table: "WeatherForecasts",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Humidity",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "Precipitation",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "UvIndex",
                table: "WeatherForecasts");
        }
    }
}
