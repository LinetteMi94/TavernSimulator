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
    /// Создаёт новую таверну с начальными параметрами.
    /// </summary>
    public static void CreateTavern(this Tavern tavern)
    {
        var availiableProducts = ProductCatalog.Products.Where(x => x.RequiredTavernLevel == 1).ToList();
        foreach (var product in availiableProducts)
        {
            tavern.Products.Add(product.Name, new Random().Next(4,10));
        }
        tavern.AvailableDishes = DishCatalog.Dishes.Where(x => x.RequiredTavernLevel == 1).ToList();
    }

    /// <summary>
    /// Готовит выбранное блюдо и изменяет запасы необходимых ингредиентов.
    /// </summary>
    /// <returns>Приготовлено ли блюдо.</returns>
    private static bool CookDish(this Tavern tavern, Dish dish)
    {
        if (!tavern.AvailableDishes.Contains(dish))
        {
            Console.WriteLine($"Вы еще не изучили рецепт блюда {dish.Name}!");
            return false;
        }
        foreach (var ingrid in dish.Ingredients)
        {
            if  (!tavern.Products.ContainsKey(ingrid.Key))
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
    /// Проверяет, достаточно ли опыта для повышения уровня таверны.
    /// </summary>
    private static void CheckLevelUp(this Tavern tavern)
    {
        if (tavern.Experience >= _maxExperience)
        {
            tavern.Level++;
            tavern.Experience -= _maxExperience ;
            _maxExperience *= 2;
        }
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
}