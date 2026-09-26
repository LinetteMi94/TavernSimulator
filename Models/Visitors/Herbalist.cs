using TavernSimulator.Data;
using TavernSimulator.Enums;

namespace TavernSimulator.Models.Visitors;

/// <summary>
/// Представляет травницу, которая посещает таверну.
/// Предпочитает блюда с овощами, травами и лёгкие напитки.
/// </summary>
public class Herbalist : Visitor
{
    public override string TypeName { get; set; } = "Травница";
    public override int Money { get; } = new Random().Next(30,40);
    public override List<string> PossibleNames { get; set; } = ["Меланья", "Аглая","Лукерья","Акулина","Феврония","Пелагея","Дарья",
        "Матрёна","Евдокия","Прасковья","Домника","Феодосия","Марфа","Устинья","Евпраксия","Василиса","Агафья","Ксения","Степанида","Соломонида"];
    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x =>x.Type == DishTypes.Овощное || x.Ingredients.ContainsKey("Травы") || x.Type == DishTypes.Суп || x.Name.Contains("Чай") ).ToList();
}