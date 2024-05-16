using Ardalis.Specification;
using Webtorio.Models.Buildings;
using Webtorio.Models.Recipes;

namespace Webtorio.Specifications.Slots;

public sealed class NonDepositSlotsReadOnlySpec : Specification<Slot>
{
    public NonDepositSlotsReadOnlySpec()
    {
        Query
            .Where(s => !(s is DepositSlot))
            .Include(s => s.Building)
                .ThenInclude(b => b!.BuildingType)
            
            .Include(s => s.Building)
                .ThenInclude<Slot, Building?, Recipe>(b => (b as ManufactureBuilding)!.SelectedRecipe!)
                    .ThenInclude(r => r.RecipeItems)
                        .ThenInclude(ri => ri.ItemType)
            .AsNoTracking();
    }
}