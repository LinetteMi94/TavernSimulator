using System;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Models;

namespace TavernSimulator.Service;

public static class VisitorService
{
    public static void CreateVisitor(this Visitor visitor)
    {
        visitor.Name = visitor.PossibleNames[new Random().Next(0, visitor.PossibleNames.Count)];
        visitor.Money = new Random().Next(15,30);
        visitor.PreferredDishes = DishCatalog.Dishes.Where(x => x.Price <= 15).ToList();
    }
    
    public static Dish ChooseDish(this Visitor visitor, Tavern tavern)
    {
        var dishes = visitor.PreferredDishes.Where(x => x.RequiredTavernLevel<= tavern.Level && x.Price<=visitor.Money).ToList();
        return dishes[new Random().Next(0, dishes.Count)];
    }
}