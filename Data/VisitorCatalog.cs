using TavernSimulator.Enums;
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
            new(-1,-1,VisitorTypes.Spring,() => new Dryad()),
            new (0,100,VisitorTypes.None, () => new Peasant()),
            new (0,100,VisitorTypes.None,() => new Nun()),
            new (10,110,VisitorTypes.None, () => new Herbalist()),
            new (10,110,VisitorTypes.None, () => new Nanny()),
            new (20,120,VisitorTypes.None, () => new Baker()),
            new (20,120,VisitorTypes.None, () => new Woodcutter()),
            new (30,130,VisitorTypes.None, () => new Merchant())
        ];
}