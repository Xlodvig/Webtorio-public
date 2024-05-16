namespace Webtorio.Contracts.Storage;

public record RemoveResourceRequest(
    int ResourceTypeId,
    double Amount);