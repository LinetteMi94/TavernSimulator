using System;
using System.Collections.Generic;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Game;
using TavernSimulator.Service;

namespace TavernSimulator.Models.Visitors;

/// <summary>
/// Представляет травницу, которая посещает таверну.
/// Предпочитает блюда с овощами, травами и лёгкие напитки.
/// </summary>
public class Herbalist : Visitor, IRecipeTeacher
{
    public override string TypeName { get; set; } = "Травница";
    public override int Money { get; } = new Random().Next(30,40);
    public override List<string> PossibleNames { get; set; } = ["Меланья", "Аглая","Лукерья","Акулина","Феврония","Пелагея","Дарья",
        "Матрёна","Евдокия","Прасковья","Домника","Феодосия","Марфа","Устинья","Евпраксия","Василиса","Агафья","Ксения","Степанида","Соломонида"];
    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x =>x.Type is DishTypes.Овощное or DishTypes.Суп || x.Ingredients.ContainsKey("Травы") || x.Name.Contains("Чай") ).ToList();

    public override List<Item> AvaliableItemsForLeave { get; set; } = 
    [
        new ("Мешочек с травами", "Небольшой льняной мешочек с сушёными травами.", 4, 12),
        new ("Стеклянный флакон", "Маленький флакон с остатками лечебной настойки.", 3, 10),
        new ("Старая ступка", "Небольшая каменная ступка для измельчения трав.", 8, 20),
        new ("Серебряная игла", "Тонкая игла, которой травница зашивает мешочки с травами.", 12, 30),
        new ("Засушенный цветок", "Редкий цветок, аккуратно высушенный между страницами книги.", 2, 18)
    ];
    
    public bool HasRecipes { get; set; } = true;
    public List<Dish> SecretRecipes { get; set; } = DishCatalog.Dishes
        .Where(x => x.Id.StartsWith("herbalist_")).ToList();
        
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
            case < 101 :
                ShareRecipe(tavern);
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
                var result = tavern.BuyProducts(herbs, count, 1);
                GameOutput.ShowBuyProductResult(result, herbs, count);
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
        var result = tavern.BuyProducts(herbs, count, 0);
        GameOutput.ShowBuyProductResult(result, herbs, count);
    }

    /// <summary>
    /// Предлагает таверне изучить случайный секретный рецепт травницы.
    /// </summary>
    public void ShareRecipe(Tavern tavern)
    {
        Dish? dish = null;
        if (this is IRecipeTeacher visitor) dish = visitor.TrySetRecipeToTeach(dish, tavern);
        if (dish != null)
        {
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
    }
}