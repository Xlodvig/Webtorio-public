using System.ComponentModel.DataAnnotations;
using Ardalis.Specification;

namespace Webtorio.Models.StaticData;

public abstract class ItemType : IEntity<int>
{
    public int Id { get; set; }
    [StringLength(100)]
    public string Name { get; init; } = null!;
}