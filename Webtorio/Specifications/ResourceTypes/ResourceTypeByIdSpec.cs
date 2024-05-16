using Ardalis.Specification;
using Webtorio.Models.StaticData;

namespace Webtorio.Specifications.ResourceTypes;

public sealed class ResourceTypeByIdSpec : Specification<ResourceType>
{
    public ResourceTypeByIdSpec(int id)
    {
        Query
            .Where(rt => rt.Id == id);
    }
}