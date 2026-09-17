using TavernSimulator.Input;
using TavernSimulator.Models;

namespace TavernSimulator.Menus;

/// <summary>
/// Отвечает за отображение игровых меню и управление основными этапами игрового дня.
/// </summary>
public static class MainMenu
{
    private static Tavern _tavern;
    
    /// <summary>
    /// Отображает заголовок таверны и основную информацию о её текущем состоянии.
    /// </summary>
    private static void ShowHeader()
    {
        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine("║   🪿 ТАВЕРНА «ГУСЬ И ПИРОГ» 🥧     ║");
        Console.WriteLine("╠════════════════════════════════════╣ ");
        Console.WriteLine($"║ День: {_tavern.Day}                            ║");
        Console.WriteLine($"║ Золото: {_tavern.Gold}                        ║");
        Console.WriteLine("║                                    ║");
        Console.WriteLine($"║ Репутация: {_tavern.Reputation}                       ║");
        Console.WriteLine("║ Посетителей сегодня: 3             ║");
        Console.WriteLine("║ Выполнено заказов: 0               ║");
        Console.WriteLine("╚════════════════════════════════════╝");
    }

    /// <summary>
    /// Отображает утреннее меню и действия, необходимые для подготовки таверны к открытию.
    /// </summary>
    public static void ShowMorningMenu(Tavern tavern)
    {
        _tavern = tavern;
        ShowHeader();
        Console.WriteLine();
        Console.WriteLine("1. Посмотреть запасы");
        Console.WriteLine("2. Купить продукты");
        Console.WriteLine("3. Посмотреть меню");
        Console.WriteLine("4. Открыть таверну");
        var input = InputValidator.GetValidInput(null, 4);
        switch (input)
        {
            case 1:
                Console.Clear();
                ShowHeader();
                Console.WriteLine("\nПродуктовый склад: \n");
                if (_tavern.Products.Count == 0) Console.WriteLine("Продуктовый склад пуст!");
                else
                {
                    foreach (var item in _tavern.Products)
                    {
                       // Console.WriteLine($"{item.Key.Name}: {item.Value} золотых монет");
                    }
                }
                break;
            case 2:
                Console.WriteLine("Купить продукты");
                break;
            case 3:
                foreach (var dish in _tavern.AvailableDishes)
                {
                    //Console.WriteLine($"{dish.Name}: {dish.Price} золотых монет");
                }
                break;
            case 4:
                ShowDayMenu();
                break;
        }
    }

    /// <summary>
    /// Отображает дневное меню и предоставляет игроку доступ к основным действиям во время работы таверны.
    /// </summary>
    private static void ShowDayMenu()
    {
        ShowHeader();
        Console.WriteLine();
        Console.WriteLine("1. Закрыть таверну");
        var input = InputValidator.GetValidInput(null, 1);
        switch (input)
        {
            case 1:
                ShowEveningMenu();
                break;
        }
    }

    /// <summary>
    /// Отображает вечернее меню с итогами игрового дня и позволяет завершить день.
    /// </summary>
    private static void ShowEveningMenu()
    {
        ShowHeader();
        Console.WriteLine($"День {_tavern.Day} завершён!");
        Console.WriteLine();
        Console.WriteLine("1. Посмотреть запасы");
        Console.WriteLine("2. Купить продукты");
        Console.WriteLine("3. Идти спать");
        var input = InputValidator.GetValidInput(null, 3);
        switch (input)
        {
            case 1:
                Console.Clear();
                ShowHeader();
                Console.WriteLine("\nПродуктовый склад: \n");
                if (_tavern.Products.Count == 0) Console.WriteLine("Продуктовый склад пуст!");
                else
                {
                    foreach (var item in _tavern.Products)
                    {
                        // Console.WriteLine($"{item.Key.Name}: {item.Value} золотых монет");
                    }
                }
                break;
            case 2:
                Console.WriteLine("Купить продукты");
                break;
            case 3:
                _tavern.Day++;
                ShowMorningMenu(_tavern);
                break;
        }
    }
}