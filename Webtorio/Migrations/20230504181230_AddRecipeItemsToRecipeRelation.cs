using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webtorio.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeItemsToRecipeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOutput",
                table: "RecipeItem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "RecipeItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItem_RecipeId",
                table: "RecipeItem",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeItem_Recipes_RecipeId",
                table: "RecipeItem",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeItem_Recipes_RecipeId",
                table: "RecipeItem");

            migrationBuilder.DropIndex(
                name: "IX_RecipeItem_RecipeId",
                table: "RecipeItem");

            migrationBuilder.DropColumn(
                name: "IsOutput",
                table: "RecipeItem");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "RecipeItem");
        }
    }
}
