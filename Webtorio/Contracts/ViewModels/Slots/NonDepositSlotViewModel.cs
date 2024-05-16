using Webtorio.Contracts.ViewModels.Buildings;

namespace Webtorio.Contracts.ViewModels.Slots;

public record NonDepositSlotViewModel(
    int Id,
    string Title,
    ManufactureBuildingViewModel? ManufactureBuilding,
    GeneratorBuildingViewModel? GeneratorBuilding);