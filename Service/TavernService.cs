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

    public static bool CookDish(this Tavern tavern, string dishName)
    {
        var HaveProduct = true;
        Dish dish = DishCatalog.Dishes.First(x => x.Name == dishName);
        if (!tavern.AvailableDishes.Contains(dish))
        {
            Console.WriteLine($"Вы еще не изучили рецепт блюда {dishName}!");
            return false;
        }
        foreach (var ingrid in dish.Ingredients)
        {
            if  (!tavern.Products.ContainsKey(ingrid.Key))
            {
                Console.WriteLine(ingrid.Key);
                Console.WriteLine($"Вы не знаете ингридиентов, из которых готовят {dishName}!");
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
        Console.WriteLine($"Вы приготовили {dishName}!");
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
}