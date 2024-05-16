using System.ComponentModel.DataAnnotations;
using Ardalis.Specification;

namespace Webtorio.Models;

public abstract class Item : IEntity<int>
{
    public int Id { get; set; }
    [StringLength(100)]
    public string Name { get; init; } = null!;
}