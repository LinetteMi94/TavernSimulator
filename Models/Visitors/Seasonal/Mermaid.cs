using TavernSimulator.Data;
using TavernSimulator.Enums;

namespace TavernSimulator.Models.Visitors.Seasonal;

/// <summary>
/// Представляет русалку, летнего персонажа, которая посещает таверну.
/// Предпочитает блюда без мяса и рыбы.
/// </summary>
public class Mermaid: Visitor
{
    public override string TypeName { get; set; } = "Русалка";
    public override int Money { get; } = new Random().Next(27,38);

    public override List<string> PossibleNames { get; set; } =
    [
        "Рыбелла", "Водяна", "Ручейка", "Брызгальда", "Водослава", "Росава", "Озерина",
        "Волнеслава", "Капельда", "Мокрина", "Речана", "Жемчужка", "Бульбина", "Плеска",
        "Росинка", "Водея", "Лазурка", "Рыбана"
    ];
    public override List<Dish> PreferredDishes => 
        DishCatalog.Dishes.Where(x => x.Name.StartsWith("summer_") 
                                      || x.Type == DishTypes.Рыбное
                                      || x.Ingredients.Keys.All(ingredientName =>
                                          ProductCatalog.GetProduct(ingredientName) is { Type: ProductTypes.Ягода }
                                      )).ToList();
    public override List<Item> AvaliableItemsForLeave { get; set; } =
    [
        new ("Речная ракушка", "Небольшая гладкая ракушка, отполированная течением и покрытая перламутром.", 3, 9),
        new ("Речной жемчуг", "Небольшая жемчужина с мягким серебристым блеском, найденная на дне реки.", 5, 15),
        new ("Водоросль", "Свежая зелёная водоросль с лёгким морским запахом. Подойдёт для кухни или алхимии.", 2, 6),
        new ("Гладкий камень", "Маленький разноцветный камень, удивительно гладкий и холодный после долгого пребывания в воде.", 2, 5),
        new ("Капля русалки", "Крошечная прозрачная капля воды, которая не испаряется даже под жарким солнцем.", 6, 18)
    ];

    public override VisitorTypes Type { get; set; } = VisitorTypes.Summer;

    public override void OnEvent(Tavern tavern)
    {
       
    }
}