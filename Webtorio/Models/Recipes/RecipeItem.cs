using Ardalis.Specification;
using Webtorio.Models.StaticData;

namespace Webtorio.Models.Recipes;

public class RecipeItem : IEntity<int>
{
    public int Id { get; set; }
    public double ItemAmount { get; init; }
    public int ItemTypeId { get; init; }
    public ItemType ItemType { get; init; } = null!;
    public int RecipeId { get; init; }
    public Recipe Recipe { get; init; } = null!;
    public bool IsOutput { get; init; }
    public bool IsBufferEmpty { get; set; } = true;
}