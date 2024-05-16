using Webtorio.Models.Recipes;

namespace Webtorio.Models.StaticData;

public class BuildingRecipeMatch
{
    public int ManufactureBuildingTypeId { get; init; }
    public ManufactureBuildingType ManufactureBuildingType { get; init; } = null!;
    
    public int RecipeId { get; init; }
    public Recipe Recipe { get; init; } = null!;
}