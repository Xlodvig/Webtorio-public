using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webtorio.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildingStateRenameProductiveBuilding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuildingRecipeMatch_ItemTypes_ProductiveBuildingTypeId",
                table: "BuildingRecipeMatch");

            migrationBuilder.RenameColumn(
                name: "FuelValue",
                table: "ItemTypes",
                newName: "OutputAmount");

            migrationBuilder.RenameColumn(
                name: "EnergyConsumption",
                table: "ItemTypes",
                newName: "OutputItemTypeId");

            migrationBuilder.RenameColumn(
                name: "ProductiveBuildingTypeId",
                table: "BuildingRecipeMatch",
                newName: "ManufactureBuildingTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_BuildingRecipeMatch_ProductiveBuildingTypeId",
                table: "BuildingRecipeMatch",
                newName: "IX_BuildingRecipeMatch_ManufactureBuildingTypeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsBufferEmpty",
                table: "RecipeItem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EnergyConsumptionKW",
                table: "ItemTypes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FuelValueMJ",
                table: "ItemTypes",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "InputAmount",
                table: "ItemTypes",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InputItemTypeId",
                table: "ItemTypes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNoSolid",
                table: "ItemTypes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BuildingRecipeMatch_ItemTypes_ManufactureBuildingTypeId",
                table: "BuildingRecipeMatch",
                column: "ManufactureBuildingTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuildingRecipeMatch_ItemTypes_ManufactureBuildingTypeId",
                table: "BuildingRecipeMatch");

            migrationBuilder.DropColumn(
                name: "IsBufferEmpty",
                table: "RecipeItem");

            migrationBuilder.DropColumn(
                name: "EnergyConsumptionKW",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "FuelValueMJ",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "InputAmount",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "InputItemTypeId",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "IsNoSolid",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "OutputItemTypeId",
                table: "ItemTypes",
                newName: "EnergyConsumption");

            migrationBuilder.RenameColumn(
                name: "OutputAmount",
                table: "ItemTypes",
                newName: "FuelValue");

            migrationBuilder.RenameColumn(
                name: "ManufactureBuildingTypeId",
                table: "BuildingRecipeMatch",
                newName: "ProductiveBuildingTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_BuildingRecipeMatch_ManufactureBuildingTypeId",
                table: "BuildingRecipeMatch",
                newName: "IX_BuildingRecipeMatch_ProductiveBuildingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuildingRecipeMatch_ItemTypes_ProductiveBuildingTypeId",
                table: "BuildingRecipeMatch",
                column: "ProductiveBuildingTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
