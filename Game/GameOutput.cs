using TavernSimulator.Enums;
using TavernSimulator.Models;

namespace TavernSimulator.Game;

/// <summary>
/// Содержит методы для отображения результатов игровых операций в консоли.
/// </summary>
public static class GameOutput
{
    
    /// <summary>
    /// Отображает результат покупки или получения продукта в консоли.
    /// </summary>
    /// <param name="result">Результат попытки покупки или получения продукта.</param>
    /// <param name="product">Полученный или выбранный для покупки продукт.</param>
    /// <param name="count">Количество полученного или выбранного для покупки продукта.</param>
    public static void ShowBuyProductResult(PurchaseProductResult result, Product product, int count)
    {
        switch (result)
        {
            case PurchaseProductResult.PurchaseSuccess:
                Console.WriteLine($"Получено {product.Name} - {count} шт.!");
                break;
            case PurchaseProductResult.ReceiveSuccess:
                Console.WriteLine($"Куплено {product.Name} - {count} шт.!");
                break;
            case PurchaseProductResult.NotEnoughGold:
                Console.WriteLine("Недостаточно золота!");
                break;
        }
    } 
    
    /// <summary>
    /// Отображает результат приготовления блюда в консоли.
    /// </summary>
    /// <param name="result">Результат попытки приготовления блюда.</param>
    /// <param name="dish">Приготовленное или выбранное для приготовления блюдо.</param>
    public static void ShowCookResult(CookResult result, Dish dish)
    {
        switch (result)
        {
            case CookResult.Success:
                Console.WriteLine($"Вы приготовили {dish.Name}!");
                break;
            case CookResult.NotEnoughIngredients:
                Console.WriteLine($"У вас недостаточно ингридиентов!");
                break;
            case CookResult.UnknownIngredient:
                Console.WriteLine($"Блюдо {dish.Name} содержит неизвестный вам ингридиент!");
                break;
            case CookResult.RecipeNotLearned:
                Console.WriteLine($"Вы еще не изучили рецепт блюда {dish.Name}!");
                break;
        }
    } 
    
 
    
    /// <summary>
    /// Отображает результат изучения рецепта блюда в консоли.
    /// </summary>
    /// <param name="result">Результат попытки изучения рецепта блюда.</param>
    /// <param name="dish">Изученное или выбранное для изучения блюдо.</param>
    public static void ShowLearnDishResult(LearnDishResult result, Dish dish)
    {
        switch (result)
        {
            case LearnDishResult.Success:
                Console.WriteLine($"Вы изучили {dish.Name}!");
                break;
            case LearnDishResult.RecipeNotLearned:
                Console.WriteLine($"Изучить {dish.Name} не удалось! Попробуйте ещё раз");
                break;
        }
    } 
    
    /// <summary>
    /// Отображает результат продажи продукта в консоли.
    /// </summary>
    /// <param name="result">Результат попытки продажи продукта.</param>
    /// <param name="product">Название выбранного для продажи продукта.</param>
    /// <param name="count">Количество выбранного для продажи продукта.</param>
    /// <param name="price">Цена выбранного для продажи продукта.</param>
    public static void ShowSellProductResult(SellProductResult result, string product, int count, int price)
    {
        switch (result)
        {
            case SellProductResult.SaleSuccess:
                Console.WriteLine($"Вы продали {product} {count} шт. и заработали {price} зол.");
                break;
            case SellProductResult.NotEnoughProducts:
                
                Console.WriteLine($"На складе недостаточно продукта {product}!");
                break;
        }
    }
}