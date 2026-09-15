using TavernSimulator.Enums;

namespace TavernSimulator.Models;

/// <summary>
/// Представляет блюдо, которое можно приготовить и продать в таверне.
/// </summary>
public class Dish(string name, decimal price)
{
    public string? Name { get; set; } = name;
    public decimal Price { get; set; } = price;
    public Dictionary<ProductName, int>? Ingredients { get; set; } = new();
}