using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Service;

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
    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x =>x.Type is DishTypes.Овощное or DishTypes.Суп || x.Ingredients.ContainsKey("Травы") || x.Name.Contains("Чай") ).ToList();
    public override void OnEvent(Tavern tavern)
    {
        var count = new Random().Next(2, 5);
        Console.WriteLine($"\nТравница {Name} улыбается:\n«Хорошо готовишь. У меня сегодня остались свежие травы.\nМогу отдать тебе немного по дешёвой цене»");
        Console.WriteLine($"\nТравница предлагает:\n Травы × {count}\nЦена: {count} золота за всё");
        Console.WriteLine("1. Купить\n2. Отказаться\n");
        var herbs = ProductCatalog.Products.First(x => x.Name == "Травы");
        var choice = Input.InputValidator.GetValidInput(2);
        if (choice == 1)
        {
            tavern.BuyProducts(herbs, count, 1);
            Console.WriteLine("Вот и славно. Хорошие травы, свежие. Пригодятся тебе на кухне.");
        }
        if (choice == 2)
        {
            Console.WriteLine("Ну что ж, дело твоё. Если передумаешь, заходи, пока травы не завяли.");
        }
    }
}