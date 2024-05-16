using ErrorOr;

namespace Webtorio.Common.Errors;

public static partial class Errors
{
    public static class Common
    {
        public static Error NotFound<T>() => Error.NotFound(
            code: $"{typeof(T).Name}.NotFound",
            description: $"Entity {typeof(T).Name} with requested id not found");
        
        public static Error NotFound<T>(int id) => Error.NotFound(
            code: $"{typeof(T).Name}.NotFound",
            description: $"Entity {typeof(T).Name} with id = {id} not found");
    }
}