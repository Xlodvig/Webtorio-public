using Ardalis.Specification;
using Webtorio.Models.Buildings;
using Webtorio.Models.StaticData;

namespace Webtorio.Specifications.Buildings;

public sealed class BuildingByIdWithAvailableRecipesFullDataReadOnlySpec : Specification<ManufactureBuilding>, 
    ISingleResultSpecification
{
    public BuildingByIdWithAvailableRecipesFullDataReadOnlySpec(int id)
    {
        Query
            .Include(b => b.BuildingType)
                .ThenInclude(bt => (bt as ManufactureBuildingType)!
                        .BuildingRecipeMatches.Where(brm => brm.Recipe.IsAvailable == true))
                    .ThenInclude(brm => brm.Recipe)
                        .ThenInclude(r => r.RecipeItems)
                            .ThenInclude(ri => ri.ItemType)
            .Where(b => b.Id == id)
            .AsNoTracking();
    }
}