using Mapster;
using Webtorio.Contracts.ViewModels.Slots;
using Webtorio.Models.Buildings;

namespace Webtorio.Mappings;

public class SlotMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Slot, SlotViewModel>();
        
        config.NewConfig<DepositSlot, DepositSlotViewModel>()
            .Map(
            dest => dest.Building,
            source => source.ExtractiveBuilding);

        config.NewConfig<Slot, NonDepositSlotViewModel>()
            .Map(
                dest => dest.ManufactureBuilding,
                source => source.Building as ManufactureBuilding,
                source => source.Building is ManufactureBuilding)
            .Map(
                dest => dest.GeneratorBuilding,
                source => source.Building as GeneratorBuilding,
                source => source.Building is GeneratorBuilding);
    }
}