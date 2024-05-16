using Webtorio.Models.StaticData;

namespace Webtorio.Models;

public class Resource : Item
{
    public int ResourceTypeId { get; init; }
    public ResourceType ResourceType { get; init; } = null!;
    public double Amount { get; set; }
}