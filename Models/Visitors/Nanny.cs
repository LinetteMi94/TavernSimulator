using System;
using System.Collections.Generic;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Game;
using TavernSimulator.Service;

namespace TavernSimulator.Models.Visitors;

/// <summary>
/// Представляет няньку, которая посещает таверну.
/// Предпочитает простые и сытные блюда, такие как супы и каши.
/// </summary>
public class Nanny : Visitor, IRecipeTeacher
{
    public override string TypeName { get; set; } = "Нянька";
    public override int Money { get; } = new Random().Next(30,35);
    public override List<string> PossibleNames { get; set; } = ["Матрёна","Евдокия","Прасковья","Устинья","Феврония","Агафья",
        "Пелагея","Акулина","Марфа","Степанида","Феодора","Гликерия","Фёкла","Домна","Анфиса","Василиса","Меланья","Евпраксия","Дарья","Параскева"];
    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x =>x.Type is DishTypes.Суп or DishTypes.Напиток or DishTypes.Каша ).ToList();
    public bool HasRecipes { get; set; } = true;
    public override List<Item> AvaliableItemsForLeave { get; set; } = 
    [
        new ("Детская ленточка", "Яркая ленточка для волос.", 2, 8),
        new ("Деревянная игрушка", "Небольшая игрушка, вырезанная из дерева.", 5, 15),
        new ("Вышитый платок", "Аккуратно вышитый платок.", 6, 18),
        new ("Детская пуговица", "Маленькая пуговица с изображением цветка.", 1, 10),
        new ("Старая книжка", "Небольшая детская книжка с потёртой обложкой.", 5, 20)
    ];
    
    public List<Dish> SecretRecipes { get; set; } = DishCatalog.Dishes
        .Where(x => x.Id.StartsWith("nanny_")).ToList();
    
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
        var result = tavern.BuyProducts(apples, count, 0);
        GameOutput.ShowBuyProductResult(result, apples, count);
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
        Game.Game.ShowDishIngridients(dish);
        Console.WriteLine("1. Да\n2. Нет\n");
        var choice = Input.InputValidator.GetValidInput(2);
        switch (choice)
        {
            case 1:
                var result = tavern.CookDish(dish);
                GameOutput.ShowCookResult(result, dish);
                if (result == CookResult.Success)
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
    public void ShareRecipe(Tavern tavern)
    {
        Dish? dish = null;
        if (this is IRecipeTeacher visitor) dish = visitor.TrySetRecipeToTeach(dish, tavern);
        if (dish != null)
        {
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
                    break;
                case 2:
                    Console.WriteLine("Как знаешь. Может, когда-нибудь передумаешь. А я пока поберегу свои маленькие семейные секреты.");
                    break;
            }
        }
    }
}