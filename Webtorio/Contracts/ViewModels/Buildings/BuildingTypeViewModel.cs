using System.Text.Json.Serialization;
using Webtorio.Models.Buildings;

namespace Webtorio.Contracts.ViewModels.Buildings;

public record BuildingTypeViewModel(
    int Id,
    string Name,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Energy Energy);