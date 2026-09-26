using System;
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
        /// <param name="handleChoice">Метод, обрабатывающий выбранный пользователем пункт меню.</param>
        public static void ShowMorningMenu(Action<int> handleChoice)
        {
            Game.Game.ShowHeader();
            Console.WriteLine();
            Console.WriteLine("1. Посмотреть запасы");
            Console.WriteLine("2. Купить продукты");
            Console.WriteLine("3. Посмотреть меню");
            Console.WriteLine("4. Изучить новые рецепты");
            Console.WriteLine("5. Открыть таверну");
            var input = InputValidator.GetValidInput(5);
            handleChoice(input);
        }
        
        /// <summary>
        /// Отображает меню магазина и позволяет покупать продукты в таверну.
        /// </summary>
        /// <param name="handleChoice">Метод, обрабатывающий выбранный пользователем пункт меню.</param>
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
        
        /// <summary>
        /// Отображает меню обслуживания посетителя и действия, необходимые для его обслуживания.
        /// </summary>
        /// <param name="handleChoice">Метод, обрабатывающий выбор пользователя и список выбранных блюд.</param>
        /// <param name="dishes">Список блюд, доступных для заказа.</param>
        public static void ServeVisitorMenu(Action<int, List<Dish>> handleChoice, List<Dish> dishes)
        {
            Console.WriteLine();
            Console.WriteLine("1. Посмотреть рецепты");
            Console.WriteLine("2. Накормить");
            Console.WriteLine("3. Прогнать");
            var input = InputValidator.GetValidInput(3);
            handleChoice(input, dishes);
        }
        
        /// <summary>
        /// Отображает меню выбора блюд, доступных для изучения, и позволяет изучать их.
        /// </summary>
        /// <param name="handleChoice">Метод, обрабатывающий выбранный пользователем пункт меню.</param>
        public static void ShowLearnDishesMenu(Action handleChoice)
        { 
            Console.WriteLine();
            Console.WriteLine("1. Изучить рецепт");
            Console.WriteLine("2. Назад");
            var input = InputValidator.GetValidInput(2);
            if (input == 1) handleChoice();
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
