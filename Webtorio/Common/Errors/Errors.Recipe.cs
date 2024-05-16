using ErrorOr;

namespace Webtorio.Common.Errors;

public static partial class Errors
{
    public static class Recipe
    {
        public static Error NotAvailable => Error.Conflict(
            code: "Recipe.NotAvailable",
            description: "Recipe with requested id is not available for this building");
    }
}
