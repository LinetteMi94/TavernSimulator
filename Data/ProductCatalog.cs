using System.Text.Json;
using TavernSimulator.Models;

namespace TavernSimulator.Data;

/// <summary>
/// Содержит полный каталог продуктов, доступных в игре.
/// </summary>
public class ProductCatalog
{
    public List<Product>? Products { get; set; }

    public ProductCatalog()
    {
        string json = File.ReadAllText("Data/ProductCatalogJSON.json");
        Products = JsonSerializer.Deserialize<List<Product>>(json);
    }
}