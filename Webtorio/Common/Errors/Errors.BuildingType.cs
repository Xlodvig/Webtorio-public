using ErrorOr;

namespace Webtorio.Common.Errors;

public static partial class Errors
{
    public static class BuildingType
    {
        public static Error NotManufacture => Error.Conflict(
            code: "BuildingType.NotManufacture",
            description: "Building with requested id must be ManufactureBuildingType");
    }
}