using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webtorio.Migrations
{
    /// <inheritdoc />
    public partial class RenameBuildingFieldBurnerEnergyReserveOnTick : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BurnerEnergyReserveOnTick",
                table: "Items",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BurnerEnergyReserveOnTick",
                table: "Items");
        }
    }
}
