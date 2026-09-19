namespace TavernSimulator.Models;

/// <summary>
/// Представляет продукт, используемый для приготовления блюд.
/// </summary>
public class Product
{
    public string Id { get; set; } 
    public string Name { get; set; }
    public string Type { get; set; }
    public int Price { get; set; }
    public int RequiredTavernLevel  { get; set; }
}
