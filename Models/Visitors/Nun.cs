using System;
using System.Collections.Generic;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Service;

namespace TavernSimulator.Models.Visitors;

/// <summary>
/// Представляет монахиню, которая посещает таверну и делится секретными рецептами монастырской кухни.
/// </summary>
public class Nun : Visitor, IRecipeTeacher
{
    public override string TypeName { get; set; } =  "Монахиня"; 
    public override int Money { get; } = new Random().Next(30,45);
    public override List<string> PossibleNames { get; set; } = ["Агнесса","Беатриса","Клара",
        "Марта","Матильда","Розалия","Эльза","Хильда","Эльвира","Адела","Гертруда","Бригитта"];
     public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x =>x.Type is DishTypes.Сладкое or DishTypes.Напиток or DishTypes.Выпечка ).ToList();
     public bool HasRecipes { get; set; } = true;
     public override List<Item> AvaliableItemsForLeave { get; set; } = 
     [
         new ("Деревянные чётки", "Небольшие чётки из светлого дерева.", 5, 20),
         new ("Маленький крестик", "Простой деревянный крестик на шнурке.", 4, 18),
         new ("Монастырская свеча", "Небольшая свеча из пчелиного воска.", 3, 12),
         new ("Мешочек с лавандой", "Льняной мешочек с сушёной лавандой.", 4, 15),
         new ("Старая молитвенная книга", "Небольшая книга с потёртой обложкой.", 10, 35)
     ];
    
     public List<Dish> SecretRecipes { get; set; } = DishCatalog.Dishes
         .Where(x => x.Id.StartsWith("nun_")).ToList();
    
     
        public override void OnEvent(Tavern tavern)
        {
            ShareRecipe(tavern);
        }

        public void ShareRecipe(Tavern tavern)
        {
            Dish? dish = null;
            if (this is IRecipeTeacher visitor) dish = visitor.TrySetRecipeToTeach(dish, tavern);
            if (dish != null)
            {
                Console.WriteLine($"\nМонахиня {Name} хитро на тебя смотрит:\n" +
                                  $"«В монастыре мы часто готовим кабачки. Есть один рецепт, который особенно хорошо получается осенью. Хочешь, я поделюсь им?»");
                Console.WriteLine($"\nНазывается блюдо - {dish.Name}. Хочешь научу?");
                Console.WriteLine("1. Да\n2. Нет\n");
                var choice = Input.InputValidator.GetValidInput(2);
                switch (choice)
                {
                    case 1:
                        Console.WriteLine(
                            "Этот рецепт у нас берегут особенно тщательно. Я нечасто делюсь им с посторонними... Но тебе, пожалуй, доверю.");
                        tavern.LearnSecretDish(dish);
                        SecretRecipes.Remove(dish);
                        break;
                    case 2:
                        Console.WriteLine("Как пожелаешь. Рецепт никуда не убежит.");
                        break;
                }
            }
        }
}