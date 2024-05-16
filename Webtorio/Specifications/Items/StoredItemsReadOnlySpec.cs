using Ardalis.Specification;
using Webtorio.Models;
using Webtorio.Models.Buildings;

namespace Webtorio.Specifications.Items;

public sealed class StoredItemsReadOnlySpec : Specification<Item>
{
    public StoredItemsReadOnlySpec()
    {
        Query
            .Where(i => (i is Building && ((Building)i).State == BuildingState.Stored) || !(i is Building))
            .AsNoTracking();
    }
}