using TavernSimulator.Enums;
using TavernSimulator.Models;

namespace TavernSimulator.Data;

/// <summary>
/// Содержит все продукты, доступные в игре.
/// </summary>
public class ProductCatalog
{
    public List<Product> Products { get; set; } = 
        [
            new (ProductName.Картофель, ProductType.Овощ,2),
            new (ProductName.Морковь, ProductType.Овощ, 2),
            new (ProductName.Лук, ProductType.Овощ, 2),
            new (ProductName.Курица, ProductType.Мясо,6),
            new (ProductName.Яйца, ProductType.Другое,3),
            new (ProductName.Мука, ProductType.Другое,3),
            new (ProductName.Яблоки,ProductType.Фрукт, 3),
            new (ProductName.Мёд, ProductType.Сладости,5),
            new (ProductName.Молоко, ProductType.МолочныеПродукты,3),
            new (ProductName.Травы, ProductType.ТравыИСпеции,2),
            new (ProductName.Чеснок, ProductType.Овощ, 3),
            new (ProductName.Капуста, ProductType.Овощ, 3),
            new (ProductName.Тыква, ProductType.Овощ, 5),
            new (ProductName.Груша, ProductType.Фрукт, 4),
            new (ProductName.Вишня, ProductType.Ягода, 5),
            new (ProductName.Клубника, ProductType.Ягода, 5),
            new (ProductName.Гриб, ProductType.Другое, 5),
            new (ProductName.Гречка, ProductType.Крупы, 4),
            new (ProductName.Рис, ProductType.Крупы, 5),
            new (ProductName.Свинина, ProductType.Мясо,7),
            new (ProductName.Говядина, ProductType.Мясо,8),
            new (ProductName.Гусь, ProductType.Мясо,12),
            new (ProductName.Форель, ProductType.Рыба,9),
            new (ProductName.Сливки, ProductType.МолочныеПродукты,5),
            new (ProductName.Масло, ProductType.МолочныеПродукты,5),
            new (ProductName.Сыр, ProductType.МолочныеПродукты,8),
            new (ProductName.Сметана, ProductType.МолочныеПродукты,4),
            new (ProductName.Укроп, ProductType.ТравыИСпеции,2),
            new (ProductName.Мята, ProductType.ТравыИСпеции,3),
            new (ProductName.Сахар, ProductType.Сладости,4),
            new (ProductName.Варенье, ProductType.Сладости,6),
            new (ProductName.ЧёрныйПерец, ProductType.ТравыИСпеции,4),
            new (ProductName.Петрушка, ProductType.ТравыИСпеции,2)
        ];
}