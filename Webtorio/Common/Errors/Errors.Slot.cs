using ErrorOr;

namespace Webtorio.Common.Errors;

public static partial class Errors
{
    public static class Slot
    {
        public static Error BuildingNotFound(int slotId) => Error.NotFound(
            code: "Slot.BuildingNotFound",
            description: $"There is no Building in Slot with id = {slotId}");

        public static Error InappropriateBuilding => Error.Conflict(
            code: "Slot.InappropriateBuilding",
            description: "Inappropriate Building Type for this Slot");
        
        public static Error Occupied(int slotId) => Error.Conflict(
            code: "Slot.Occupied",
            description: $"There is another Building in Slot with id = {slotId}");
    }
}