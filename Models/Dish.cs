namespace TavernSimulator.Models;

/// <summary>
/// Представляет блюдо, которое можно приготовить и продать в таверне.
/// </summary>
public class Dish(string name, decimal price)
{
    public string? Name { get; set; } = name;
    public decimal Price { get; set; } = price;
    public Dictionary<Product, int>? Products { get; set; } = new();
}