using ErrorOr;

namespace Webtorio.Common.Errors;

public static partial class Errors
{
    public static class BuildingWork
    {
        public static Error ErrorGettingState => Error.Failure(
            code: "BuildingWork.ErrorGettingState",
            description: "Error while getting state from state machine");
        
        public static Error ErrorChangeStoredState => Error.Conflict(
            code: "BuildingWork.ErrorChangeStoredState",
            description: "You cannot turn on/off a building in a Stored State");
    }
}