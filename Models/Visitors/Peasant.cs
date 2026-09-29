using System.Collections.Generic;
using TavernSimulator.Data;
using TavernSimulator.Game;
using TavernSimulator.Service;

namespace TavernSimulator.Models.Visitors;

/// <summary>
/// Представляет крестьянина, который посещает таверну.
/// Предпочитает простые и недорогие блюда.
/// </summary>
public class Peasant : Visitor
{
    public override string TypeName { get; set; } =  "Крестьянин"; 
    public override int Money { get; } = new Random().Next(20,30);
    
    private Product secretProduct { get; set; } = new () { Name = "Кабачок", Price = 0, RequiredTavernLevel = 1 };
    public override List<string> PossibleNames { get; set; } = ["Иван", "Степан", "Тихон", "Фома", "Яков", "Кузьма", "Прохор",
        "Матвей", "Савелий", "Михаил", "Егор", "Фёдор", "Лука", "Пётр", "Афанасий", "Данила", "Григорий", "Макар", "Трофим", "Никита"];

    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x => x.Price <= 20).ToList();
    public override void OnEvent(Tavern tavern)
    {
        var count = new Random().Next(5, 20);
        Console.WriteLine($"\nКрестьянин {Name} достаёт из под стула мешок:\n«Кабачков столько выросло, что я уже не знаю, куда их девать. " +
                          $"В магазине таких всё равно нет. Забирай несколько, а то жена скоро из них стены начнёт строить»");
        var result = tavern.BuyProducts(secretProduct, count, 0);
        GameOutput.ShowBuyProductResult(result, secretProduct, count);
    }
}