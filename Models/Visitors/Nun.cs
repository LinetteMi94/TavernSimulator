using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Service;

namespace TavernSimulator.Models.Visitors;

/// <summary>
/// Представляет монахиню, которая посещает таверну и делится секретными рецептами монастырской кухни.
/// </summary>
public class Nun : Visitor
{
    public override string TypeName { get; set; } =  "Монахиня"; 
    public override int Money { get; } = new Random().Next(30,45);
    public override List<string> PossibleNames { get; set; } = ["Агнесса","Беатриса","Клара",
        "Марта","Матильда","Розалия","Эльза","Хильда","Эльвира","Адела","Гертруда","Бригитта"];
     public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x =>x.Type is DishTypes.Сладкое or DishTypes.Напиток or DishTypes.Выпечка ).ToList();
     private bool _hasRecipes = true;
    
     private List<Dish> SecretRecipes = 
        [
            new () {
                Name = "Рагу с кабачком",
                Type = DishTypes.Овощное,
                Price = 18,
                RequiredTavernLevel = 1,
                Experience = 8,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 2, ["Картофель"] = 1, ["Морковь"] = 1, ["Лук"] = 1 }
            },
            new () {
                Name = "Кабачковые оладьи",
                Type = DishTypes.Выпечка,
                Price = 16,
                RequiredTavernLevel = 2,
                Experience = 7,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 1, ["Пшеничная мука"] = 1, ["Яйцо"] = 1 }
            },
            new () {
                Name = "Кабачки с чесноком",
                Type = DishTypes.Овощное,
                Price = 18,
                RequiredTavernLevel = 3,
                Experience = 10,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 2, ["Чеснок"] = 1, ["Травы"] = 1 }
            },

            new () {
                Name = "Кабачки с грибами",
                Type = DishTypes.Овощное,
                Price = 20,
                RequiredTavernLevel = 4,
                Experience = 11,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 2, ["Грибы"] = 2, ["Лук"] = 1 }
            },
            new () {
                Name = "Кабачковый пирог",
                Type = DishTypes.Выпечка,
                Price = 24,
                RequiredTavernLevel = 5,
                Experience = 13,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 2, ["Пшеничная мука"] = 2, ["Яйцо"] = 1, ["Мёд"] = 1 }
            },
            new () {
                Name = "Кабачковая запеканка с сыром",
                Type = DishTypes.Выпечка,
                Price = 23,
                RequiredTavernLevel = 13,
                Experience = 12,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 2, ["Сыр"] = 1, ["Яйцо"] = 1, ["Молоко"] = 1 }
            },
            new () {
                Name = "Тушёные кабачки с капустой",
                Type = DishTypes.Овощное,
                Price = 26,
                RequiredTavernLevel = 14,
                Experience = 14,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 2, ["Капуста"] = 1, ["Лук"] = 1, ["Масло"] = 1 }
            },
            new () {
                Name = "Жареные кабачки",
                Type = DishTypes.Овощное,
                Price = 21,
                RequiredTavernLevel = 15,
                Experience = 10,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 2, ["Масло"] = 1 }
            },
            new () {
                Name = "Фаршированные кабачки с рисом и горохом",
                Type = DishTypes.Овощное,
                Price = 22,
                RequiredTavernLevel = 20,
                Experience = 10,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 2, ["Рис"] = 1, ["Горох"] = 1, ["Лук"] = 1, ["Масло"] = 1 }
            },
            new () {
                Name = "Маринованный кабачок",
                Type = DishTypes.Овощное,
                Price = 16,
                RequiredTavernLevel = 21,
                Experience = 7,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 2, ["Острый перец"] = 1, ["Травы"] = 1 }
            },
            new () {
                Name = "Острые кабачки",
                Type = DishTypes.Овощное,
                Price = 21,
                RequiredTavernLevel = 22,
                Experience = 12,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 2, ["Острый перец"] = 1, ["Чеснок"] = 1, ["Масло"] = 1 }
            },

            new () {
                Name = "Кабачки, фаршированные форелью",
                Type = DishTypes.Рыбное,
                Price = 30,
                RequiredTavernLevel = 24,
                Experience = 20,
                Ingredients = new Dictionary<string, int> { ["Кабачок"] = 2, ["Форель"] = 1, ["Рис"] = 1, ["Лук"] = 1, ["Травы"] = 1 }
            },
            
        ];
     
        public override void OnEvent(Tavern tavern)
        {
            if (!_hasRecipes) return; 
            Dish? dish = null;
            while(_hasRecipes)
            {
                dish = SecretRecipes.Where(x => x.RequiredTavernLevel <= tavern.Level).OrderBy(_ => Random.Shared.Next())
                    .FirstOrDefault();
                if (tavern.AvailableDishes.Contains(dish))
                {
                    SecretRecipes.Remove(dish);
                    if (SecretRecipes.Count == 0) _hasRecipes = false;
                }
                else break;
            }
            Console.WriteLine($"\nМонахиня {Name} хитро на тебя смотрит:\n" +
                                  $"«В монастыре мы часто готовим кабачки. Есть один рецепт, который особенно хорошо получается осенью. Хочешь, я поделюсь им?»");
            Console.WriteLine($"\nНазывается блюдо - {dish.Name}. Хочешь научу?");
            Console.WriteLine("1. Да\n2. Нет\n");
            var choice = Input.InputValidator.GetValidInput(2);
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Этот рецепт у нас берегут особенно тщательно. Я нечасто делюсь им с посторонними... Но тебе, пожалуй, доверю.");
                    tavern.LearnSecretDish(dish);
                    SecretRecipes.Remove(dish);
                    break;
                case 2:
                    Console.WriteLine("Как пожелаешь. Рецепт никуда не убежит.");
                    break;
            }
        }
}