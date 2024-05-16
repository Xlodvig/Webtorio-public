namespace Webtorio.Models.StaticData;

public class GeneratorBuildingType : BuildingType
{
    public int? InputItemTypeId { get; init; }
    public double? InputAmount { get; init; }
    public int OutputItemTypeId { get; init; }
    public double OutputAmount { get; init; }
}