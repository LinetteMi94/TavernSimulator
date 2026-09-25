using System.Collections.Generic;
using TavernSimulator.Data;

namespace TavernSimulator.Models.Visitors;

public class Peasant : Visitor
{
   public override string TypeName { get; set; } =  "Крестьянин"; 

    public override List<string> PossibleNames { get; set; } = ["Иван", "Степан", "Тихон", "Фома", "Яков", "Кузьма", "Прохор",
        "Матвей", "Савелий", "Михаил", "Егор", "Фёдор", "Лука", "Пётр", "Афанасий", "Данила", "Григорий", "Макар", "Трофим", "Никита"];

    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x => x.Price <= 20).ToList();
    public override int Money { get; } = new Random().Next(20,30);
}