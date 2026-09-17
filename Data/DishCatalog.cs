using System.Text.Json;
using TavernSimulator.Enums;
using TavernSimulator.Models;

namespace TavernSimulator.Data;

/// <summary>
/// Содержит все блюда, доступные в игре.
/// </summary>
public class DishCatalog
{
    public List<Dish>? Dishes { get; set; } 
       
    public DishCatalog()
    {
        string json = File.ReadAllText("Data/DishCatalogJSON.json");
        Dishes = JsonSerializer.Deserialize<List<Dish>>(json);
    }    
}