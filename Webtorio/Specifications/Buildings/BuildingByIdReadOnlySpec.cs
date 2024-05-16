using Ardalis.Specification;
using Webtorio.Models.Buildings;

namespace Webtorio.Specifications.Buildings;

public sealed class BuildingByIdReadOnlySpec : Specification<Building>, ISingleResultSpecification
{
    public BuildingByIdReadOnlySpec(int id)
    {
        Query
            .Include(b => b.BuildingType)
            .Include(b => b.Slot)
            .Where(b => b.Id == id)
            .AsNoTracking();
    }
}