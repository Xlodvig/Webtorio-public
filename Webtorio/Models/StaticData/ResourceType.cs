namespace Webtorio.Models.StaticData;

public class ResourceType : ItemType
{
    public bool IsFuel { get; init; }
    public double? FuelValueMJ { get; init; }
    public bool IsNoSolid { get; init; }
}