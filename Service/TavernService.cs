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
        tavern.Products = productCatalog.Products.Where(x => x.RequiredTavernLevel == 1).ToList();
        tavern.AvailableDishes = DishCatalog.Dishes;
    }
}