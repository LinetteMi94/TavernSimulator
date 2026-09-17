using TavernSimulator.Data;
using TavernSimulator.Models;

namespace TavernSimulator.Service;

/// <summary>
/// Содержит логику управления таверной.
/// </summary>
public static class TavernService
{
    
    public static void CreateTavern(this Tavern tavern)
    {
        var productCatalog = new ProductCatalog();
        var dishCatalog = new DishCatalog();
        tavern.Products = productCatalog.Products.Where(x => x.RequiredTavernLevel == 1).ToList();
        tavern.AvailableDishes = dishCatalog.Dishes.Where(x => x.RequiredTavernLevel == 1).ToList();
        Console.WriteLine("Доступные продукты таверны:");
        foreach (var product in tavern.Products)
            Console.WriteLine(product.Name + ' ' + product.Price);
        Console.WriteLine("\nДоступные блюда таверны:");
        foreach (var dish in tavern.AvailableDishes)
            Console.WriteLine(dish.Name + ' ' + dish.Price);
    }
}