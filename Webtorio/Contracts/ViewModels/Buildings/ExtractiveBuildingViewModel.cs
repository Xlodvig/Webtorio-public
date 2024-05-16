using System.Text.Json.Serialization;
using Webtorio.Models.Buildings;

namespace Webtorio.Contracts.ViewModels.Buildings;

public record ExtractiveBuildingViewModel(
    int Id,
    string Name,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    BuildingState State,
    BuildingTypeViewModel BuildingType,
    double MiningSpeed,
    double ResourceBuffer,
    double BurnerEnergyReserveOnTick);