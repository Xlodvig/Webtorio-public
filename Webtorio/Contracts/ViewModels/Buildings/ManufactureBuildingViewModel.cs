using System.Text.Json.Serialization;
using Webtorio.Models.Buildings;

namespace Webtorio.Contracts.ViewModels.Buildings;

public record ManufactureBuildingViewModel(
    int Id,
    string Name,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    BuildingState State,
    BuildingTypeViewModel BuildingType,
    double WorkSpeed,
    RecipeViewModel? SelectedRecipe,
    double? TicksBeforeWorkIsDone,
    double BurnerEnergyReserveOnTick);