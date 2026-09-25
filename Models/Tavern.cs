using System.Collections.Generic;

namespace TavernSimulator.Models;

/// <summary>
/// Представляет таверну, хранит её состояние, запасы продуктов,
/// доступные блюда, золото и информацию о посетителях.
/// </summary>
public class Tavern
{
    public const string Name = "Гусь и пирог";
    public Dictionary<string, int> Products { get; set; } = new();
    public List<Dish>? AvailableDishes { get; set; } 
    public int Day { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int Level { get; set; } = 1;
    public int Gold { get; set; } = 100;
}