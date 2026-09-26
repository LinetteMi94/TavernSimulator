using System.Collections.Generic;

namespace TavernSimulator.Models;

/// <summary>
/// Представляет базовый класс для посетителей таверны.
/// </summary>
public abstract class Visitor
{
    public abstract string TypeName { get; set; } 
    
    public string Name { get; set; } 

    public abstract int Money { get;  }
    
    public abstract List<string> PossibleNames { get; set; } 

    public abstract List<Dish> PreferredDishes { get;  }

    public List<Dish> Order { get; set; } = new();
}