using Ardalis.Specification;
using Webtorio.Models.Buildings;

namespace Webtorio.Specifications.Buildings;

public sealed class BuildingByIdSpec : Specification<Building>, ISingleResultSpecification
{
    public BuildingByIdSpec(int id)
    {
        Query
            .Include(b => b.BuildingType)
            .Include(b => b.Slot)
                .ThenInclude(s => (s as DepositSlot)!.Deposit)
                    .ThenInclude(d => d!.DepositSlots)
            .Include(b => b.SelectedFuelResourceType)
            .Include(b => ((ManufactureBuilding)b).SelectedRecipe)
                .ThenInclude(r => r!.RecipeItems)
            .Where(b => b.Id == id);
    }
}