using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Service;

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
    
    private List<Dish> SecretRecipes = 
        [
            new () {
                Name = "Яблочные лепёшки",
                Type = DishTypes.Выпечка,
                Price = 25,
                RequiredTavernLevel = 2,
                Experience = 14,
                Ingredients = new Dictionary<string, int> { ["Пшеничная мука"] = 2, ["Яблоко"] = 1, ["Яйцо"] = 1, ["Мёд"] = 1  }
            },
            new () {
                Name = "Медовые булочки",
                Type = DishTypes.Выпечка,
                Price = 28,
                RequiredTavernLevel = 3,
                Experience = 18,
                Ingredients = new Dictionary<string, int> { ["Пшеничная мука"] = 2, ["Молоко"] = 1, ["Яйцо"] = 1, ["Мёд"] = 2  }
            },
            new () {
                Name = "Тыквенная кашка",
                Type = DishTypes.Каша,
                Price = 17,
                RequiredTavernLevel = 4,
                Experience = 7,
                Ingredients = new Dictionary<string, int> { ["Тыква"] = 2, ["Молоко"] = 1 }
            },
            new () {
                Name = "Тыквенные пирожки",
                Type = DishTypes.Выпечка,
                Price = 29,
                RequiredTavernLevel = 5,
                Experience = 16,
                Ingredients = new Dictionary<string, int> { ["Пшеничная мука"] = 2, ["Тыква"] = 2, ["Яйцо"] = 1, ["Мёд"] = 1 }
            },
            new () {
                Name = "Тыквенное печенье",
                Type = DishTypes.Выпечка,
                Price = 25,
                RequiredTavernLevel = 6,
                Experience = 13,
                Ingredients = new Dictionary<string, int> { ["Пшеничная мука"] = 2, ["Тыква"] = 1, ["Яйцо"] = 1, ["Мёд"] = 1 }
            },
            new () {
                Name = "Сладкая тыквенная запеканка",
                Type = DishTypes.Сладкое,
                Price = 26,
                RequiredTavernLevel = 7,
                Experience = 14,
                Ingredients = new Dictionary<string, int> { ["Тыква"] = 2, ["Молоко"] = 2, ["Яйцо"] = 1, ["Мёд"] = 1 }
            },
            new () {
                Name = "Грушевые пирожки",
                Type = DishTypes.Выпечка,
                Price = 20,
                RequiredTavernLevel = 8,
                Experience = 13,
                Ingredients = new Dictionary<string, int> { ["Пшеничная мука"] = 2, ["Груша"] = 1, ["Яйцо"] = 1, ["Мёд"] = 1 }
            },
            new () {
                Name = "Нежная яблочная каша",
                Type = DishTypes.Каша,
                Price = 21,
                RequiredTavernLevel = 9,
                Experience = 11,
                Ingredients = new Dictionary<string, int> {["Яблоко"] = 1, ["Молоко"] = 1, ["Мёд"] = 1, ["Гречка"] = 1 }
            },
            new () {
                Name = "Ржаные тыквенные кексики",
                Type = DishTypes.Выпечка,
                Price = 35,
                RequiredTavernLevel = 10,
                Experience = 24,
                Ingredients = new Dictionary<string, int> { ["Ржаная мука"] = 2, ["Тыква"] = 2, ["Яйцо"] = 1, ["Молоко"] = 1, ["Мёд"] = 1, }
            }
        ];
    
    public override void OnEvent(Tavern tavern)
    {   
        var random = Random.Shared.Next(100);
        switch (random)
        {
            case < 65 : 
                BringProducts(tavern);
                break;
            case < 90 :
                AskForFoodForSickChild(tavern);
                break;
            case < 101 :
                ShareRecipe(tavern);
                break;
        }
    }
    
    /// <summary>
    /// Даёт в дар таверне яблоки, собранные детьми.
    /// </summary>
    private void BringProducts(Tavern tavern)
    {
        var count = new Random().Next(3, 6);
        Console.WriteLine($"\nНянька {Name} улыбается:\n«Хорошая у тебя таверна, уютная" +
                          $"\nДержи-ка, принесла тебе немного яблок из сада. Дети помогали собирать, так что считай, это их маленький вклад в твою таверну. " +
                          $"Яблоки сами выбирали, правда половину по дороге чуть не съели»");
        var apples = ProductCatalog.Products.First(x => x.Name == "Яблоко");
        tavern.BuyProducts(apples, count, 0);
    }
    
    /// <summary>
    /// Просит приготовить простое блюдо для больного ребёнка.
    /// </summary>
    private void AskForFoodForSickChild(Tavern tavern)
    {
        Console.WriteLine($"\nНянька {Name} грустно смотрит:\n«Слушай, у меня малыш приболел. Ничего тяжёлого ему сейчас нельзя, " +
                          $"а есть всё равно нужно. Не мог бы ты приготовить что-нибудь простое и тёплое? Может, кашку или лёгкий супчик?»");
        var dish = DishCatalog.Dishes.Where(x => x.Type is DishTypes.Каша or DishTypes.Суп && x.RequiredTavernLevel <= tavern.Level)
            .OrderBy(_ => Random.Shared.Next())
            .FirstOrDefault();
        Console.WriteLine($"\nНянька просит приготовить\n {dish.Name}");
        tavern.ShowDishIngridients(dish);
        Console.WriteLine("1. Да\n2. Нет\n");
        var choice = Input.InputValidator.GetValidInput(2);
        switch (choice)
        {
            case 1:
                if (tavern.CookDish(dish))
                {
                    tavern.Gold += dish.Price;
                    Console.WriteLine("Спасибо тебе большое. Думаю, ему сейчас как раз такая еда и нужна. Только положи порцию небольшую, он у меня малоежка.");
                }

                break;
            case 2:
                Console.WriteLine("Понимаю. Тогда ничего страшного. Придётся мне самой что-нибудь придумать для него. Спасибо, что выслушал.");
                break;
        }
    }
    
    /// <summary>
    /// Делится простым рецептом сладкого или домашней выпечки для детей.
    /// </summary>
    private void ShareRecipe(Tavern tavern)
    {
        if (SecretRecipes.Count == 0) return;
        try
        {
            var dish = SecretRecipes.Where(x => x.RequiredTavernLevel <= tavern.Level).OrderBy(_ => Random.Shared.Next())
                .FirstOrDefault();;
            Console.WriteLine($"\nНянька {Name} хитро на тебя смотрит:\n" +
                              $"«Знаешь, я тут подумала… Есть у меня несколько рецептов, которые дети особенно любят. Ничего мудрёного, всё простое, домашнее. Но если правильно приготовить, тарелка потом пустая за минуту.»");
            Console.WriteLine($"\nМогу одним поделиться. Называется блюдо - {dish.Name}. Хочешь научу?");
            Console.WriteLine("1. Да\n2. Нет\n");
            var choice = Input.InputValidator.GetValidInput(2);
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Вот и хорошо! Тогда слушай внимательно. Рецепт простой, но я тебе его не просто так рассказываю. Дети его обожают, особенно когда приготовишь с мёдом. Запоминай, пригодится.");
                    tavern.LearnSecretDish(dish);
                    SecretRecipes.Remove(dish);
                    break;
                case 2:
                    Console.WriteLine("Как знаешь. Может, когда-нибудь передумаешь. А я пока поберегу свои маленькие семейные секреты.");
                    break;
            }
        }
        catch { BringProducts(tavern);}
    }
}