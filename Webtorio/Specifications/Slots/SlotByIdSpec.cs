using Ardalis.Specification;
using Webtorio.Models.Buildings;

namespace Webtorio.Specifications.Slots;

public sealed class SlotByIdSpec<T> : Specification<T>, ISingleResultSpecification<T> where T : Slot
{
    public SlotByIdSpec(int id)
    {
        Query
            .Where(s => s.Id == id)
            .Include(s => s.Building)
                .ThenInclude(b => b!.BuildingType);
    }
}