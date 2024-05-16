using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webtorio.Migrations
{
    /// <inheritdoc />
    public partial class AddSlotToBuilding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Slots_BuildingId",
                table: "Slots");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_BuildingId",
                table: "Slots",
                column: "BuildingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Slots_BuildingId",
                table: "Slots");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_BuildingId",
                table: "Slots",
                column: "BuildingId");
        }
    }
}
