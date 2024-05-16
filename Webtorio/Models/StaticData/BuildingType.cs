using Webtorio.Models.Buildings;

namespace Webtorio.Models.StaticData;

public abstract class BuildingType : ItemType
{
    public Energy Energy { get; init; }
    public int EnergyConsumptionKW { get; init; }
    public bool IsAvailable { get; init; }
}