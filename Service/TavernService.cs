using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Models;

namespace TavernSimulator.Service;

/// <summary>
/// Содержит логику управления таверной.
/// </summary>
public static class TavernService
{
    
    public static void CreateTavern(this Tavern tavern)
    {
        tavern.AvailableDishes = DishCatalog.Dishes;
    }
}