using Ardalis.Specification;
using Webtorio.Models;

namespace Webtorio.Specifications.Resources;

public sealed class ResourceWithResourceTypeByResourceTypeIdSpec : Specification<Resource>
{
    public ResourceWithResourceTypeByResourceTypeIdSpec(int id)
    {
        Query
            .Where(r => r.ResourceTypeId == id)
            .Include(r => r.ResourceType);
    }
}