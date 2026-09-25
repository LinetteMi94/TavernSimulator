using System;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Models;
using TavernSimulator.Models.Visitors;

namespace TavernSimulator.Service;

public static class VisitorService
{
    public static Visitor CreateVisitor()
    {
        var choice = Random.Shared.Next(100);
        Visitor visitor = choice switch
        {
            < 60 => new Peasant(),
            _ => new Woodcutter()
        };
        visitor.Name = visitor.PossibleNames[new Random().Next(0, visitor.PossibleNames.Count)];
        return visitor;
    }
    
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