using Ardalis.Specification;
using Webtorio.Models.Buildings;

namespace Webtorio.Specifications.Buildings;

public sealed class StoredBuildingsReadOnlySpec<T> : Specification<T> where T : Building
{
    public StoredBuildingsReadOnlySpec()
    {
        Query
            .Where(b => b.State == BuildingState.Stored)
            .Include(b => b.BuildingType)
            .AsNoTracking();
    }
}