using Ardalis.Specification;
using Webtorio.Models;

namespace Webtorio.Specifications.Deposits;

public sealed class DepositByIdSpec : Specification<Deposit>
{
    public DepositByIdSpec(int id)
    {
        Query
            .Where(d => d.Id == id)
            .Include(d => d.ResourceType)
            .Include(d => d.DepositSlots)
                .ThenInclude(bs => bs.Building)
                    .ThenInclude(b => b!.BuildingType);
    }
}