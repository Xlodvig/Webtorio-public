using Ardalis.Specification;
using Webtorio.Models;

namespace Webtorio.Specifications.Resources;

public sealed class ResourceByResourceTypeIdReadOnlySpec : Specification<Resource>
{
    public ResourceByResourceTypeIdReadOnlySpec(int id)
    {
        Query
            .Where(r => r.ResourceTypeId == id)
            .Include(r => r.ResourceType)
            .AsNoTracking();
    }
}