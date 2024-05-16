using System.Text.Json.Serialization;
using Webtorio.Models.Buildings;

namespace Webtorio.Contracts.ViewModels.Buildings;

public record GeneratorBuildingTypeViewModel(
    int Id,
    string Name,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Energy Energy,
    int? InputItemTypeId,
    double? InputAmount,
    int OutputItemTypeId,
    double OutputAmount);