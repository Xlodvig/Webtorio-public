using Ardalis.Specification;
using Webtorio.Models.Buildings;
using Webtorio.Models.StaticData;

namespace Webtorio.Specifications.Buildings;

public sealed class BuildingByIdWithAvailableRecipesSpec : Specification<ManufactureBuilding>, ISingleResultSpecification
{
    public BuildingByIdWithAvailableRecipesSpec(int id)
    {
        Query
            .Include(b => b.BuildingType)
                .ThenInclude(bt => (bt as ManufactureBuildingType)!
                        .BuildingRecipeMatches.Where(brm => brm.Recipe.IsAvailable == true))
            .Where(b => b.Id == id);
    }
}