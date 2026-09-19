using TavernSimulator.Input;
using TavernSimulator.Models;

namespace TavernSimulator.Menus
{
    /// <summary>
    /// Отвечает за отображение игровых меню и управление основными этапами игрового дня.
    /// </summary>
    public static class MainMenu
    {
        /// <summary>
        /// Отображает утреннее меню и действия, необходимые для подготовки таверны к открытию.
        /// </summary>
        public static void ShowMorningMenu(Action<int> handleChoice)
        {
            Game.Game.ShowHeader();
            Console.WriteLine();
            Console.WriteLine("1. Посмотреть запасы");
            Console.WriteLine("2. Купить продукты");
            Console.WriteLine("3. Посмотреть меню");
            Console.WriteLine("4. Открыть таверну");
            var input = InputValidator.GetValidInput(4);
            handleChoice(input);
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Отображает дневное меню и предоставляет игроку доступ к основным действиям во время работы таверны.
        /// </summary>
        public static void ShowDayMenu(Action<int> handleChoice)
        {
            Game.Game.ShowHeader();
            Console.WriteLine();
            Console.WriteLine("1. Закрыть таверну");
            var input = InputValidator.GetValidInput(1);
            handleChoice(input);
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        /// Отображает вечернее меню с итогами игрового дня и позволяет завершить день.
        /// </summary>
        public static void ShowEveningMenu(int day, Action<int> handleChoice)
        {
            Game.Game.ShowHeader();
            Console.WriteLine($"День {day} завершён!");
            Console.WriteLine();
            Console.WriteLine("1. Посмотреть запасы");
            Console.WriteLine("2. Купить продукты");
            Console.WriteLine("3. Идти спать");
            var input = InputValidator.GetValidInput(3);
            handleChoice(input);
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Отображает меню магазина и позволяет покупать продукты в таверну.
        /// </summary>
        public static void ShowShopMenu(Action handleChoice)
        { 
            Console.WriteLine();
            Console.WriteLine("1. Купить продукт");
            Console.WriteLine("2. Назад");
            var input = InputValidator.GetValidInput(2);
            if (input == 1) handleChoice();
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
