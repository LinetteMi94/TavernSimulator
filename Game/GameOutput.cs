using System;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Input;
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
    public static void ShowAchievements()
    {
        Console.Clear();
        var achievements = AchievementCatalog.Achievements;
        var counter = 1;
        foreach (var achievement in achievements)
        {
            Console.Write(achievement.IsUnlocked ? "✅" : "❌");
            Console.WriteLine($" {counter}. {achievement.Name} - {achievement.Description}");
            counter++;
        }
        InputValidator.Continue();
    } 
    
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
    /// Отображает информацию о найденном предмете и доступные действия с ним.
    /// </summary>
    /// <param name="item">Найденный предмет.</param>
    public static void ShowFoundItem(FoundItemResult result, Item item)
    {
        switch (result)
        {
            case FoundItemResult.Kept:
                Console.WriteLine($"Вы убрали «{item.Name}» за прилавок. Авось пригодится.");
                break;
            case FoundItemResult.ThrownAway:
                Console.WriteLine($"«{item.Name}» отправился прямиком в мусорное ведро.");
                break;
            case FoundItemResult.Found:
                Console.WriteLine($"После ухода посетителя вы обнаружили: {item.Name}.\n{item.Description}");
                break; 
            case FoundItemResult.ReturnedToOwner:
                Console.WriteLine($"Вы вернули «{item.Name}» владельцу. Тот в благодарность заплатил вам {item.OwnerPrice} золотых.");
                break;
            case FoundItemResult.Sold:
                Console.WriteLine($"Торговец забрал «{item.Name}» и отсчитал вам {item.Value} зол.");
                break;
        }
    }

    /// <summary>
    /// Отображает список найденных вещей, оставленных на хранение в таверне.
    /// </summary>
    /// <param name="tavern">Таверна, список найденных вещей которой необходимо отобразить.</param>
    public static void ShowFoundItems(Tavern tavern)
    {
        Console.Clear();
        Console.WriteLine("\nХлам под прилавком: \n");
        if (tavern.FoundItems.Count == 0) Console.WriteLine("Под прилавком лежит лишь пыль...");
        else
        {
            var counter = 1;
            foreach (var item in tavern.FoundItems.OrderBy(pr => pr.Name))
            {
                Console.WriteLine($"{counter}. {item.Name} - ({item.Value} зол.)");
                Console.WriteLine($"{item.Description}");
                Console.WriteLine($"Владелец: {item.OwnerType} {item.OwnerName}\n");
            }
        }
        InputValidator.Continue();
    }
    
    // <summary>
    /// Отображает список продуктов, хранящихся на складе таверны.
    /// </summary>
    public static void ShowTheFoodStorage(Tavern tavern)
    {
        Console.Clear();
        Console.WriteLine("\nПродуктовый склад: \n");
        if (tavern.Products.Count == 0) Console.WriteLine("Продуктовый склад пуст!");
        else
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"|  {"Продукт",-20}|{"Количество",14} |");
            Console.WriteLine(new string('-', 40));
            foreach (var item in tavern.Products.OrderBy(pr => pr.Key))
            {
                Console.WriteLine($"|  {item.Key,-20}|{item.Value,10} шт. |");
            }
            Console.WriteLine(new string('-', 40));
        }
        InputValidator.Continue();
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