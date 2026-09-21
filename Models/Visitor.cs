namespace TavernSimulator.Models;

public abstract class Visitor
{
    public abstract string TypeName { get; set; } 
    
    public abstract string Name { get; set; } 

    public abstract int Money { get; set; }
    
    public abstract List<string> PossibleNames { get; set; } 

    public abstract List<Dish> PreferredDishes { get; set; } 
}