using TavernSimulator.Enums;

namespace TavernSimulator.Models;

/// <summary>
/// Представляет продукт, используемый для приготовления блюд.
/// </summary>
public class Product(ProductName name, int price)
{
    public ProductName Name { get; set; } = name;
    public decimal Price { get; set; } = price;
}