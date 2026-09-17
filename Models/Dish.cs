namespace TavernSimulator.Models;

/// <summary>
/// Представляет блюдо, которое можно приготовить и продать в таверне.
/// </summary>
public class Dish
{
    public string Id { get; set; }
    public string? Name { get; set; } 
    public int Price { get; set; } 
    public int RequiredTavernLevel { get; set; }
    public int Experience { get; set; }
    public Dictionary<string, int>? Ingredients { get; set; } = new();
}