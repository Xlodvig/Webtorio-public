using Ardalis.Specification;

namespace Webtorio.Models.Buildings;

public class Slot : IEntity<int>
{
    public int Id { get; set; }
    public string Title => BuildingId is not null ? Building!.Name : "Empty";
    public int? BuildingId { get; set; }
    public Building? Building { get; init; }
}