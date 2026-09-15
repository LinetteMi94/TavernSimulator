using TavernSimulator.Enums;

namespace TavernSimulator.Models;

/// <summary>
/// Представляет таверну, хранит её состояние, запасы продуктов,
/// доступные блюда, золото, репутацию и информацию о посетителях.
/// </summary>
public class Tavern
{
    public const string Name = "Гусь и пирог";
    public Dictionary<Product, int>? Products { get; set; }
    public List<Dish>? AvailableDishes { get; set; } = new();
    public int Day { get; set; } = 1;
    public int Customers { get; set; }
    public int Gold { get; set; } = 100;
    public int Reputation { get; set; } = 1;

    public void CreateTavern()
    {
        var potato = new Product(ProductName.Картофель, 2);
        var carrot = new Product(ProductName.Морковь, 2);
        var onion = new Product(ProductName.Лук, 2);
        var meat = new Product(ProductName.Мясо, 6);
        var eggs = new Product(ProductName.Яйца, 3);
        var floor = new Product(ProductName.Мука, 3);
        var apples = new Product(ProductName.Яблоки, 3);
        var honey = new Product(ProductName.Мёд, 5);
        var milk = new Product(ProductName.Молоко, 3);
        var herbs = new Product(ProductName.Травы, 2);
        Products = new Dictionary<Product, int>
        {
            [potato] = 8,
            [carrot] = 6,
            [onion] = 6,
            [meat] = 4,
            [eggs] = 8,
            [floor] = 5,
            [apples] = 8,
            [honey] = 3,
            [milk] = 5,
            [herbs] = 6,
        };
        
        AvailableDishes.Add(new Dish("Суп из корнеплодов", 16)
        {
            Products =  new Dictionary<Product, int>
            {
                [potato] = 2, 
                [carrot] = 1,
                [onion] = 1,
                [herbs] = 1
            }
        });
        AvailableDishes.Add(new Dish("Овощное рагу", 14)
        {
            Products =  new Dictionary<Product, int>
            {
                [potato] = 1, 
                [carrot] = 2,
                [onion] = 1
            }
        });
        AvailableDishes.Add(new Dish("Травяной чай", 14)
        {
            Products =  new Dictionary<Product, int>
            {
                [honey] = 1, 
                [herbs] = 2
            }
        });
        AvailableDishes.Add(new Dish("Яичница", 13)
        {
            Products =  new Dictionary<Product, int>
            {
                [onion] = 1, 
                [eggs] = 2
            }
        });
        AvailableDishes.Add(new Dish("Мясное рагу", 38)
        {
            Products =  new Dictionary<Product, int>
            {
                [meat] = 2, 
                [potato] = 2,
                [carrot] = 1,
                [onion] = 1
            }
        });
        AvailableDishes.Add(new Dish("Мясной пирог", 28)
        {
            Products =  new Dictionary<Product, int>
            {
                [meat] = 1, 
                [floor] = 2,
                [eggs] = 1,
                [onion] = 1
            }
        });
        AvailableDishes.Add(new Dish("Яблочный пирог", 39)
        {
            Products =  new Dictionary<Product, int>
            {
                [apples] = 3, 
                [floor] = 2,
                [eggs] = 1,
                [honey] = 1
            }
        });
        AvailableDishes.Add(new Dish("Медовые яблоки", 25)
        {
            Products =  new Dictionary<Product, int>
            {
                [apples] = 2, 
                [honey] = 2
            }
        });
        AvailableDishes.Add(new Dish("Медовуха", 18)
        {
            Products =  new Dictionary<Product, int> { [honey] = 2 }
        });
    }
}