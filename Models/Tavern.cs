using TavernSimulator.Data;
using TavernSimulator.Enums;
using System.Linq;

namespace TavernSimulator.Models;

/// <summary>
/// Представляет таверну, хранит её состояние, запасы продуктов,
/// доступные блюда, золото, репутацию и информацию о посетителях.
/// </summary>
public class Tavern
{
    public const string Name = "Гусь и пирог";
    public List<Product> Products { get; set; }
    public List<Dish>? AvailableDishes { get; set; } 
    public int Day { get; set; } = 1;
    public int Customers { get; set; }
    public int Gold { get; set; } = 100;
    public int Reputation { get; set; } = 1;
}