using ErrorOr;

namespace Webtorio.Common.Errors;

public static partial class Errors
{
    public static class Building
    {
        public static Error NotImplemented(string buildingTypeName) => Error.Conflict(
            code: "BuildingType.Conflict",
            description: $"Creating Building with Type {buildingTypeName} not implemented yet :(");

        public static Error NotStored => Error.Conflict(
            code: "Building.NotStored",
            description: "The Building is not in a Stored State");
    }
}