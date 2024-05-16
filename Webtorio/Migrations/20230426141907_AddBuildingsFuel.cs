using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webtorio.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildingsFuel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FuelValue",
                table: "ResourceTypes",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFuel",
                table: "ResourceTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "BurnerEnergyReserveOnTick",
                table: "Items",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SelectedFuelResourceTypeId",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_SelectedFuelResourceTypeId",
                table: "Items",
                column: "SelectedFuelResourceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ResourceTypes_SelectedFuelResourceTypeId",
                table: "Items",
                column: "SelectedFuelResourceTypeId",
                principalTable: "ResourceTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ResourceTypes_SelectedFuelResourceTypeId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_SelectedFuelResourceTypeId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "FuelValue",
                table: "ResourceTypes");

            migrationBuilder.DropColumn(
                name: "IsFuel",
                table: "ResourceTypes");

            migrationBuilder.DropColumn(
                name: "SelectedFuelResourceTypeId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "BurnerEnergyReserveOnTick",
                table: "Items",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);
        }
    }
}
