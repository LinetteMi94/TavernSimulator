using TavernSimulator.Models.Visitors;
using TavernSimulator.Models.Visitors.Seasonal;

namespace TavernSimulator.Data;

/// <summary>
/// Содержит каталог доступных для генерации посетителей.
/// </summary>
public static class VisitorCatalog
{
    public static readonly List<VisitorInfo> Visitors = 
        [
            new(0,1000,() => new Dryad()),
            new (0,100, () => new Peasant()),
            new (0,100, () => new Nun()),
            new (10,110, () => new Herbalist()),
            new (10,110, () => new Nanny()),
            new (20,120, () => new Baker()),
            new (20,120, () => new Woodcutter()),
            new (30,130, () => new Merchant())
        ];
}