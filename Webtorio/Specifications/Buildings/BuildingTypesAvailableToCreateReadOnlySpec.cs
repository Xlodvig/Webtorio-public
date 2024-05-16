using Ardalis.Specification;
using Webtorio.Models.StaticData;

namespace Webtorio.Specifications.Buildings;

public sealed class BuildingTypesAvailableToCreateReadOnlySpec : Specification<BuildingType>
{
    public BuildingTypesAvailableToCreateReadOnlySpec()
    {
        Query
            .Where(bt => bt.IsAvailable == true)
            .AsNoTracking();
    }
}