namespace Webtorio.Contracts.ViewModels.Buildings;

public record BuildingViewModel(
    ExtractiveBuildingViewModel? ExtractiveBuilding,
    ManufactureBuildingViewModel? ManufactureBuilding,
    GeneratorBuildingViewModel? GeneratorBuilding);