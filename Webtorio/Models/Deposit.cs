using Ardalis.Specification;
using ErrorOr;
using Webtorio.Application.Buildings.Services;
using Webtorio.Application.Interfaces;
using Webtorio.Models.Buildings;
using Webtorio.Models.StaticData;

namespace Webtorio.Models;

public class Deposit : IEntity<int>
{
    public int Id { get; set; }
    public int ResourceTypeId { get; init; }
    public ResourceType ResourceType { get; init; } = null!;
    public double ResourceAmount { get; set; }
    public List<DepositSlot> DepositSlots { get; init; }

    public Deposit(int resourceTypeId, double resourceAmount, int depositSlotsAmount = 9)
    {
        ResourceTypeId = resourceTypeId;
        ResourceAmount = resourceAmount;
        DepositSlots = new List<DepositSlot>();
        AddDepositSlots(depositSlotsAmount);
    }
    
    public bool ExtractResource(double amount)
    {
        if (ResourceAmount - amount >= 0)
        {
            ResourceAmount -= amount;
            return true;
        }

        return false;
    }

    private void AddDepositSlots(int amount = 9)
    {
        var newDepositSlots = new List<DepositSlot>();
        
        for (var i = 0; i < amount; i++) 
            newDepositSlots.Add(new DepositSlot());

        DepositSlots.AddRange(newDepositSlots);
    }
    
    public async Task<ErrorOr<Success>> MineAndStoreResourceAsync(double resourceAmount, IRepository repository,
        BuildingWorkService buildingWorkService, CancellationToken cancellationToken)
    {
        if (ResourceAmount - resourceAmount < 0)
            resourceAmount = ResourceAmount;

        ResourceAmount -= resourceAmount;

        var addResult = await repository.AddResourceAsync(ResourceTypeId, resourceAmount, cancellationToken);

        if (addResult.IsError)
            return addResult.Errors;
        
        if (ResourceAmount == 0)
        {
            var result = await StoreBuildingsFromDepositSlotsAsync(buildingWorkService, cancellationToken);

            if (result.IsError)
                return result.Errors;
            
            repository.Delete(this);
        }

        return Result.Success;
    }

    private async Task<ErrorOr<Success>> StoreBuildingsFromDepositSlotsAsync(BuildingWorkService buildingWorkService, 
        CancellationToken cancellationToken)
    {
        var errors = new List<Error>();

        foreach (var slot in DepositSlots.Where(ds => ds.Building is not null))
        {
            var result = await buildingWorkService.StoreBuildingFromSlotAsync(slot, cancellationToken);
           
            if (result.IsError) 
                errors.AddRange(result.Errors);
        }

        if (errors.Any())
            return errors;

        return Result.Success;
    }
    
    private Deposit(){}
}
