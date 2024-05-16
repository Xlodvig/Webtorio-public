using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webtorio.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRecipeWithRecipeItemRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeItem_Recipes_RecipeId",
                table: "RecipeItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeItem_Recipes_RecipeId1",
                table: "RecipeItem");

            migrationBuilder.DropIndex(
                name: "IX_RecipeItem_RecipeId",
                table: "RecipeItem");

            migrationBuilder.DropIndex(
                name: "IX_RecipeItem_RecipeId1",
                table: "RecipeItem");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "RecipeItem");

            migrationBuilder.DropColumn(
                name: "RecipeId1",
                table: "RecipeItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "RecipeItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecipeId1",
                table: "RecipeItem",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItem_RecipeId",
                table: "RecipeItem",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItem_RecipeId1",
                table: "RecipeItem",
                column: "RecipeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeItem_Recipes_RecipeId",
                table: "RecipeItem",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeItem_Recipes_RecipeId1",
                table: "RecipeItem",
                column: "RecipeId1",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
