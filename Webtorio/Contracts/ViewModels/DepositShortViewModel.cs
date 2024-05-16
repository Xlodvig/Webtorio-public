namespace Webtorio.Contracts.ViewModels;

public record DepositShortViewModel(
    int Id,
    ResourceTypeViewModel ResourceType,
    double ResourceAmount);