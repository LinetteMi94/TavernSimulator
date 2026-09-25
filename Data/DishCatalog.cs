using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using TavernSimulator.Models;

namespace TavernSimulator.Data;

/// <summary>
/// Содержит полный каталог блюд, доступных в игре.
/// </summary>
public static class DishCatalog
{
    public static List<Dish>? Dishes { get; set; } 
       
    static DishCatalog()
    {
        string json = File.ReadAllText("Data/DishCatalogJSON.json");
        Dishes = JsonSerializer.Deserialize<List<Dish>>(json);
    }    
}