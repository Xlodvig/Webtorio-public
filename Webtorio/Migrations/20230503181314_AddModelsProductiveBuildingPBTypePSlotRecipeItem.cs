using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Webtorio.Migrations
{
    /// <inheritdoc />
    public partial class AddModelsProductiveBuildingPBTypePSlotRecipeItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_ResourceTypes_ResourceTypeId",
                table: "Deposits");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_BuildingTypes_BuildingTypeId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ResourceTypes_ResourceTypeId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ResourceTypes_SelectedFuelResourceTypeId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "BuildingTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResourceTypes",
                table: "ResourceTypes");

            migrationBuilder.RenameTable(
                name: "ResourceTypes",
                newName: "ItemTypes");

            migrationBuilder.AddColumn<double>(
                name: "ResourceBuffer",
                table: "Items",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SelectedRecipeId",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TicksBeforeWorkIsDone",
                table: "Items",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WorkSpeed",
                table: "Items",
                type: "double precision",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFuel",
                table: "ItemTypes",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ItemTypes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Energy",
                table: "ItemTypes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnergyConsumption",
                table: "ItemTypes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "ItemTypes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MiningSpeed",
                table: "ItemTypes",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WorkSpeed",
                table: "ItemTypes",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemTypes",
                table: "ItemTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CraftingTime = table.Column<double>(type: "double precision", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildingRecipeMatch",
                columns: table => new
                {
                    ProductiveBuildingTypeId = table.Column<int>(type: "integer", nullable: false),
                    RecipeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingRecipeMatch", x => new { x.RecipeId, x.ProductiveBuildingTypeId });
                    table.ForeignKey(
                        name: "FK_BuildingRecipeMatch_ItemTypes_ProductiveBuildingTypeId",
                        column: x => x.ProductiveBuildingTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuildingRecipeMatch_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemAmount = table.Column<double>(type: "double precision", nullable: false),
                    ItemTypeId = table.Column<int>(type: "integer", nullable: false),
                    RecipeId = table.Column<int>(type: "integer", nullable: false),
                    RecipeId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeItem_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeItem_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeItem_Recipes_RecipeId1",
                        column: x => x.RecipeId1,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_SelectedRecipeId",
                table: "Items",
                column: "SelectedRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingRecipeMatch_ProductiveBuildingTypeId",
                table: "BuildingRecipeMatch",
                column: "ProductiveBuildingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItem_ItemTypeId",
                table: "RecipeItem",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItem_RecipeId",
                table: "RecipeItem",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItem_RecipeId1",
                table: "RecipeItem",
                column: "RecipeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_ItemTypes_ResourceTypeId",
                table: "Deposits",
                column: "ResourceTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemTypes_BuildingTypeId",
                table: "Items",
                column: "BuildingTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemTypes_ResourceTypeId",
                table: "Items",
                column: "ResourceTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemTypes_SelectedFuelResourceTypeId",
                table: "Items",
                column: "SelectedFuelResourceTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Recipes_SelectedRecipeId",
                table: "Items",
                column: "SelectedRecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_ItemTypes_ResourceTypeId",
                table: "Deposits");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemTypes_BuildingTypeId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemTypes_ResourceTypeId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemTypes_SelectedFuelResourceTypeId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Recipes_SelectedRecipeId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "BuildingRecipeMatch");

            migrationBuilder.DropTable(
                name: "RecipeItem");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Items_SelectedRecipeId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemTypes",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "ResourceBuffer",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SelectedRecipeId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TicksBeforeWorkIsDone",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WorkSpeed",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "Energy",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "EnergyConsumption",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "MiningSpeed",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "WorkSpeed",
                table: "ItemTypes");

            migrationBuilder.RenameTable(
                name: "ItemTypes",
                newName: "ResourceTypes");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFuel",
                table: "ResourceTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResourceTypes",
                table: "ResourceTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BuildingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    Energy = table.Column<int>(type: "integer", nullable: false),
                    EnergyConsumption = table.Column<int>(type: "integer", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MiningSpeed = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_ResourceTypes_ResourceTypeId",
                table: "Deposits",
                column: "ResourceTypeId",
                principalTable: "ResourceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_BuildingTypes_BuildingTypeId",
                table: "Items",
                column: "BuildingTypeId",
                principalTable: "BuildingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ResourceTypes_ResourceTypeId",
                table: "Items",
                column: "ResourceTypeId",
                principalTable: "ResourceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ResourceTypes_SelectedFuelResourceTypeId",
                table: "Items",
                column: "SelectedFuelResourceTypeId",
                principalTable: "ResourceTypes",
                principalColumn: "Id");
        }
    }
}
