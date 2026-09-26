using System;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Models;
using TavernSimulator.Models.Visitors;

namespace TavernSimulator.Service;

/// <summary>
/// Содержит логику создания посетителей и формирования их заказов.
/// </summary>
public static class VisitorService
{
    /// <summary>
    /// Создаёт нового посетителя случайного типа.
    /// </summary>
    public static Visitor CreateVisitor()
    {
        var choice = Random.Shared.Next(100);
        Visitor visitor = choice switch
        {
            < 30 => new Peasant(),
            < 60 => new Herbalist(),
            _ => new Woodcutter()
        };
        visitor.Name = visitor.PossibleNames[new Random().Next(0, visitor.PossibleNames.Count)];
        return visitor;
    }
    
    /// <summary>
    /// Формирует заказ посетителя с учётом его предпочтений и доступных блюд.
    /// </summary>
    /// <param name="visitor">Посетитель, для которого формируется заказ.</param>
    /// <param name="tavern">Таверна, в которой посетитель делает заказ.</param>
    /// <returns>Список блюд, выбранных посетителем.</returns>
    public static List<Dish> ChooseOrder(this Visitor visitor, Tavern tavern)
    {
        int count = new Random().Next(1, 4);
        int money = visitor.Money;
        var dishes = visitor.PreferredDishes.Where(x => x.RequiredTavernLevel<= tavern.Level && x.Price<=money).ToList();
        for (int i = 0; i < count; i++)
        {
           dishes = dishes.Where(x => x.Price<=money).ToList();
           if (dishes.Count == 0) break;
           var dish = dishes[new Random().Next(0, dishes.Count)];
           visitor.Order.Add(dish);
           money -= dish.Price;
           dishes.Remove(dish);
        }
        return visitor.Order;
    }
}