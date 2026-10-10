using System;
using System.Collections.Generic;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Enums;
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
    
    /// <summary>
    /// Создаёт список продуктов, которые сегодня будут продаваться в магазине.
    /// </summary>
    private static void CreateAvailableProductsInShopToday()
    {
        _availableProductsInShopToday = new Dictionary<Product, int>();
        var availableProducts = ProductCatalog.Products
            .Where(product => product.RequiredTavernLevel <= _tavern.Level)
            .OrderBy(_ => Random.Shared.Next()).Take(7).OrderBy(x => x.Name).ToList();
        foreach (var product in availableProducts)
        {
            if (product.RequiredTavernLevel < 3)
            {
                _availableProductsInShopToday.Add(product, new Random().Next(5,12));
                continue;
            }
            _availableProductsInShopToday.Add(product, new Random().Next(4,9));
        }
    }
    
    /// <summary>
    /// Определяет количество посетителей на сегодняшний день.
    /// </summary>
    private static void GetVisitorsToday() => _visitorsToday = new Random().Next(2, 6);
    
    /// <summary>
    /// Изучает указанный рецепт, списывает продукты на изучение
    /// и добавляет изученный рецепт в меню таверны.
    /// </summary>
    private static void HandleLearnDishesMenuChoice()
    {
        Console.WriteLine("Какой рецепт хотите выучить сегодня?");
        var dishes = DishCatalog.Dishes.Where(x => x.RequiredTavernLevel <= _tavern.Level && !_tavern.AvailableDishes.Contains(x) && x.Id.StartsWith("base_")).ToList();
        var index = InputValidator.GetValidInput(dishes.Count);
        var dish = dishes.ElementAt(index - 1);
        
        foreach (var ingrid in dish.Ingredients)
        {
            if  (!_tavern.Products.ContainsKey(ingrid.Key))
            {
                Console.WriteLine(ingrid.Key);
                Console.WriteLine($"Вы не знаете ингридиентов, из которых готовят {dish.Name}!");
                return;
            }
            var product = _tavern.Products.First(x => x.Key == ingrid.Key);
            if (product.Value < 1)
            {
                Console.WriteLine($"У вас недостаточно {product.Key}!");
                return;
            }
        }
        var result = _tavern.LearnDish(dish);
        GameOutput.ShowLearnDishResult(result, dish);
    }
    
    /// <summary>
    /// Обрабатывает выбор игрока в утреннем меню таверны.
    /// </summary>
    private static void HandleMorningMenuChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                _isMorning = false;
                break;
            case 2:
                CheckProductAvailability();
                break;
            case 3:
                if (ShowTheShop()) MainMenu.ShowShopMenu(HandleShopMenuChoice);
                break;
            case 4:
                LookUnderCounter();
                break;
            case 5:
                ShowTavernMenu();
                break;
            case 6:
                if(ShowAvailableDishesToLearn()) MainMenu.ShowLearnDishesMenu(HandleLearnDishesMenuChoice);
                break;
            case 7:
                GameOutput.ShowAchievements();
                break;
        }
    }
    
    /// <summary>
    /// Обрабатывает выбор игрока в утреннем меню таверны.
    /// </summary>
    private static void HandleServeVisitorMenuChoice(int choice, List<Dish> dishes, Visitor visitor)
    {
        switch (choice)
        {
            case 1:
                _tavern.ShowDishesIngridients(dishes);
                break;
            case 2:
                ReturnVisitorItem(visitor);
                var IsCooking = _tavern.CookOrder(dishes);
                if (!IsCooking) Console.WriteLine("Посетитель уходит голодный");
                else
                {
                    Console.WriteLine($"Вы отдаёте заказ посетителю.");
                    _tavern.Gold += dishes.Sum(x => x.Price);
                    _moneyToday += dishes.Sum(x => x.Price);
                    _experienceToday += dishes.Sum(x => x.Experience);
                    _completedOrdersToday++;
                    visitor.TriggerEvent(_tavern);
                }
                var leaveItem = visitor.LeaveRandomItem();
                if (leaveItem != null)
                {
                    MainMenu.ShowFoundItemMenu(leaveItem, _tavern.AddFoundItem);
                    
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
    /// Покупает указанное количество продукта в магазине, списывает золото
    /// и добавляет приобретённый продукт на склад таверны.
    /// </summary>
    private static void HandleShopMenuChoice()
    {
        Console.WriteLine("Какой продукт необходимо приобрести? (Или нажать 0 для выхода в меню)");
        var index = InputValidator.GetValidInput(_availableProductsInShopToday.Count, 0);
        if (index == 0) return;
        var product = _availableProductsInShopToday.ElementAt(index - 1).Key;
        Console.WriteLine("В каком количестве? (Или нажать 0 для выхода в меню)");
        var count = InputValidator.GetValidInput(_availableProductsInShopToday.ElementAt(index-1).Value, 0);
        if (count == 0) return;
        if (_tavern.Gold >= product.Price * count)
        {
            _availableProductsInShopToday[product] -= count;
             var result = _tavern.BuyProducts(product, count);
             GameOutput.ShowBuyProductResult(result, product,count);
            if (_availableProductsInShopToday.ElementAt(index - 1).Value == 0) _availableProductsInShopToday.Remove(product);
        }
        else GameOutput.ShowBuyProductResult(PurchaseProductResult.NotEnoughGold, product,count);
    }

    /// <summary>
    /// Отображает список найденных вещей, оставленных на хранение в таверне.
    /// </summary>
    private static void LookUnderCounter()
    {
        GameOutput.ShowFoundItems(_tavern);
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
    /// Обрабатывает возвращение найденной вещи посетителю.
    /// </summary>
    /// <param name="visitor">Посетитель, которому принадлежит найденная вещь.</param>
    private static void ReturnVisitorItem(Visitor visitor)
    {
        var item = _tavern.FindVisitorItem(visitor);
        if (item == null) return;
        Console.WriteLine("\nВы: Кажется, это ваше. Нашлось под прилавком. Хорошо, что вы вернулись!");
        Console.WriteLine($"Вы достаёте {item.Name} и подталкиваете к посетителю.");
        Console.WriteLine($"{visitor.Name} удивленно смотрит.");
        Console.WriteLine($"{visitor.Name}: Вы сохранили мою вещь? Благодарю. Подобная честность встречается нечасто. Примите мою благодарность.");
        Console.WriteLine($"В благодарность {visitor.Name} оставляет на прилавке {item.OwnerPrice} зол.");
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

        MainMenu.ServeVisitorMenu(HandleServeVisitorMenuChoice, dishes, visitor); 
        Console.ReadKey();
        Console.Clear();
    }
    
    /// <summary>
    /// Отображает рецепты, доступные для изучения.
    /// </summary>
    /// <returns> Имеются ли блюда для изучения </returns>
    private static bool ShowAvailableDishesToLearn()
    {
        Console.Clear();
        Console.WriteLine("\nДоступные блюда для изучения: \n");
        var dishes = DishCatalog.Dishes.Where(x => x.RequiredTavernLevel <= _tavern.Level && !_tavern.AvailableDishes.Contains(x) && x.Id.StartsWith("base_")).ToList();
        if (dishes.Count == 0)
        {
            Console.WriteLine("Доступных блюд для изучения нет!");
            InputValidator.Continue();
            return false;
        }
        Console.WriteLine(new string('-', 79));
        Console.WriteLine($"|  №  |  {"Блюдо",-30}| {"Необходимые продукты для изучения",36} |");
        Console.WriteLine(new string('-', 79));
        var index = 1;
        foreach (var item in dishes)
        {
            Console.WriteLine($"| {index,2}  | {item.Name,-43}                           |");
            foreach (var ingredient in item.Ingredients)
            {
                var product = ProductCatalog.Products.First(x => x.Name == ingredient.Key).Name;
                Console.WriteLine($"|     | {product,61} - 1 шт. |");
            }
            Console.WriteLine(new string('-', 79));
            index++;
        }
        return true;
    }
    
    /// <summary>
    /// Показывает необходимые ингридиенты для списка блюд и количество необходимых ингридиентов в таверне.
    /// </summary>
    /// <param name="dishes">Список блюд, ингредиенты которых необходимо отобразить.</param>
    private static void ShowDishesIngridients(this Tavern tavern, List<Dish> dishes)
    {
        Console.Clear();
        foreach (var dish in dishes)
        {
            ShowDishIngridients(dish);
        }
    }
    
    /// <summary>
    /// Показывает необходимые ингридиенты для блюда и количество необходимых ингридиентов в таверне.
    /// </summary>
    /// <param name="dish">Блюдо, ингредиенты которых необходимо отобразить.</param>
    public static void ShowDishIngridients(Dish dish)
    {
        if (!_tavern.AvailableDishes.Contains(dish))
        {
            Console.WriteLine($"Вы не знаете рецепта для блюда {dish.Name}");
            return;
        }
        Console.WriteLine($"\nИнгридиенты для блюда {dish.Name}:\n");
        var counter = 1;
        foreach (var ingrid in dish.Ingredients)
        { 
            var product = _tavern.Products.Where(x => x.Key == ingrid.Key).Select(x =>  x.Value).FirstOrDefault();
            Console.WriteLine($"{counter++}. {ingrid.Key, -20} -{ingrid.Value,3} шт.     (В наличии {product,2} шт.)");
        }
        Console.WriteLine();
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
        InputValidator.Continue();
    }
    
    /// <summary>
    /// Отображает список продуктов, хранящихся на складе таверны.
    /// </summary>
    private static void CheckProductAvailability()
    {
        GameOutput.ShowTheFoodStorage(_tavern);
    }
    
    /// <summary>
    /// Отображает список продуктов, которые сегодня лежат на прилавке магазина.
    /// </summary>
    /// <returns> Имеются ли продукты в магазине для покупки </returns>
    private static bool ShowTheShop()
    {
        if (_availableProductsInShopToday.Count == 0)
        {
            Console.WriteLine("\nМагазин пуст!");
            InputValidator.Continue();
            return false;
        }
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
        return true;
    }
    
    /// <summary>
    /// Запускает игровой процесс и управляет основным циклом игры.
    /// </summary>
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
    /// Выводит в консоль статистику за день.
    /// </summary>
    private static void ViewDailyStatistics()
    {
        _tavern.Day++;
        ShowHeader();
        Console.WriteLine($"День {_tavern.Day} завершён!");
        Console.WriteLine();
        Console.WriteLine($"Обслужено посетителей: {_completedOrdersToday}");
        Console.WriteLine($"Заработано золотых: {_moneyToday}");
        Console.WriteLine($"Получено опыта: {_experienceToday}");
        InputValidator.Continue();
    }
}