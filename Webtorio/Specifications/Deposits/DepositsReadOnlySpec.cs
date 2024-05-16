using Ardalis.Specification;
using Webtorio.Models;

namespace Webtorio.Specifications.Deposits;

public sealed class DepositsReadOnlySpec : Specification<Deposit>
{
    public DepositsReadOnlySpec()
    {
        Query
            .Include(d => d.ResourceType)
            .AsNoTracking();
    }
}