using TavernSimulator.Enums;
using TavernSimulator.Models;

namespace TavernSimulator.Data;

/// <summary>
/// Описывает условия появления посетителя и способ его создания.
/// </summary>
public class VisitorInfo(int minReputation, int maxReputation, VisitorTypes type, Func<Visitor> createVisitor)
{
    public int MinReputation { get; set; } = minReputation;
    public int MaxReputation { get; set; } = maxReputation;
    public VisitorTypes Type { get; set; } = type;
    public Func<Visitor> Create { get; set; } = createVisitor;
}