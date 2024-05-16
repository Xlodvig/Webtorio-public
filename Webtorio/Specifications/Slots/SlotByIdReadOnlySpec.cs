using Ardalis.Specification;
using Webtorio.Models.Buildings;

namespace Webtorio.Specifications.Slots;

public sealed class SlotByIdReadOnlySpec<T> : Specification<T>, ISingleResultSpecification<T> where T : Slot
{
    public SlotByIdReadOnlySpec(int id)
    {
        Query
            .Where(s => s.Id == id)
            .Include(s => s.Building)
                .ThenInclude(b => b!.BuildingType)
            .AsNoTracking();
    }
}