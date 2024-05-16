namespace Webtorio.Contracts.Storage;

public record AddResourceRequest(
    int ResourceTypeId,
    double Amount);