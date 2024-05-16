using ErrorOr;
using Webtorio.Models.Buildings;

namespace Webtorio.Application.Buildings.Services.StateMachine;

public class CheckResult
{
    public bool IsSuccess { get; init; }
    public BuildingState OnFailureTarget { get; init; }
    public List<Error>? Failures { get; init; }

    public static CheckResult Success() => 
        new() { IsSuccess = true };

    public static CheckResult Failure(BuildingState buildingState, List<Error> errors) =>
        new()
        {
            IsSuccess = false,
            OnFailureTarget = buildingState,
            Failures = errors,
        };
}