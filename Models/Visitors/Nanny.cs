using TavernSimulator.Data;
using TavernSimulator.Enums;

namespace TavernSimulator.Models.Visitors;

/// <summary>
/// Представляет няньку, которая посещает таверну.
/// Предпочитает простые и сытные блюда, такие как супы и каши.
/// </summary>
public class Nanny : Visitor
{
    public override string TypeName { get; set; } = "Нянька";
    public override int Money { get; } = new Random().Next(30,35);
    public override List<string> PossibleNames { get; set; } = ["Матрёна","Евдокия","Прасковья","Устинья","Феврония","Агафья",
        "Пелагея","Акулина","Марфа","Степанида","Феодора","Гликерия","Фёкла","Домна","Анфиса","Василиса","Меланья","Евпраксия","Дарья","Параскева"];
    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x =>x.Type is DishTypes.Суп or DishTypes.Напиток or DishTypes.Каша ).ToList();
}