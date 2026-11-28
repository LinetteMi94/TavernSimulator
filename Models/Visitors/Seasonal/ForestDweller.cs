using TavernSimulator.Data;
using TavernSimulator.Enums;

namespace TavernSimulator.Models.Visitors.Seasonal;

/// <summary>
/// Представляет лесовика, осеннего персонажа, который посещает таверну.
/// Предпочитает блюда с грибами и ягодами.
/// </summary>
public class ForestDweller : Visitor
{
    public override string TypeName { get; set; } = "Лесничий";
    public override int Money { get; } = new Random().Next(27,36);

    public override List<string> PossibleNames { get; set; } =
    [
        "Боровик", "Подберёзовик","Лисичкин","Опёнок","Моховик","Подосиновик","Маслёнок",
        "Груздь","Кедровик","Рыжик","Хвоевик","Еловей", "Кедрич", "Крапивник","Морошковик",
        "Мятовик"
    ];
    public override List<Dish> PreferredDishes => 
        DishCatalog.Dishes.Where(x => x.Name.StartsWith("autumn_") 
                                      || x.Ingredients.Keys.All(ingredientName =>
                                          ProductCatalog.GetProduct(ingredientName) is { Type: ProductTypes.Ягода }
                                      || ProductCatalog.GetProduct(ingredientName) is {Type: ProductTypes.Гриб }
                                      )).ToList();
    public override List<Item> AvaliableItemsForLeave { get; set; } =
    [
        new ("Еловая шишка", "Крупная смолистая шишка, принесённая из старой хвойной чащи.", 2, 6),
        new ("Пучок мха", "Мягкий лесной мох, собранный с поваленного дерева.", 2, 7),
        new ("Янтарный кусочек", "Маленький кусочек тёплого янтаря, найденный среди лесного мусора.", 5, 15),
        new ("Кора берёзы", "Светлый кусочек берёзовой коры, аккуратно снятый со старого дерева.", 2, 6),
        new ("Лесной камень", "Небольшой камень необычной формы, покрытый тонким узором мха.", 3, 9)
    ];

    public override VisitorTypes Type { get; set; } = VisitorTypes.Autumn;

    public override void OnEvent(Tavern tavern)
    {
       
    }
}