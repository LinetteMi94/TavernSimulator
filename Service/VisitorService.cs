using System;
using System.Collections.Generic;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Menus;
using TavernSimulator.Models;
using TavernSimulator.Models.Visitors;

namespace TavernSimulator.Service;

/// <summary>
/// Содержит логику создания посетителей и формирования их заказов.
/// </summary>
public static class VisitorService
{
    /// <summary>
    /// Формирует заказ посетителя с учётом его предпочтений и доступных блюд.
    /// </summary>
    /// <param name="visitor">Посетитель, для которого формируется заказ.</param>
    /// <param name="tavern">Таверна, в которой посетитель делает заказ.</param>
    /// <returns>Список блюд, выбранных посетителем.</returns>
    public static List<Dish> ChooseOrder(this Visitor visitor, Tavern tavern, Season season, DayEvent dayEvent)
    {
        int count = new Random().Next(2, 4);
        int money = visitor.Money;
        List<Dish> dishes = [];
        if (dayEvent == DayEvent.PumpkinDay)
        {
            dishes = tavern.AvailableDishes
                .Where(x => x.Price<=money && x.Ingredients.ContainsKey("Тыква")).ToList();
        }

        if (dayEvent == DayEvent.HeatwaveDay)
        {
            dishes = tavern.AvailableDishes
                .Where(x => x.Price<=money && x.Type == DishTypes.Напиток).ToList();
        }

        if (dayEvent == DayEvent.None)
        {
            dishes = visitor.PreferredDishes
                .Where(x => x.RequiredTavernLevel <= tavern.Level && x.Price <= money &&
                            (x.Id.StartsWith("base_") || x.Id.StartsWith(season.ToString().ToLower()) ||
                             (!x.Id.StartsWith("spring_") && !x.Id.StartsWith("summer_") &&
                              !x.Id.StartsWith("autumn_") && !x.Id.StartsWith("winter_") &&
                              tavern.AvailableDishes.Contains(x)))).ToList();
        }

        for (int i = 0; i < count; i++)
        {
            dishes = dishes.Where(x => x.Price<=money).ToList();
            if (dishes.Count == 0) break;
            var dish = dishes[new Random().Next(0, dishes.Count)];
            dishes.Remove(dish);
            if(visitor.Order.Any(x => x.Type == DishTypes.Напиток) && dish.Type == DishTypes.Напиток) continue;
            visitor.Order.Add(dish);
            money -= dish.Price;
        }
        return visitor.Order;
    }
    
    /// <summary>
    /// Создаёт нового посетителя случайного типа.
    /// </summary>
    public static Visitor CreateVisitor(Tavern tavern)
    {
        var visitors = VisitorCatalog.Visitors
            .Where(x => x.MinReputation <= tavern.Reputation && x.MaxReputation>= tavern.Reputation).ToList();
        var visitor = visitors[new Random().Next(visitors.Count)].Create();
        visitor.Name = visitor.PossibleNames[new Random().Next(0, visitor.PossibleNames.Count)];
        return visitor;
    }
    
    /// <summary>
    /// Случайно выбирает предмет, который посетитель может оставить в таверне.
    /// </summary>
    public static Item? LeaveRandomItem(this Visitor visitor)
    {
        if (!ShouldTriggerEvent(15)) return null;
        var item = visitor.AvaliableItemsForLeave[new Random().Next(0, visitor.AvaliableItemsForLeave.Count)];
        item.OwnerName = visitor.Name;
        item.OwnerType = visitor.TypeName;
        return item;
    }
    
    public static void TriggerEvent(this Visitor visitor, Tavern tavern)
    {
        if (ShouldTriggerEvent(20)) visitor.OnEvent(tavern);
    }
    
    /// <summary>
    /// Определяет, произойдёт ли случайное событие на основе заданной вероятности.
    /// </summary>
    /// <param name="chance">Вероятность наступления события в процентах.</param>
    /// <returns>True, если событие должно произойти.</returns>
    private static bool ShouldTriggerEvent(int chance)
    {
        return Random.Shared.Next(100) < chance;
    }
}