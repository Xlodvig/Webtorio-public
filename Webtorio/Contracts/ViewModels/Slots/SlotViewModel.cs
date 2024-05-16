using Webtorio.Contracts.ViewModels.Buildings;

namespace Webtorio.Contracts.ViewModels.Slots;

public record SlotViewModel(
    int Id,
    string Title,
    BuildingViewModel? Building);