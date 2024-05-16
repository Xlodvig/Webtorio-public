using System.Text.Json.Serialization;
using Webtorio.Models.Buildings;

namespace Webtorio.Contracts.ViewModels.Buildings;

public record GeneratorBuildingViewModel(
    int Id,
    string Name,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    BuildingState State,
    GeneratorBuildingTypeViewModel GeneratorBuildingType);