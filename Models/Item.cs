namespace TavernSimulator.Models;

/// <summary>
/// Представляет предмет, найденный в таверне и оставленный на хранение.
/// </summary>
public class Item(string name, string description, int value, int ownerPrice)
{
    public string? Name { get; set; } = name;
    public string Description { get; set; } = description;
    public int Value { get; set; } = value;
    public int OwnerPrice { get; set; } = ownerPrice;
    public string? OwnerName { get; set; }
    public string? OwnerType { get; set; }
}