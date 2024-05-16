using Mapster;
using Webtorio.Contracts.ViewModels.Buildings;
using Webtorio.Models.StaticData;

namespace Webtorio.Mappings;

public class BuildingTypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<BuildingType, BuildingTypeViewModel>();
        
        config.NewConfig<GeneratorBuildingType, GeneratorBuildingTypeViewModel>();
    }
}