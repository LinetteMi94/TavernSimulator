using System;
using System.Collections.Generic;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Input;
using TavernSimulator.Menus;
using TavernSimulator.Models;
using TavernSimulator.Models.Visitors;
using TavernSimulator.Service;

namespace TavernSimulator.Game;

/// <summary>
/// Управляет запуском игры и основным игровым процессом.
/// </summary>
public static class Game
{
    private static bool _isRunning = true;
    private static bool _isMorning;
    private static bool _isVisitorBeingServed;
    private static Tavern _tavern = new();
    private static int _visitorsToday;
    private static int _moneyToday;
    private static int _experienceToday;
    private static int _completedOrdersToday;
    private static Dictionary<Product, int> _availableProductsInShopToday = new ();
    
    public static void Start()
    {
        _tavern.CreateTavern();
        Console.WriteLine("Добро пожаловать в таверну!");
        Console.WriteLine("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
        while (_isRunning)
        {
            Console.Clear();
            _completedOrdersToday = 0;
            _moneyToday = 0;
            _experienceToday = 0;
            ShowHeader();
            CreateAvailableProductsInShopToday();
            _isMorning = true;
            while (_isMorning)
            {
                MainMenu.ShowMorningMenu(HandleMorningMenuChoice);
            }
            OpenTavern();
            ViewDailyStatistics();
        }
    }

    /// <summary>
    /// Определяет количество посетителей на сегодняшний день.
    /// </summary>
    private static void GetVisitorsToday()
    {
        _visitorsToday = new Random().Next(2, 6);
    }
    
    /// <summary>
    /// Показывает необходимые ингридиенты для блюда и количество необходимых ингридиентов в таверне.
    /// </summary>
    private static void ShowDishesIngridients(List<Dish> dishes)
    {
        Console.Clear();
        foreach (var dish in dishes)
        {
            Console.WriteLine($"\nИнгридиенты для блюда {dish.Name}:\n");
            var counter = 1;
            foreach (var ingrid in dish.Ingredients)
            { 
                var product = _tavern.Products.Where(x => x.Key == ingrid.Key).Select(x =>  x.Value).First();
                Console.WriteLine($"{counter++}. {ingrid.Key, -20} -{ingrid.Value,3} шт.     (В наличии {product,2} шт.)");
            }

            Console.WriteLine();
        }
    }
    
    /// <summary>
    /// Организует взаимодействие с посетителем до завершения его обслуживания.
    /// </summary>
    private static void ServeVisitor(this Visitor visitor, List<Dish> dishes)
    {
        Console.WriteLine(visitor.Name + " хочет заказать : ");
        for (var i=0; i<dishes.Count; i++)
        {
            Console.Write($"{i+1}. {dishes[i].Name}\n");
        }

        MainMenu.ServeVisitorMenu(HandleServeVisitorMenuChoice, dishes); 
        Console.ReadKey();
        Console.Clear();
    }

    /// <summary>
    /// Обрабатывает выбор игрока в утреннем меню таверны.
    /// </summary>
    private static void HandleServeVisitorMenuChoice(int choice, List<Dish> dishes)
    {
        switch (choice)
        {
            case 1:
                ShowDishesIngridients(dishes);
                break;
            case 2:
                var IsCooking = _tavern.CookOrder(dishes);
                if (!IsCooking) Console.WriteLine("Посетитель уходит голодный");
                else
                {
                    Console.WriteLine($"Вы отдаёте заказ посетителю.");
                    _tavern.Gold += dishes.Sum(x => x.Price);
                    _moneyToday += dishes.Sum(x => x.Price);
                    _experienceToday += dishes.Sum(x => x.Experience);
                    _completedOrdersToday++;
                }
                _isVisitorBeingServed = false;
                break;
            case 3:
                Console.WriteLine("Вы прогнали поcетителя!");
                _isVisitorBeingServed = false;
                break;
        }
    }
    
    /// <summary>
    /// Открывает таверну и организует обслуживание посетителей в течение дня.
    /// </summary>
    private static void OpenTavern()
    {
        GetVisitorsToday();
        for (int i = 0; i < _visitorsToday; i++)
        {
            Console.Clear();
            ShowHeader();
            Visitor visitor = VisitorService.CreateVisitor();
            Console.WriteLine("Новый посетитель: " + visitor.TypeName + " " +  visitor.Name);
            _isVisitorBeingServed = true;
            var dishes = visitor.ChooseOrder(_tavern);
            while (_isVisitorBeingServed)
            {
                visitor.ServeVisitor(dishes);
            }
        }
    }
    
    /// <summary>
    /// Отображает заголовок таверны и основную информацию о её текущем состоянии.
    /// </summary>
    public static void ShowHeader()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine("║   🪿 ТАВЕРНА «ГУСЬ И ПИРОГ» 🥧     ║");
        Console.WriteLine("╠════════════════════════════════════╣ ");
        Console.WriteLine($"║ День: {_tavern.Day}                            ║");
        Console.WriteLine($"║ Золото: {_tavern.Gold}                        ║");
        Console.WriteLine("║                                    ║");
        Console.WriteLine($"║ Уровень: {_tavern.Level}                         ║");
        Console.WriteLine($"║ Опыт: {_tavern.Experience}                            ║");
        Console.WriteLine($"║ Посетителей сегодня: {_visitorsToday}             ║");
        Console.WriteLine($"║ Выполнено заказов: {_completedOrdersToday}               ║");
        Console.WriteLine("╚════════════════════════════════════╝");
    }

    /// <summary>
    /// Обрабатывает выбор игрока в утреннем меню таверны.
    /// </summary>
    private static void HandleMorningMenuChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                ShowTheFoodStorage();
                break;
            case 2:
                ShowTheShop();
                MainMenu.ShowShopMenu(HandleShopMenuChoice);
                break;
            case 3:
                ShowTavernMenu();
                break;
            case 4:
               ShowAvailableDishesToLearn();
               break;
            case 5:
                _isMorning = false;
                break;
        }
    }
    
    /// <summary>
    /// Выводит в консоль статистику за день.
    /// </summary>
    private static void ViewDailyStatistics()
    {
        ShowHeader();
        Console.WriteLine($"День {_tavern.Day} завершён!");
        Console.WriteLine();
        Console.WriteLine($"Обслужено посетителей: {_completedOrdersToday}");
        Console.WriteLine($"Заработано золотых: {_moneyToday}");
        Console.WriteLine($"Получено опыта: {_experienceToday}");
        Console.WriteLine("\nНажмите любую клавишу для завершения дня...");
        Console.ReadKey();
    }
    
    /// <summary>
    /// Покупает указанное количество продукта в магазине, списывает золото
    /// и добавляет приобретённый продукт на склад таверны.
    /// </summary>
    private static void HandleShopMenuChoice()
    {
        Console.WriteLine("Какой продукт необходимо приобрести?");
        var index = InputValidator.GetValidInput(_availableProductsInShopToday.Count);
        var product = _availableProductsInShopToday.ElementAt(index - 1).Key;
        Console.WriteLine("В каком количестве?");
        var count = InputValidator.GetValidInput(_availableProductsInShopToday.ElementAt(index-1).Value);
        if (_tavern.Gold >= product.Price * count)
        {
            _tavern.Gold -= product.Price * count;
            _availableProductsInShopToday[product] -= count;
            _tavern.Products.TryAdd(product.Name, 0);
            _tavern.Products[product.Name] += count;
            Console.WriteLine($"Куплено {product.Name} - {count} шт.!");
            if (_availableProductsInShopToday.ElementAt(index - 1).Value == 0) _availableProductsInShopToday.Remove(product);
        }
        else Console.WriteLine("Недостаточно золота!");
    }

    /// <summary>
    /// Отображает список продуктов, хранящихся на складе таверны.
    /// </summary>
    private static void ShowTheFoodStorage()
    {
        Console.Clear();
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
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    /// <summary>
    /// Создаёт список продуктов, которые сегодня будут продаваться в магазине.
    /// </summary>
    private static void CreateAvailableProductsInShopToday()
    {
        _availableProductsInShopToday = new Dictionary<Product, int>();
        var availableProducts = ProductCatalog.Products
            .Where(product => product.RequiredTavernLevel <= _tavern.Level)
            .OrderBy(x => new Random().Next()).Take(7).ToList();
        foreach (var product in availableProducts)
        {
            _availableProductsInShopToday.Add(product, new Random().Next(4,9));
        }
    }
    
    /// <summary>
    /// Отображает список продуктов, которые сегодня лежат на прилавке магазина.
    /// </summary>
    private static void ShowTheShop()
    {
        Console.Clear();
        Console.WriteLine($"\nТаверна может потратить {_tavern.Gold} зол.");
        Console.WriteLine("\nМагазин: \n");
        Console.WriteLine(new string('-', 66));
        Console.WriteLine($"|  №   | {"Продукт",-20} | {"Цена",15} | {"Количество",14} |");
        Console.WriteLine(new string('-', 66));
        int index = 1;
        foreach (var product in _availableProductsInShopToday.Where(product => product.Value != 0))
        {
            Console.WriteLine($"| {index,3}  | {product.Key.Name,-20} | {product.Key.Price,10} зол. | {product.Value,10} шт. |");
            index++;
        }
        Console.WriteLine(new string('-', 66));
    }
    
    /// <summary>
    /// Отображает блюда, доступные для заказа в таверне.
    /// </summary>
    private static void ShowTavernMenu()
    {
        Console.Clear();
        Console.WriteLine("\nМеню таверны \"Гусь и Пирог\": \n");
        Console.WriteLine(new string('-', 47));
        Console.WriteLine($"|  №   | {"Блюдо",-20}|{"Цена",15} |");
        Console.WriteLine(new string('-', 47));
        int index = 1;
        foreach (var dish in _tavern.AvailableDishes)
        {
            Console.WriteLine($"| {index,3}  | {dish.Name,-20}|{dish.Price,10} зол. |");
            index++;
        }
        Console.WriteLine(new string('-', 47));
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
    
    /// <summary>
    /// Отображает рецепты, доступные для изучения.
    /// </summary>
    private static void ShowAvailableDishesToLearn()
    {
        Console.Clear();
        Console.WriteLine("\nДоступные блюда для изучения: \n");
        var dishes = DishCatalog.Dishes.Where(x => x.RequiredTavernLevel <= _tavern.Level && !_tavern.AvailableDishes.Contains(x)).ToList();
        if (dishes.Count == 0) Console.WriteLine("Доступных блюд для изучения нет!");
        else
        {
            Console.WriteLine(new string('-', 66));
            Console.WriteLine($"|  №  |  {"Блюдо",-30}| {"Необходимые продукты",23} |");
            Console.WriteLine(new string('-', 66));
            var index = 1;
            foreach (var item in dishes)
            {
                Console.WriteLine($"| {index,2}  | {item.Name,-30}                           |");
                foreach (var ingredient in item.Ingredients)
                {
                    var product = ProductCatalog.Products.First(x => x.Name == ingredient.Key).Name;
                    Console.WriteLine($"|     | {product,48} - 1 шт. |");
                }
                Console.WriteLine(new string('-', 66));
                index++;
            }
        }
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}