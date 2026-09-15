using TavernSimulator.Enums;

namespace TavernSimulator.Models;

/// <summary>
/// Представляет продукт, используемый для приготовления блюд.
/// </summary>
public class Product(ProductName name, ProductType type, int price)
{
    public ProductName Name { get; set; } = name;
    public ProductType Type { get; set; } = type;
    public decimal Price { get; set; } = price;
}