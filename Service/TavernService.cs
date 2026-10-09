using System;
using System.Collections.Generic;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Game;
using TavernSimulator.Models;

namespace TavernSimulator.Service;

/// <summary>
/// Содержит логику управления таверной.
/// </summary>
public static class TavernService
{
    private static int _maxExperience = 100;

    /// <summary>
    /// Добавляет найденный предмет в список вещей, оставленных на хранение в таверне.
    /// </summary>
    /// <param name="tavern">Таверна, в которой найден предмет.</param>
    /// <param name="item">Найденный предмет.</param
    public static FoundItemResult AddFoundItem(this Tavern tavern, int choice, Item item)
    {
        if (choice == 1)
        {
            tavern.FoundItems?.Add(item);
            return FoundItemResult.Kept;
        }
        return FoundItemResult.ThrownAway;
    }
    
    /// <summary>
    /// Покупает продукт в указанном количестве, добавляет его в список имеющихся продуктов таверны и списывает у таверны золото за покупку.
    /// </summary>
    /// <param name="tavern">Таверна, в которой посетитель делает заказ.</param>
    /// <param name="product">Продукт, который сейчас покупается.</param>
    /// <param name="count">Количество покупаемого продукта.</param>
    /// <param name="price">Цена продукта, если он покупается не за фиксированную цену.</param>
    public static PurchaseProductResult BuyProducts(this Tavern tavern, Product product, int count, int price = -1)
    {
        tavern.Products.TryAdd(product.Name, 0);
        tavern.Products[product.Name] += count;
        if (price == 0) return PurchaseProductResult.PurchaseSuccess;
        if (price == -1)
        {
            tavern.Gold -= product.Price * count;
        }
        else
        {
            tavern.Gold -= price * count;
        }
        return PurchaseProductResult.ReceiveSuccess;
    }
    
    /// <summary>
    /// Проверяет, достаточно ли опыта для повышения уровня таверны.
    /// </summary>
    private static void CheckLevelUp(this Tavern tavern)
    {
        if (tavern.Experience >= _maxExperience)
        {
            tavern.Level++;
            tavern.Experience -= _maxExperience;
            _maxExperience *= 2;
        }
    }
    
    /// <summary>
    /// Готовит выбранное блюдо и изменяет запасы необходимых ингредиентов.
    /// </summary>
    /// <param name="tavern">Таверна, в которой готовится блюдо.</param>
    /// <param name="dish">Блюдо, которое необходимо приготовить.</param>
    /// <returns>Результат попытки приготовления блюда.</returns>
    public static CookResult CookDish(this Tavern tavern, Dish dish)
    {
        if (!tavern.AvailableDishes.Contains(dish)) return CookResult.RecipeNotLearned;

        foreach (var ingrid in dish.Ingredients)
        {
            if (!tavern.Products.ContainsKey(ingrid.Key)) return CookResult.UnknownIngredient;

            var product = tavern.Products.First(x => x.Key == ingrid.Key);
            if (product.Value < ingrid.Value) return CookResult.NotEnoughIngredients;
        }

        foreach (var ingrid in dish.Ingredients)
        {
            var product = tavern.Products.First(x => x.Key == ingrid.Key);
            tavern.Products[product.Key] -= ingrid.Value;
        }

        tavern.TotalDishesCooked++;
        AchievementService.IsAchievementUnlocked(AchievementRequirementType.DishesCooked, tavern);
        tavern.Experience += dish.Experience;
        tavern.CheckLevelUp();
        return CookResult.Success;
    }
    
    /// <summary>
    /// Обрабатывает приготовление всех блюд, входящих в заказ посетителя.
    /// </summary>
    /// <returns>Приготовлен ли заказ.</returns>
    public static bool CookOrder(this Tavern tavern, List<Dish> dishes)
    {
        foreach (var dish in dishes)
        {
            var result = tavern.CookDish(dish);
            GameOutput.ShowCookResult(result, dish);
            if (result != CookResult.Success) return false;
        }
        return true;
    }
    
    /// <summary>
    /// Создаёт новую таверну с начальными параметрами.
    /// </summary>
    public static void CreateTavern(this Tavern tavern)
    {
        var availiableProducts = ProductCatalog.Products
            .Where(x => x.RequiredTavernLevel == 1)
            .ToList();
        foreach (var product in availiableProducts)
        {
            tavern.Products.Add(product.Name, new Random().Next(4, 10));
        }

        tavern.AvailableDishes = DishCatalog.Dishes
            .Where(x => x.RequiredTavernLevel == 1 && x.Id.StartsWith("base_"))
            .ToList();
    }
    
    /// <summary>
    /// Находит найденную вещь, принадлежащую указанному посетителю.
    /// </summary>
    /// <param name="tavern">Таверна, в которой выполняется поиск.</param>
    /// <param name="visitor">Посетитель, чью вещь необходимо найти.</param>
    /// <returns>Найденная вещь или null, если вещь не найдена.</returns>
    public static Item? FindVisitorItem(this Tavern tavern, Visitor visitor)
    {
        var item = tavern.FoundItems.FirstOrDefault(item => item.OwnerType == visitor.TypeName && item.OwnerName == visitor.Name);
        if(item != null) tavern.ReturnItemToOwner(item);
        return item;
    }
    
    /// <summary>
    /// Изучает новое блюдо и добавляет его в список освоенных блюд таверны.
    /// </summary>
    public static LearnDishResult LearnDish(this Tavern tavern, Dish dish)
    {
        foreach (var ingrid in dish.Ingredients)
        {
            var product = tavern.Products.First(x => x.Key == ingrid.Key);
            tavern.Products[product.Key]--;
        }

        var choice = Random.Shared.Next(100);
        bool isLearned = choice switch
        {
            < 40 => true,
            _ => false
        };

        if (isLearned)
        {
            tavern.AvailableDishes.Add(dish);
            return LearnDishResult.Success;
        }
        return LearnDishResult.RecipeNotLearned;
    }
    
    /// <summary>
    /// Изучает новое секретное блюдо и добавляет его в список освоенных блюд таверны.
    /// </summary>
    public static void LearnSecretDish(this Tavern tavern, Dish dish)
    {
        tavern.AvailableDishes.Add(dish);
        DishCatalog.Dishes.Add(dish);
        GameOutput.ShowLearnDishResult(LearnDishResult.Success, dish);
    }

    /// <summary>
    /// Возвращает найденную вещь её владельцу и начисляет таверне вознаграждение.
    /// </summary>
    /// <param name="tavern">Таверна, в которой хранится найденная вещь.</param>
    /// <param name="item">Вещь, которую необходимо вернуть владельцу.</param>
    private static void ReturnItemToOwner(this Tavern tavern, Item item)
    {
        tavern.FoundItems!.Remove(item);
        tavern.Gold += item.OwnerPrice;
    }
    
    /// <summary>
    /// Продаёт посетителю необходимый продукт.
    /// </summary>
    public static SellProductResult SellRequestedProduct(this Tavern tavern, KeyValuePair<string, int> product, int count = 1, int price = -1)
    {
        if (product.Value < count) return SellProductResult.NotEnoughProducts;
        tavern.Products[product.Key] -= count;
        int tempPrice;
        if (price > 0) tempPrice = count * price;
        else
        {
            var tempProduct = ProductCatalog.Products.First(x => x.Name == product.Key);
            tempPrice = count * tempProduct.Price;
        }
        tavern.Gold += tempPrice;
        return SellProductResult.SaleSuccess;
    }
    
    
}