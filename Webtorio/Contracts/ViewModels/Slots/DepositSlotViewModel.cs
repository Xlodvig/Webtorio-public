using Webtorio.Contracts.ViewModels.Buildings;

namespace Webtorio.Contracts.ViewModels.Slots;

public record DepositSlotViewModel(
    int Id,
    string Title,
    ExtractiveBuildingViewModel? Building);