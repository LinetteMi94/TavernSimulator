using TavernSimulator.Models;

namespace TavernSimulator.Data;

/// <summary>
/// Описывает условия появления посетителя и способ его создания.
/// </summary>
public class VisitorInfo(int minReputation, int maxReputation, Func<Visitor> createVisitor)
{
    public int MinReputation { get; set; } = minReputation;
    public int MaxReputation { get; set; } = maxReputation;
    public Func<Visitor> Create { get; set; } = createVisitor;
}