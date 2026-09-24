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
        
    public static void CreateTavern(this Tavern tavern)
    {
        var availiableProducts = ProductCatalog.Products.Where(x => x.RequiredTavernLevel == 1).ToList();
        foreach (var product in availiableProducts)
        {
            tavern.Products.Add(product.Name, new Random().Next(4,10));
        }
        tavern.AvailableDishes = DishCatalog.Dishes.Where(x => x.RequiredTavernLevel == 1).ToList();
    }

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

    private static void CheckLevelUp(this Tavern tavern)
    {
        if (tavern.Experience >= _maxExperience)
        {
            tavern.Level++;
            tavern.Experience -= _maxExperience ;
            _maxExperience *= 2;
        }
    }
    
    public static bool CookOrder(this Tavern tavern, List<Dish> dishes)
    {
        foreach (var dish in dishes)
        {
            if (!tavern.CookDish(dish)) return false;
        }
        return true;
    }

}