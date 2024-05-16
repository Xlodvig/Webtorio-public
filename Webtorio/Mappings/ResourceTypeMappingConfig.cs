using Mapster;
using Webtorio.Contracts.ViewModels;
using Webtorio.Models.StaticData;

namespace Webtorio.Mappings;

public class ResourceTypeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config) => 
        config.NewConfig<ResourceType, ResourceTypeViewModel>();
}