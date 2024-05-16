using Ardalis.Specification;
using Webtorio.Models.StaticData;

namespace Webtorio.Specifications.Buildings;

public sealed class BuildingTypeByIdSpec : Specification<BuildingType>, ISingleResultSpecification
{
    public BuildingTypeByIdSpec(int id) => 
        Query.Where(bt => bt.Id == id);
}