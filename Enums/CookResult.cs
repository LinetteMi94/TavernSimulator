namespace TavernSimulator.Enums;

/// <summary>
/// Представляет результаты попытки приготовления блюда.
/// </summary>
public enum CookResult
{
    RecipeNotLearned,
    UnknownIngredient,
    NotEnoughIngredients,
    Success
}