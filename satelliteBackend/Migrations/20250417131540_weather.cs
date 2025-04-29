using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace satelliteBackend.Migrations
{
    /// <inheritdoc />
    public partial class weather : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Humidity",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "TemperatureC",
                table: "WeatherForecasts");

            migrationBuilder.RenameColumn(
                name: "WindKph",
                table: "WeatherForecasts",
                newName: "WindSpeed");

            migrationBuilder.RenameColumn(
                name: "UVIndex",
                table: "WeatherForecasts",
                newName: "Temperature");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "WeatherForecasts",
                newName: "ForecastDate");

            migrationBuilder.RenameColumn(
                name: "ConditionText",
                table: "WeatherForecasts",
                newName: "WeatherDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WindSpeed",
                table: "WeatherForecasts",
                newName: "WindKph");

            migrationBuilder.RenameColumn(
                name: "WeatherDescription",
                table: "WeatherForecasts",
                newName: "ConditionText");

            migrationBuilder.RenameColumn(
                name: "Temperature",
                table: "WeatherForecasts",
                newName: "UVIndex");

            migrationBuilder.RenameColumn(
                name: "ForecastDate",
                table: "WeatherForecasts",
                newName: "Date");

            migrationBuilder.AddColumn<double>(
                name: "Humidity",
                table: "WeatherForecasts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TemperatureC",
                table: "WeatherForecasts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
