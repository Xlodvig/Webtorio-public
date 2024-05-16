namespace Webtorio.Contracts.ViewModels;

public record RecipeViewModel(
    int Id,
    string Name,
    List<RecipeItemViewModel> InputItems,
    List<RecipeItemViewModel> OutputItems,
    double CraftingTime);