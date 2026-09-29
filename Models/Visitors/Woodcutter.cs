using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Game;
using TavernSimulator.Service;

namespace TavernSimulator.Models.Visitors;

/// <summary>
/// Представляет лесоруба, который посещает таверну.
/// Предпочитает сытные мясные блюда, а также напитки.
/// </summary>
public class Woodcutter: Visitor
{
    public override string TypeName { get; set; } = "Лесоруб";
    public override int Money { get; } = new Random().Next(30,40);
    public override List<string> PossibleNames { get; set; } = [ "Прохор", "Тихон", "Фёдор", "Степан", "Трофим", "Кузьма", "Макар",
        "Ефим", "Гаврила", "Лука", "Яков", "Пахом", "Савва", "Елисей", "Мирон", "Игнат", "Наум", "Фома", "Данила", "Егор"];
    public override List<Dish> PreferredDishes => DishCatalog.Dishes.Where(x => x.Type is DishTypes.Мясное or DishTypes.Напиток).ToList();
    
    public override void OnEvent(Tavern tavern)
    {   
        var random = Random.Shared.Next(100);
        switch (random)
        {
            case < 33 : 
                EatMore(tavern);
                break;
            case < 66 :
                GiveMushrooms(tavern);
                break;
            case < 101 :
                BuyBreadForWife(tavern);
                break;
        }
    }
    
    /// <summary>
    /// Предлагает лесорубу купить хлеб для жены по повышенной цене.
    /// </summary>
    private void BuyBreadForWife(Tavern tavern)
    {
        var count = new Random().Next(2, 5);
        Console.WriteLine($"\nЛесоруб {Name} хитро на тебя смотрит:\n" +
                          $"«Жена за хлебом послала. Сказала, чтобы без хлеба домой не возвращался. Есть у тебя {count} шт.?»");
        var bread = tavern.Products.FirstOrDefault(x => x.Key == "Хлеб");
        Console.WriteLine(bread.Value > 0
            ? $"\n(В таверне хлеба в наличии: {bread.Value} шт.)\n"
            : $"\n(В таверне хлеба в наличии нет.)\n");
        Console.WriteLine("1. Есть. Держи. Только у меня он подороже будет.\n2. Нет\n");
        var choice = Input.InputValidator.GetValidInput(2);
        switch (choice)
        {
            case 1:
                Console.WriteLine("Лесоруб: Ну а куда деваться… Сказал же, без хлеба домой не возвращаться.");
                var result = tavern.SellRequestedProduct(bread, count,6);
                GameOutput.ShowSellProductResult(result, bread.Key, count, 6);
                Console.WriteLine(result == SellProductResult.SaleSuccess
                    ? "Лесоруб: Фух, спас меня. А то жена меня бы без хлеба обратно отправила."
                    : "Лесоруб: Ну всё… чувствую, сегодня мне домой лучше не спешить.");
                break;
            case 2:
                Console.WriteLine("Лесоруб: Нет хлеба? Вот же напасть. Теперь ещё в другой конец деревни идти.");
                break;
        }
    }
    /// <summary>
    /// Предлагает таверне приготовить дополнительное блюдо для голодного лесоруба.
    /// </summary>
    private void EatMore(Tavern tavern)
    {
        Console.WriteLine($"\nЛесоруб {Name} недоумевающе смотрит на тарелку:\n«Уф… день сегодня тяжёлый выдался. Поел, а всё равно будто пустой. Давай-ка ещё чего-нибудь. Самое сытное, что у тебя есть.»");
        var dish = tavern.AvailableDishes.OrderByDescending(x => x.Price).First();
        Console.WriteLine($"\nЛесоруб просит приготовить\n {dish.Name}");
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
                    Console.WriteLine("Вот это я понимаю. Неси скорее, пока совсем с ног не свалился.");
                }
                break;
            case 2:
                Console.WriteLine("Ну и ладно. Пойду домой, может, там ещё что-нибудь осталось.");
                break;
        }
    }
    
    /// <summary>
    /// Передаёт таверне найденные в лесу грибы в качестве бесплатного продукта.
    /// </summary>
    private void GiveMushrooms(Tavern tavern)
    {
        var count = new Random().Next(3, 6);
        Console.WriteLine($"\nЛесоруб {Name} смущенно смотрит:\n«Сытная еда, спасибо!" +
                          $"\nПока дрова рубил, на грибы наткнулся. Набрал немного. Держи, тебе пригодятся. Продавать не буду, я всё-таки не торговец.»");
        var mushrooms = ProductCatalog.Products.First(x => x.Name == "Грибы");
        var result = tavern.BuyProducts(mushrooms, count, 0);
        GameOutput.ShowBuyProductResult(result, mushrooms, count);
    }
}