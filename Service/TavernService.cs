using System;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Models;

namespace TavernSimulator.Service;

/// <summary>
/// Содержит логику управления таверной.
/// </summary>
public static class TavernService
{
    private static int _maxExperience = 100;

    /// <summary>
    /// Покупает продукт в указанном количестве, добавляет его в список имеющихся продуктов таверны и списывает у таверны золото за покупку.
    /// </summary>
    /// <param name="tavern">Таверна, в которой посетитель делает заказ.</param>
    /// <param name="product">Продукт, который сейчас покупается.</param>
    /// <param name="count">Количество покупаемого продукта.</param>
    /// <param name="price">Цена продукта, если он покупается не за фиксированную цену.</param>
    public static void BuyProducts(this Tavern tavern, Product product, int count, int price = -1)
    {
        tavern.Products.TryAdd(product.Name, 0);
        tavern.Products[product.Name] += count;
        if (price == 0)
        {
            Console.WriteLine($"Получено {product.Name} - {count} шт.!");
            return;
        }
        if (price == -1)
        {
            tavern.Gold -= product.Price * count;
        }
        else
        {
            tavern.Gold -= price * count;
        }
        Console.WriteLine($"Куплено {product.Name} - {count} шт.!");
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
    /// <returns>Приготовлено ли блюдо.</returns>
    public static bool CookDish(this Tavern tavern, Dish dish)
    {
        if (!tavern.AvailableDishes.Contains(dish))
        {
            Console.WriteLine($"Вы еще не изучили рецепт блюда {dish.Name}!");
            return false;
        }

        foreach (var ingrid in dish.Ingredients)
        {
            if (!tavern.Products.ContainsKey(ingrid.Key))
            {
                Console.WriteLine(ingrid.Key);
                Console.WriteLine($"Вы не знаете ингридиентов, из которых готовят {dish.Name}!");
                return false;
            }

            var product = tavern.Products.First(x => x.Key == ingrid.Key);
            if (product.Value < ingrid.Value)
            {
                Console.WriteLine($"У вас недостаточно {product.Key}!");
                return false;
            }
        }

        foreach (var ingrid in dish.Ingredients)
        {
            var product = tavern.Products.First(x => x.Key == ingrid.Key);
            tavern.Products[product.Key] -= ingrid.Value;
        }

        Console.WriteLine($"Вы приготовили {dish.Name}!");
        tavern.Experience += dish.Experience;
        tavern.CheckLevelUp();
        return true;
    }
    
    /// <summary>
    /// Обрабатывает приготовление всех блюд, входящих в заказ посетителя.
    /// </summary>
    /// <returns>Приготовлен ли заказ.</returns>
    public static bool CookOrder(this Tavern tavern, List<Dish> dishes)
    {
        foreach (var dish in dishes)
        {
            if (!tavern.CookDish(dish)) return false;
        }

        return true;
    }
    
    /// <summary>
    /// Создаёт новую таверну с начальными параметрами.
    /// </summary>
    public static void CreateTavern(this Tavern tavern)
    {
        var availiableProducts = ProductCatalog.Products.Where(x => x.RequiredTavernLevel == 1).ToList();
        foreach (var product in availiableProducts)
        {
            tavern.Products.Add(product.Name, new Random().Next(4, 10));
        }

        tavern.AvailableDishes = DishCatalog.Dishes.Where(x => x.RequiredTavernLevel == 1).ToList();
    }
    
    /// <summary>
    /// Изучает новое блюдо и добавляет его в список освоенных блюд таверны.
    /// </summary>
    public static void LearnDish(this Tavern tavern, Dish dish)
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
            Console.WriteLine($"Вы изучили {dish.Name}!");
        }
        else Console.WriteLine($"Изучить {dish.Name} не удалось! Попробуйте ещё раз");
    }
    
    /// <summary>
    /// Изучает новое секретное блюдо и добавляет его в список освоенных блюд таверны.
    /// </summary>
    public static void LearnSecretDish(this Tavern tavern, Dish dish)
    {
        tavern.AvailableDishes.Add(dish);
        DishCatalog.Dishes.Add(dish);
        Console.WriteLine($"Вы изучили {dish.Name}!");
        
    }

    /// <summary>
    /// Продаёт посетителю необходимый продукт.
    /// </summary>
    public static bool SellRequestedProduct(this Tavern tavern, KeyValuePair<string, int> product, int count = 1, int price = -1)
    {
        if (product.Value < count)
        {
            Console.WriteLine($"На складе недостаточно продукта {product.Key}!");
            return false;
        }
        tavern.Products[product.Key] -= count;
        int tempPrice;
        if (price > 0) tempPrice = count * price;
        else
        {
            var tempProduct = ProductCatalog.Products.First(x => x.Name == product.Key);
            tempPrice = count * tempProduct.Price;
        }
        tavern.Gold += tempPrice;
        Console.WriteLine($"Вы продали {product.Key} {count} шт. и заработали {tempPrice} зол.");
        return true;
    }
    
    /// <summary>
    /// Показывает необходимые ингридиенты для списка блюд и количество необходимых ингридиентов в таверне.
    /// </summary>
    /// <param name="dishes">Список блюд, ингредиенты которых необходимо отобразить.</param>
    public static void ShowDishesIngridients(this Tavern tavern, List<Dish> dishes)
    {
        Console.Clear();
        foreach (var dish in dishes)
        {
            tavern.ShowDishIngridients(dish);
        }
    }
    
    /// <summary>
    /// Показывает необходимые ингридиенты для блюда и количество необходимых ингридиентов в таверне.
    /// </summary>
    /// <param name="dish">Блюдо, ингредиенты которых необходимо отобразить.</param>
    public static void ShowDishIngridients(this Tavern tavern, Dish dish)
    {
        if (!tavern.AvailableDishes.Contains(dish))
        {
            Console.WriteLine($"Вы не знаете рецепта для блюда {dish.Name}");
            return;
        }
        Console.WriteLine($"\nИнгридиенты для блюда {dish.Name}:\n");
        var counter = 1;
        foreach (var ingrid in dish.Ingredients)
        { 
            var product = tavern.Products.Where(x => x.Key == ingrid.Key).Select(x =>  x.Value).FirstOrDefault();
            Console.WriteLine($"{counter++}. {ingrid.Key, -20} -{ingrid.Value,3} шт.     (В наличии {product,2} шт.)");
        }
        Console.WriteLine();
    }
}