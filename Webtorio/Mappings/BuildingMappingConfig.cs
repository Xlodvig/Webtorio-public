using Mapster;
using Webtorio.Contracts.ViewModels.Buildings;
using Webtorio.Models.Buildings;

namespace Webtorio.Mappings;

public class BuildingMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ExtractiveBuilding, ExtractiveBuildingViewModel>();

        config.NewConfig<ManufactureBuilding, ManufactureBuildingViewModel>();
        
        config.NewConfig<GeneratorBuilding, GeneratorBuildingViewModel>();

        config.NewConfig<Building, BuildingViewModel>()
            .Map(
                dest => dest.ExtractiveBuilding,
                source => source as ExtractiveBuilding,
                source => source is ExtractiveBuilding)
            .Map(
                dest => dest.ManufactureBuilding,
                source => source as ManufactureBuilding,
                source => source is ManufactureBuilding)
            .Map(
                dest => dest.GeneratorBuilding,
                source => source as GeneratorBuilding,
                source => source is GeneratorBuilding);
    }
}