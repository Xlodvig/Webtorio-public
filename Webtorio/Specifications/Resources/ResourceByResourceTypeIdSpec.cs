using Ardalis.Specification;
using Webtorio.Models;

namespace Webtorio.Specifications.Resources;

public sealed class ResourceByResourceTypeIdSpec : Specification<Resource>
{
    public ResourceByResourceTypeIdSpec(int id)
    {
        Query
            .Where(r => r.ResourceTypeId == id)
            .Include(r => r.ResourceType);
    }
}