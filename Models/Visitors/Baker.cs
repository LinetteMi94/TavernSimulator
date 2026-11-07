using System;
using System.Collections.Generic;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Game;
using TavernSimulator.Service;

namespace TavernSimulator.Models.Visitors;

/// <summary>
/// Представляет пекаря, который посещает таверну.
/// Предпочитает выпечку и сладкие блюда.
/// </summary>
public class Baker : Visitor, IRecipeTeacher
{
    public override string TypeName { get; set; } = "Пекарь";
    public override int Money { get; } = new Random().Next(30,45);
    public override List<string> PossibleNames { get; set; } =
    [
        "Кузьма", "Емельян", "Тихон", "Ерофей", "Фома",
        "Прохор", "Лаврентий", "Гордей", "Савва", "Мирон",
        "Агафон", "Елисей", "Наум", "Пахом", "Фрол",
        "Никифор", "Архип", "Захар", "Игнат", "Тимофей"
    ];
    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x =>x.Type is DishTypes.Выпечка or DishTypes.Сладкое).ToList();

    public List<string> Questions { get; set; } =
    [
        "Какой продукт я однажды забыл добавить в тесто, а пирог всё равно получился на удивление хорошим?",
        "Какой продукт я первым делом проверяю, когда захожу утром в пекарню?",
        "Какой продукт я спрятал от своего ученика, чтобы он не съел его раньше времени?",
        "Что, по-твоему, я взял с собой в первую поездку на ярмарку?",
        "Какой продукт моя бабушка запрещала мне трогать без спроса?",
        "Что я однажды случайно уронил в корзину с выпечкой?",
        "Какой продукт я подарил соседу в благодарность за помощь?",
        "Что я купил на первые заработанные деньги?",
        "Какой продукт я однажды перепутал с совершенно другим на рынке?",
        "Что я всегда беру с собой, отправляясь в дальнюю дорогу?"
    ];
    public override List<Item> AvaliableItemsForLeave { get; set; } =
    [
        new ("Деревянная скалка", "Простая деревянная скалка, отполированная долгими годами работы.", 5, 15),
        new ("Медная формочка", "Небольшая медная формочка для выпечки фигурного печенья.", 10, 28),
        new ("Льняной фартук", "Плотный льняной фартук, испачканный мукой и потемневший от времени.", 6, 16),
        new ("Складной нож", "Небольшой рабочий нож с деревянной рукоятью.", 8, 22),
        new ("Медный свисток", "Маленький свисток, которым пекарь подзывает помощников из соседней комнаты.", 7, 20)
    ];
    
    public bool HasRecipes { get; set; } = true;
    public List<Dish> SecretRecipes { get; set; } = DishCatalog.Dishes
        .Where(x => x.Id.StartsWith("baker_")).ToList();
    
    public override void OnEvent(Tavern tavern)
    {
        var random = Random.Shared.Next(100);
        switch (random)
        {
            case < 60 : 
                GuessIngredient(tavern);
                break;
            case < 90 :
                GiveBread(tavern);
                break;
            case < 101 :
                ShareRecipe(tavern);
                break;
        }
    }

    /// <summary>
    /// Предлагает игроку угадать случайно выбранный продукт
    /// и определяет результат спора с пекарем.
    /// </summary>
    private void GuessIngredient(Tavern tavern)
    {
        Console.WriteLine($"\nПекарь {Name} довольно потирает руки:\n" +
                          "«Вот скажи мне, хозяин, доводилось ли тебе пробовать пироги лучше моих? " +
                          "Да нет, конечно! Во всей округе никто не печёт лучше меня!»");

        Console.WriteLine("\nПекарь наклоняется поближе и заговорщически улыбается:\n" +
                          "«А хочешь, проверим, насколько хорошо ты угадываешь? " +
                          "Я задам тебе один вопрос из своей жизни, а ты попробуй угадать ответ! " +
                          "Ну что, рискнёшь?»");
        Console.WriteLine("1. Да\n2. Нет\n");
        var choice = Input.InputValidator.GetValidInput(2);
        switch (choice)
        {
            case 1:
                var question = Questions[new Random().Next(Questions.Count)];
                Console.WriteLine(question);
                List<Product> products = [];
                for (int i = 0; i < 3; i++)
                {
                    var product = ProductCatalog.Products[new Random().Next(ProductCatalog.Products.Count)];
                    products.Add(product);
                    Console.WriteLine($"{i+1}. {product.Name}");
                }
                var answer = Input.InputValidator.GetValidInput(3)-1;
                if (answer == new Random().Next(1, 4))
                {
                    Console.WriteLine($"\nПекарь {Name} удивлённо поднимает брови:\n" +
                                      "«Вот это глаз! А может, и вкус у тебя не хуже моего? " +
                                      "Что ж, уговор есть уговор. Держи награду! Только не вздумай открывать свою пекарню, а то останусь без покупателей!»");
                    tavern.Experience += 100;
                    tavern.ChangeReputation(2);
                    return;
                }
                Console.WriteLine($"\nПекарь {Name} громко смеётся:\n" +
                                      "«Ха-ха! Не угадал! Ничего, дело наживное. " +
                                      "Выпечка любит терпение, а секреты мастерства сами в голову не приходят. " +
                                      "Может, в следующий раз получится!»");
                break;
            case 2:
                Console.WriteLine($"\nПекарь {Name} пожимает плечами:\n" +
                                  "«Как знаешь! Видать, не желаешь рисковать своей репутацией знатока выпечки. " +
                                  "Ну да ладно, не всякому охота мериться мастерством с настоящим пекарем!»");
                tavern.ChangeReputation(2,false);
                break;
        }
    }

    /// <summary>
    /// Передаёт таверне хлеб в подарок.
    /// </summary>
    private void GiveBread(Tavern tavern)
    {
        var count = new Random().Next(4, 7);
        Console.WriteLine($"\nПекарь {Name} улыбается:\n«Хлеба нынче напёк видимо-невидимо!" +
                          $"\nПечь всё не остывала, а теперь и девать столько некуда. Забирай немного себе!" +
                          $" Свежий хлеб в таверне всегда пригодится. Авось и гостям твоим по кусочку достанется»");
        var bread = ProductCatalog.Products.First(x => x.Name == "Хлеб");
        var result = tavern.BuyProducts(bread, count, 0);
        GameOutput.ShowBuyProductResult(result, bread, count);
    }

    /// <summary>
    /// Предлагает таверне изучить случайный секретный рецепт пекаря.
    /// </summary>
    public void ShareRecipe(Tavern tavern)
    {
        Dish? dish = null;
        if (this is IRecipeTeacher visitor) dish = visitor.TrySetRecipeToTeach(dish, tavern);
        if (dish != null)
        {
            Console.WriteLine($"\nПекарь {Name} задумчиво поглаживает бороду:\n" +
                              $"«Знаешь, есть у меня один старинный рецепт выпечки. Ещё мой дед у своего отца перенял, а тот от своего деда. " +
                              $"Не каждому я его доверяю… Но ты, вижу, с тестом обращаться умеешь. Пожалуй, поделюсь с тобой.»");
            Console.WriteLine($"\nНазывается блюдо - {dish.Name}. Хочешь научу?");
            Console.WriteLine("1. Да\n2. Нет\n");
            var choice = Input.InputValidator.GetValidInput(2);
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Тогда слушай внимательно. В выпечке главное не спешить: тесто любит терпение, а печь не прощает суеты. И запомни, хороший пекарь всегда знает, когда пора доставать пирог.");
                    tavern.LearnSecretDish(dish);
                    SecretRecipes.Remove(dish);
                    break;
                case 2:
                    Console.WriteLine("Как знаешь. Может, ещё передумаешь. Такой старинный рецепт я не каждому доверяю. А выпечка, приготовленная по нему, ещё не раз тебя выручит.");
                    break;
            }
        }
    }
}