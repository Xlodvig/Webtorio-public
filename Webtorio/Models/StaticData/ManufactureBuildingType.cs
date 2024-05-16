namespace Webtorio.Models.StaticData;

public class ManufactureBuildingType : BuildingType
{
    public double WorkSpeed { get; init; }
    public List<BuildingRecipeMatch> BuildingRecipeMatches { get; init; } = null!;
}