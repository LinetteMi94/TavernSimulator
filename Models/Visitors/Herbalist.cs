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

    private List<Dish> SecretRecipes = 
        [
            new () {
                Name = "Травяной суп",
                Type = DishTypes.Суп,
                Price = 18,
                RequiredTavernLevel = 1,
                Experience = 9,
                Ingredients = new Dictionary<string, int> { ["Картофель"] = 1, ["Морковь"] = 1, ["Лук"] = 1, ["Травы"] = 1 }
            },
            new () {
                Name = "Яичница с травами",
                Type = DishTypes.Яичное,
                Price = 17,
                RequiredTavernLevel = 1,
                Experience = 9,
                Ingredients = new Dictionary<string, int> { ["Яйцо"] = 2, ["Травы"] = 1 }
            },
            new () {
                Name = "Душистая картошка",
                Type = DishTypes.Овощное,
                Price = 19,
                RequiredTavernLevel = 2,
                Experience = 10,
                Ingredients = new Dictionary<string, int> { ["Картофель"] = 2, ["Лук"] = 1, ["Травы"] = 2 }
            },
            new () {
                Name = "Овощная похлёбка",
                Type = DishTypes.Суп,
                Price = 17,
                RequiredTavernLevel = 3,
                Experience = 10,
                Ingredients = new Dictionary<string, int> { ["Морковь"] = 2, ["Лук"] = 1, ["Травы"] = 2 }
            },
            new () {
                Name = "Курица с душистыми травами",
                Type = DishTypes.Мясное,
                Price = 26,
                RequiredTavernLevel = 4,
                Experience = 18,
                Ingredients = new Dictionary<string, int> { ["Курица"] = 1, ["Лук"] = 1, ["Травы"] = 2 }
            },
            new () {
                Name = "Травяная каша с яблоком",
                Type = DishTypes.Каша,
                Price = 19,
                RequiredTavernLevel = 5,
                Experience = 14,
                Ingredients = new Dictionary<string, int> { ["Пшеничная мука"] = 1, ["Яблоко"] = 1, ["Травы"] = 2 }
            },
            new () {
                Name = "Травяные лепёшки",
                Type = DishTypes.Выпечка,
                Price = 20,
                RequiredTavernLevel = 6,
                Experience = 15,
                Ingredients = new Dictionary<string, int> { ["Пшеничная мука"] = 2, ["Яйцо"] = 1, ["Травы"] = 1 }
            },
            new () {
                Name = "Груша с душистыми травами",
                Type = DishTypes.Сладкое,
                Price = 17,
                RequiredTavernLevel = 7,
                Experience = 9,
                Ingredients = new Dictionary<string, int> {["Груша"] = 2, ["Травы"] = 1 }
            },
            new () {
                Name = "Жареные грибы с чесноком",
                Type = DishTypes.Овощное,
                Price = 25,
                RequiredTavernLevel = 8,
                Experience = 16,
                Ingredients = new Dictionary<string, int> { ["Грибы"] = 2, ["Чеснок"] = 1, ["Лук"] = 1, ["Травы"] = 1 }
            },
            new () {
                Name = "Лимонный настой с мёдом и грушей",
                Type = DishTypes.Напиток,
                Price = 21,
                RequiredTavernLevel = 9,
                Experience = 12,
                Ingredients = new Dictionary<string, int> { ["Лимон"] = 1, ["Мёд"] = 1, ["Груша"] = 1, ["Травы"] = 2 }
            },
        ];
    public override void OnEvent(Tavern tavern)
    {
        var random = Random.Shared.Next(100);
        switch (random)
        {
            case < 60 : 
                SellHerbs(tavern);
                break;
            case < 90 :
                GiveHerbs(tavern);
                break;
            case < 100 :
                OfferRecipe(tavern);
                break;
        }
    }

    /// <summary>
    /// Предлагает таверне купить травы по сниженной цене.
    /// </summary>
    private void SellHerbs(Tavern tavern)
    {
        var count = new Random().Next(2, 5);
        Console.WriteLine($"\nТравница {Name} улыбается:\n«Хорошо готовишь. У меня сегодня остались свежие травы.\nМогу отдать тебе немного по дешёвой цене»");
        Console.WriteLine($"\nТравница предлагает:\n Травы × {count}\nЦена: {count} золота за всё");
        Console.WriteLine("1. Купить\n2. Отказаться\n");
        var herbs = ProductCatalog.Products.First(x => x.Name == "Травы");
        var choice = Input.InputValidator.GetValidInput(2);
        switch (choice)
        {
            case 1:
                tavern.BuyProducts(herbs, count, 1);
                Console.WriteLine("Вот и славно. Хорошие травы, свежие. Пригодятся тебе на кухне.");
                break;
            case 2:
                Console.WriteLine("Ну что ж, дело твоё. Если передумаешь, заходи, пока травы не завяли.");
                break;
        }
    }

    /// <summary>
    /// Передаёт таверне травы в подарок.
    /// </summary>
    private void GiveHerbs(Tavern tavern)
    {
        var count = new Random().Next(2, 5);
        Console.WriteLine($"\nТравница {Name} улыбается:\n«Вкусная у тебя еда." +
                          $"\nВот, держи Травы. Лес нынче щедрый, набрала целую охапку. Не всё же мне одной сушить да перебирать. " +
                          $"Пригодятся тебе для чая и стряпни»");
        var herbs = ProductCatalog.Products.First(x => x.Name == "Травы");
        tavern.BuyProducts(herbs, count, 0);
    }

    /// <summary>
    /// Предлагает таверне изучить случайный секретный рецепт травницы.
    /// </summary>
    private void OfferRecipe(Tavern tavern)
    {
        if (SecretRecipes.Count == 0) return;
        try
        {
            var dish = SecretRecipes.Where(x => x.RequiredTavernLevel <= tavern.Level).OrderBy(_ => Random.Shared.Next())
                .FirstOrDefault();;
            Console.WriteLine($"\nТравница {Name} хитро на тебя смотрит:\n" +
                                      $"«Знаешь, есть у меня один старый рецепт. Бабушка его от своей бабушки получила. Я редко кому его рассказываю… Но тебе, пожалуй, могу доверить.»");
            Console.WriteLine($"\nНазывается блюдо - {dish.Name}. Хочешь научу?");
            Console.WriteLine("1. Да\n2. Нет\n");
            var choice = Input.InputValidator.GetValidInput(2);
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Тогда слушай внимательно. Рецепт простой, но вся хитрость в травах. Без них получится обычное блюдо, а с ними совсем другое.");
                    tavern.LearnSecretDish(dish);
                    SecretRecipes.Remove(dish);
                    break;
                case 2:
                    Console.WriteLine("Как знаешь. Может, ещё передумаешь. Такие рецепты второй раз не всякому предлагаю.");
                    break;
            }
        }
        catch { GiveHerbs(tavern);}
    }
}