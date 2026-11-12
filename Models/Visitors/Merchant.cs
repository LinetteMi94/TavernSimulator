using TavernSimulator.Data;
using TavernSimulator.Game;
using TavernSimulator.Service;

namespace TavernSimulator.Models.Visitors;

/// <summary>
/// Представляет торговца, который посещает таверну.
/// Предпочитает редкие блюда.
/// </summary>
public class Merchant : Visitor
{
    public override string TypeName { get; set; } = "Торговец";
    public override int Money { get; } = new Random().Next(50,60);
    public override List<string> PossibleNames { get; set; } = ["Аникей", "Влас", "Гордей", "Ермил", "Наум", "Савватий", "Дорофей",
    "Кузьма", "Велемир", "Радмир", "Боримир", "Евстигней", "Иов", "Онуфрий", "Филат", "Прокопий", "Сысой", "Агапит", "Панкрат", "Ларион"];
    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x => !x.Id.StartsWith("base")).ToList();
    public override List<Item> AvaliableItemsForLeave { get; set; } =
    [
        new ("Фрагмент карты", "Потрёпанный клочок пергамента с выцветшими линиями и пометкой карандашом.", 7, 20),
        new ("Старый компас без стрелки", "Латунный компас с треснутым стеклом, стрелки нет, внутри видна потёртая разметка.", 4, 11),
        new ("Фляга с водой", "Кожаная фляга с грубой прошивкой, пахнет сыростью, а внутри плещется немного воды.", 3, 9),
        new ("Маленький колокольчик", "Крошечный медный колокольчик на изношенном шнурке, язычок слегка погнут.", 5, 15),
        new ("Серебряное кольцо с рунами", "Тонкое серебряное кольцо, руны стёрты от времени, местами видна патина.", 10, 35)
    ];
    public override void OnEvent(Tavern tavern)
    {
        TradeGoods(tavern);
    }

    private void TradeGoods(Tavern tavern)
    {
        Console.WriteLine($"\nТорговец {Name} улыбается:\n«Доброго дня, хозяин! " +
                          $"Есть у меня кое-что интересное. Предлагаю обмен: ты мне свои товары, " +
                          $"а я тебе свои. Думаю, оба останемся в выигрыше. Ну что, поторгуем?");
        Console.WriteLine("1. Да\n2. Нет\n");
        var choice = Input.InputValidator.GetValidInput(2);
        if (choice == 2)
        {
            Console.WriteLine("Что ж, твоё дело, хозяин. Может, в другой раз договоримся. Удачной торговли!");
            return;
        }
        Console.WriteLine($"\nВот это деловой подход! Тогда давай посмотрим, что у нас есть для обмена. " +
                          "Выбирай, хозяин, не стесняйся. Только помни: купец я опытный, " +
                          "задаром товар не отдаю! (клавиша 4 - отказ) \n");
        
        Product[,] goods = new Product[3, 2];
        int[,] counts = new int[3, 2];
        for (int i = 0; i < 3; i++)
        {
            goods[i, 0] = ProductCatalog.Products[Random.Shared.Next(ProductCatalog.Products.Count)];
            counts[i,0] = Random.Shared.Next(2,6);
            counts[i,1] = Random.Shared.Next(2,6);
            var tavernProducts = tavern.Products.Where(x => x.Value >= counts[i,1]).ToList();
            var randomTavernProduct = tavernProducts[Random.Shared.Next(tavernProducts.Count)];
            goods[i, 1] = ProductCatalog.Products.First(x => x.Name == randomTavernProduct.Key);
            Console.WriteLine($"{i+1}. Я отдаю тебе {counts[i,0]} {goods[i, 0].Name}, " +
                              $"ты отдаёшь мне {counts[i,1]} {goods[i, 1].Name}.");
        }
        
        var choice2 = Input.InputValidator.GetValidInput(4)-1;
        switch (choice2)
        {
            case 0:
            case 1:
            case 2:
                tavern.RemoveProduct(goods[choice2,1].Name, counts[choice2,1]);
                var result = tavern.BuyProducts(goods[choice2,0], counts[choice2,0], 0);
                GameOutput.ShowBuyProductResult(result, goods[choice2,0], counts[choice2,0]);
                tavern.ChangeReputation(3);
                break;
            case 3:
                tavern.ChangeReputation(1,false);
                Console.WriteLine("Что ж, твоё дело, хозяин. Может, в другой раз договоримся. Удачной торговли!");
                break;
        }
    }
}