using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace satelliteBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationImageEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationImage_Locations_LocationId",
                table: "LocationImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationImage",
                table: "LocationImage");

            migrationBuilder.RenameTable(
                name: "LocationImage",
                newName: "LocationImages");

            migrationBuilder.RenameIndex(
                name: "IX_LocationImage_LocationId",
                table: "LocationImages",
                newName: "IX_LocationImages_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationImages",
                table: "LocationImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationImages_Locations_LocationId",
                table: "LocationImages",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationImages_Locations_LocationId",
                table: "LocationImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationImages",
                table: "LocationImages");

            migrationBuilder.RenameTable(
                name: "LocationImages",
                newName: "LocationImage");

            migrationBuilder.RenameIndex(
                name: "IX_LocationImages_LocationId",
                table: "LocationImage",
                newName: "IX_LocationImage_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationImage",
                table: "LocationImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationImage_Locations_LocationId",
                table: "LocationImage",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
