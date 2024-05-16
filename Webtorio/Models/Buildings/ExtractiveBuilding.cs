using ErrorOr;
using Webtorio.Application.Buildings.Services;
using Webtorio.Application.Buildings.Services.StateMachine;
using Webtorio.Application.Interfaces;
using Webtorio.Common.Errors;
using Webtorio.Models.StaticData;
using Webtorio.Specifications.Deposits;

namespace Webtorio.Models.Buildings;

public class ExtractiveBuilding : Building
{
    public double MiningSpeed { get; init; }
    public double ResourceBuffer { get; set; }
    public DepositSlot? DepositSlot => Slot as DepositSlot;
    
    public ExtractiveBuilding(string name, BuildingType buildingType)
    {
        Name = name;
        BuildingType = buildingType;
        MiningSpeed = (buildingType as ExtractiveBuildingType)!.MiningSpeed;
    }

    public override async Task<CheckResult> CheckConsumptionsAsync(IRepository repository,
        CancellationToken cancellationToken) =>
        await EnsureEnergyAsync(this, repository, cancellationToken);

    public override async Task<ErrorOr<Success>> UpdateAsync(IRepository repository,
        BuildingWorkService buildingWorkService, CancellationToken cancellationToken)
    {
        ResourceBuffer += MiningSpeed;

        if (ResourceBuffer < 1)
        {
            if (BuildingType.Energy == Energy.Burner) 
                BurnerEnergyReserveOnTick -= 1;
            
            return Result.Success;
        }
        
        if (DepositSlot?.Deposit is null)
            return Errors.Common.NotFound<Deposit>();

        var depositResult = await repository
            .GetAsync(new DepositByIdSpec(DepositSlot.DepositId), cancellationToken);

        if (depositResult.IsError)
            return depositResult.Errors;
            
        var result = await depositResult.Value
            .MineAndStoreResourceAsync((int)ResourceBuffer, repository, buildingWorkService, cancellationToken);

        if (result.IsError)
            return result.Errors;

        if (BuildingType.Energy == Energy.Burner && BurnerEnergyReserveOnTick is not null)
            BurnerEnergyReserveOnTick -= 1;
        
        ResourceBuffer -= (int)ResourceBuffer;

        return Result.Success;
    }
    
    private ExtractiveBuilding(){}
}