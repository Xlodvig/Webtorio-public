using Ardalis.Specification;
using Webtorio.Models;

namespace Webtorio.Specifications.Deposits;

public sealed class DepositByIdReadOnlySpec : Specification<Deposit>
{
    public DepositByIdReadOnlySpec(int id)
    {
        Query
            .Where(d => d.Id == id)
            .Include(d => d.ResourceType)
            .Include(d => d.DepositSlots)
                .ThenInclude(bs => bs.Building)
                    .ThenInclude(b => b!.BuildingType)
            .AsNoTracking();
    }
}