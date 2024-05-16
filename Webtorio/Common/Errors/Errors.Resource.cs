using ErrorOr;

namespace Webtorio.Common.Errors;

public static partial class Errors
{
    public static class Resource
    {
        public static Error TypeNotFound => Error.NotFound(
            code: "Resource.TypeNotFound",
            description: "Unknown specified resource type");       
        
        public static Error AmountInsufficiency(int resourceTypeId) => Error.Conflict(
            code: "Resource.AmountInsufficiency",
            description: $"Resource amount with resource type id {resourceTypeId} insufficiency");
    }
}