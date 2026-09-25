using TavernSimulator.Data;
using TavernSimulator.Enums;

namespace TavernSimulator.Models.Visitors;

public class Woodcutter: Visitor
{
    public override string TypeName { get; set; } = "Лесоруб";
    public override int Money { get; } = new Random().Next(30,40);
    public override List<string> PossibleNames { get; set; } = [ "Прохор", "Тихон", "Фёдор", "Степан", "Трофим", "Кузьма", "Макар",
        "Ефим", "Гаврила", "Лука", "Яков", "Пахом", "Савва", "Елисей", "Мирон", "Игнат", "Наум", "Фома", "Данила", "Егор"];
    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x =>x.Type == DishTypes.Мясное || x.Type == DishTypes.Напиток).ToList();
}