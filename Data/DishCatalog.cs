using TavernSimulator.Enums;
using TavernSimulator.Models;

namespace TavernSimulator.Data;

/// <summary>
/// Содержит все блюда, доступные в игре.
/// </summary>
public static class DishCatalog
{
    public static List<Dish> Dishes { get; set; } = 
        [
            new ("Суп из корнеплодов", 16)
            {
                Ingredients =  new Dictionary<ProductName, int>
                {
                    [ProductName.Картофель] = 2, 
                    [ProductName.Морковь] = 1,
                    [ProductName.Лук] = 1,
                    [ProductName.Травы] = 1
                }
            },
            new ("Овощное рагу", 14)
            {
                Ingredients =  new Dictionary<ProductName, int>
                {
                    [ProductName.Картофель] = 1, 
                    [ProductName.Морковь] = 2,
                    [ProductName.Лук] = 1
                }
            },
            new ("Травяной чай", 14)
            {
                Ingredients =  new Dictionary<ProductName, int>
                {
                    [ProductName.Мёд] = 1, 
                    [ProductName.Травы] = 2
                }
            },
            new ("Яичница", 13)
            {
                Ingredients =  new Dictionary<ProductName, int>
                {
                    [ProductName.Лук] = 1, 
                    [ProductName.Яйца] = 2
                }
            },
            new ("Яблочный пирог", 39)
            {
                Ingredients =  new Dictionary<ProductName, int>
                {
                    [ProductName.Яблоки] = 3, 
                    [ProductName.Мука] = 2,
                    [ProductName.Яйца] = 1,
                    [ProductName.Мёд] = 1
                }
            },
            new ("Медовые яблоки", 25)
            {
                Ingredients =  new Dictionary<ProductName, int>
                {
                    [ProductName.Яблоки] = 2, 
                    [ProductName.Мёд] = 2
                }
            },
            new ("Медовуха", 18)
            {
                Ingredients =  new Dictionary<ProductName, int> { [ProductName.Мёд] = 2 }
            }
        ];
}