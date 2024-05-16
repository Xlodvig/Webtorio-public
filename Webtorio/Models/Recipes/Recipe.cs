using System.ComponentModel.DataAnnotations;
using Ardalis.Specification;
using Webtorio.Models.StaticData;

namespace Webtorio.Models.Recipes;

public class Recipe : IEntity<int>
{
    public int Id { get; set; }
    [StringLength(100)]
    public string Name { get; init; } = null!;
    public List<RecipeItem> RecipeItems { get; init; } = null!;
    public IEnumerable<RecipeItem> InputItems => RecipeItems.Where(ri => ri.IsOutput == false);
    public IEnumerable<RecipeItem> OutputItems => RecipeItems.Where(ri => ri.IsOutput);
    public double CraftingTime { get; init; }
    public bool IsAvailable { get; init; }
    public List<BuildingRecipeMatch> BuildingRecipeMatches { get; init; } = null!;
}