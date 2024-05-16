using Mapster;
using Webtorio.Contracts.ViewModels;
using Webtorio.Models;

namespace Webtorio.Mappings;

public class DepositMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Deposit, DepositViewModel>();

        config.NewConfig<Deposit, DepositShortViewModel>();
    }
}