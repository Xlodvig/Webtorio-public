using Webtorio.Contracts.ViewModels.Slots;

namespace Webtorio.Contracts.ViewModels;

public record DepositViewModel(
    int Id,
    ResourceTypeViewModel ResourceType,
    double ResourceAmount,
    IEnumerable<DepositSlotViewModel> DepositSlots);