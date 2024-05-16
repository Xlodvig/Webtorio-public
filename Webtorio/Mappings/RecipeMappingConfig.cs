using Mapster;
using Webtorio.Contracts.ViewModels;
using Webtorio.Models.Recipes;

namespace Webtorio.Mappings;

public class RecipeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Recipe, RecipeViewModel>()
            .Map(dest => dest.Name,
                source => source.Name);

        config.NewConfig<RecipeItem, RecipeItemViewModel>()
            .Map(dest => dest.ItemName,
                source => source.ItemType.Name);
    }
}