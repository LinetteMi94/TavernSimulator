using TavernSimulator.Data;
using TavernSimulator.Menus;
using TavernSimulator.Models;
using TavernSimulator.Service;

namespace TavernSimulator.Game;

/// <summary>
/// Управляет запуском игры и основным игровым процессом.
/// </summary>
public static class Game
{
    private static bool _isRunning = true;
    private static Tavern _tavern = new();
    private static Dictionary<Product, int> AvailableProductsInShopToday = new ();
    
    public static void Start()
    {
        _tavern.CreateTavern();
        Console.WriteLine("Добро пожаловать в таверну!");
        while (_isRunning)
        {
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
            ShowHeader();
            CreateAvailableProductsInShopToday();
            MainMenu.ShowMorningMenu(_tavern,HandleMorningMenuChoice);
        }
    }
    
    /// <summary>
    /// Отображает заголовок таверны и основную информацию о её текущем состоянии.
    /// </summary>
    public static void ShowHeader()
    {
        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine("║   🪿 ТАВЕРНА «ГУСЬ И ПИРОГ» 🥧     ║");
        Console.WriteLine("╠════════════════════════════════════╣ ");
        Console.WriteLine($"║ День: {_tavern.Day}                            ║");
        Console.WriteLine($"║ Золото: {_tavern.Gold}                        ║");
        Console.WriteLine("║                                    ║");
        Console.WriteLine($"║ Уровень: {_tavern.Level}                         ║");
        Console.WriteLine($"║ Опыт: {_tavern.Experience}                            ║");
        Console.WriteLine("║ Посетителей сегодня: 3             ║");
        Console.WriteLine("║ Выполнено заказов: 0               ║");
        Console.WriteLine("╚════════════════════════════════════╝");
    }

    private static void HandleMorningMenuChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                Console.Clear();
                ShowHeader();
                ShowTheFoodStorage();
                break;
            case 2:
                Console.Clear();
                ShowHeader();
                ShowTheShop();
                break;
            case 3:
                foreach (var dish in _tavern.AvailableDishes)
                {
                    Console.WriteLine($"{dish.Name}: {dish.Price} золотых монет");
                }
                break;
            case 4:
                break;
        }
    }

    /// <summary>
    /// Отображает список продуктов, хранящихся на складе таверны.
    /// </summary>
    private static void ShowTheFoodStorage()
    {
        Console.WriteLine("\nПродуктовый склад: \n");
        if (_tavern.Products.Count == 0) Console.WriteLine("Продуктовый склад пуст!");
        else
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"|  {"Продукт",-20}|{"Количество",14} |");
            Console.WriteLine(new string('-', 40));
            foreach (var item in _tavern.Products)
            {
                Console.WriteLine($"|  {item.Key,-20}|{item.Value,10} шт. |");
            }
            Console.WriteLine(new string('-', 40));
        }
    }

    private static void CreateAvailableProductsInShopToday()
    {
        AvailableProductsInShopToday = new Dictionary<Product, int>();
        var availableProducts = TavernService.ProductCatalog.Products
            .Where(product => product.RequiredTavernLevel <= _tavern.Level)
            .OrderBy(x => new Random().Next()).Take(7).ToList();
        foreach (var product in availableProducts)
        {
            AvailableProductsInShopToday.Add(product, new Random().Next(4,9));
        }
    }
    
    /// <summary>
    /// Отображает список продуктов, хранящихся на складе таверны.
    /// </summary>
    private static void ShowTheShop()
    {
        Console.WriteLine("\nМагазин: \n");
        Console.WriteLine(new string('-', 66));
        Console.WriteLine($"|  №   | {"Продукт",-20} | {"Цена",15} | {"Количество",14} |");
        Console.WriteLine(new string('-', 66));
        int index = 1;
        foreach (var product in AvailableProductsInShopToday)
        {
            Console.WriteLine($"| {index,3}  | {product.Key.Name,-20} | {product.Key.Price,10} зол. | {product.Value,10} шт. |");
            index++;
        }
        Console.WriteLine(new string('-', 66));
    }
}