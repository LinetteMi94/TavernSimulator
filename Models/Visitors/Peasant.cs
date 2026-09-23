using System.Collections.Generic;

namespace TavernSimulator.Models.Visitors;

public class Peasant : Visitor
{
    public override string TypeName { get; set; } =  "Крестьянин"; 
    
    public override string Name { get; set; } 

    public override int Money { get; set; }

    public override List<string> PossibleNames { get; set; } = ["Иван", "Степан", "Тихон", "Фома", "Яков", "Кузьма", "Прохор",
        "Матвей", "Савелий", "Михаил", "Егор", "Фёдор", "Лука", "Пётр", "Афанасий", "Данила", "Григорий", "Макар", "Трофим", "Никита"];

    public override List<Dish> PreferredDishes { get; set; } 
}