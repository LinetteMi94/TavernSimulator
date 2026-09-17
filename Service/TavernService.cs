using TavernSimulator.Data;
using TavernSimulator.Models;

namespace TavernSimulator.Service;

/// <summary>
/// Содержит логику управления таверной.
/// </summary>
public static class TavernService
{
    public static readonly ProductCatalog ProductCatalog = new ();
    private static readonly DishCatalog DishCatalog = new ();
    
    public static void CreateTavern(this Tavern tavern)
    {
        var availiableProducts = ProductCatalog.Products.Where(x => x.RequiredTavernLevel == 1).ToList();
        foreach (var product in availiableProducts)
        {
            tavern.Products.Add(product.Name, new Random().Next(4,10));
        }
        tavern.AvailableDishes = DishCatalog.Dishes.Where(x => x.RequiredTavernLevel == 1).ToList();
        
    }
}