using TavernSimulator.Data;

namespace TavernSimulator.Models.Visitors;

/// <summary>
/// Представляет торговца, который посещает таверну.
/// Предпочитает редкие блюда.
/// </summary>
public class Merchant : Visitor
{
    public override string TypeName { get; set; } = "Торговец";
    public override int Money { get; } = new Random().Next(50,60);
    public override List<string> PossibleNames { get; set; } = ["Аникей", "Влас", "Гордей", "Ермил", "Наум", "Савватий", "Дорофей",
    "Кузьма", "Велемир", "Радмир", "Боримир", "Евстигней", "Иов", "Онуфрий", "Филат", "Прокопий", "Сысой", "Агапит", "Панкрат", "Ларион"];
    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x => !x.Id.StartsWith("base")).ToList();
    public override List<Item> AvaliableItemsForLeave { get; set; } =
    [
        new ("Фрагмент карты", "Потрёпанный клочок пергамента с выцветшими линиями и пометкой карандашом.", 7, 20),
        new ("Старый компас без стрелки", "Латунный компас с треснутым стеклом, стрелки нет, внутри видна потёртая разметка.", 4, 11),
        new ("Фляга с водой", "Кожаная фляга с грубой прошивкой, пахнет сыростью, а внутри плещется немного воды.", 3, 9),
        new ("Маленький колокольчик", "Крошечный медный колокольчик на изношенном шнурке, язычок слегка погнут.", 5, 15),
        new ("Серебряное кольцо с рунами", "Тонкое серебряное кольцо, руны стёрты от времени, местами видна патина.", 10, 35)
    ];
    public override void OnEvent(Tavern tavern)
    {
        
    }
}