using TavernSimulator.Data;
using TavernSimulator.Enums;

namespace TavernSimulator.Models.Visitors.Seasonal;

/// <summary>
/// Представляет снеговика, зимнего персонажа, который посещает таверну.
/// Предпочитает блюда без мяса и рыбы.
/// </summary>
public class Snowman : Visitor
{
    public override string TypeName { get; set; } = "Снеговик";
    public override int Money { get; } = new Random().Next(15,25);

    public override List<string> PossibleNames { get; set; } =
    ["Сугробыч", "Морозыч", "Хрустиан", "Ледовик", "Морковальд", "Морковий", "Вьюн", "Северин", "Фростик",
        "Наст", "Снеждан", "Сугроб", "Снегослав", "Морозей", "Инебор"];
    public override List<Dish> PreferredDishes => 
        DishCatalog.Dishes.Where(x => x.Name.StartsWith("winter_") || x.Type == DishTypes.Выпечка ).ToList();
    public override List<Item> AvaliableItemsForLeave { get; set; } =
    [
        new ("Снежный шар", "Небольшой шар из чистого снега, который не тает даже в тепле.", 4, 10),
        new ("Ледяная сосулька", "Прозрачная сосулька с необычным голубоватым оттенком, холодная даже на ощупь.", 3, 9),
        new ("Снежинка из льда", "Тонкая ледяная снежинка с удивительно правильной формой, будто вырезанная мастером.", 5, 14),
        new ("Угольный глаз", "Небольшой чёрный угольный камешек, оставшийся от старого снеговика.", 2, 7),
        new ("Морковный нос", "Яркая свежая морковь, которую снеговик почему-то решил оставить в подарок.", 3, 8)
    ];

    public override VisitorTypes Type { get; set; } = VisitorTypes.Winter;

    public override void OnEvent(Tavern tavern)
    {
       
    }
}