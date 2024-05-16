using Ardalis.Specification;
using Webtorio.Models;

namespace Webtorio.Specifications.Resources;

public sealed class StoredFuelReadOnlySpec : Specification<Resource>
{
    public StoredFuelReadOnlySpec()
    {
        Query
            .Where(r => r.ResourceType.IsFuel == true)
            .Include(r => r.ResourceType)
            .AsNoTracking();
    }
}