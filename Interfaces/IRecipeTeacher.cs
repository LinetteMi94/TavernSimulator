using TavernSimulator.Models;

namespace TavernSimulator.Models;

public interface IRecipeTeacher
{
    bool HasRecipes { get; set; }
    
    List<Dish> SecretRecipes { get; set; }

    public Dish TrySetRecipeToTeach(Dish? dish, Tavern tavern)
    {
        if (!HasRecipes) return dish; 
        while(HasRecipes)
        {
            dish = SecretRecipes.Where(x => x.RequiredTavernLevel <= tavern.Level).OrderBy(_ => Random.Shared.Next())
                .FirstOrDefault();
            if (tavern.AvailableDishes.Any(x => x.Name == dish?.Name))
            {
                SecretRecipes.Remove(dish);
                if (SecretRecipes.Count == 0)
                {
                    HasRecipes = false;
                }
            }
            else break;
        }
        return dish;
    }

    public void ShareRecipe(Tavern tavern);
}