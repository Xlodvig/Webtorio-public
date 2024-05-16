using Ardalis.Specification;
using Webtorio.Models;

namespace Webtorio.Specifications.Resources;

public sealed class StoredResourcesReadOnlySpec : Specification<Resource>
{
    public StoredResourcesReadOnlySpec()
    {
        Query
            .AsNoTracking();
    }
}