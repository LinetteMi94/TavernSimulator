using TavernSimulator.Data;
using TavernSimulator.Enums;

namespace TavernSimulator.Models.Visitors.Seasonal;

/// <summary>
/// Представляет дриаду, весеннего персонажа, которая посещает таверну.
/// Предпочитает блюда без мяса и рыбы.
/// </summary>
public class Dryad : Visitor
{
    public override string TypeName { get; set; } = "Дриада";
    public override int Money { get; } = new Random().Next(30,51);

    public override List<string> PossibleNames { get; set; } =
    ["Лада", "Ясна", "Рута", "Олеся", "Зоряна", "Ива", "Калина", "Рада", "Веста", "Мила", 
        "Лилия", "Таисия", "Злата", "Арина", "Ася"];
    public override List<Dish> PreferredDishes => 
        DishCatalog.Dishes.Where(x => x.Ingredients.Keys.All(ingredientName =>
                                         ProductCatalog.GetProduct(ingredientName) is not { Type: ProductTypes.Мясо }
                                         && ProductCatalog.GetProduct(ingredientName) is not { Type: ProductTypes.Рыба }) ).ToList();
    public override List<Item> AvaliableItemsForLeave { get; set; } =
    [
        new ("Пучок сухих трав", "Небольшой пучок лесных трав, перевязанный тонкой веточкой.", 3, 8),
        new ("Семена полевых цветов", "Маленький мешочек с разноцветными семенами, собранными на лесной поляне.", 5, 12),
        new ("Деревянный амулет", "Небольшой амулет из гладкого куска дерева с вырезанным узором листа.", 7, 18),
        new ("Бутылочка лесной росы", "Прозрачная бутылочка с прохладной росой, собранной ранним весенним утром.", 6, 15),
        new ("Венок из первоцветов", "Небольшой венок из свежих весенних цветов, пахнущий лесом и влажной землёй.", 4, 10)
    ];

    public override VisitorTypes Type { get; set; } = VisitorTypes.Spring;

    public override void OnEvent(Tavern tavern)
    {
       
    }
}