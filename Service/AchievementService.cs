using System;
using System.Linq;
using TavernSimulator.Data;
using TavernSimulator.Enums;
using TavernSimulator.Models;

namespace TavernSimulator.Service;

/// <summary>
/// Содержит логику работы с достижениями.
/// </summary>
public static class AchievementService
{
   
    /// <summary>
    /// Проверяет, выполнено ли условие разблокировки достижения.
    /// </summary>
    /// <param name="type">Тип условия достижения.</param>
    /// <param name="tavern">Таверна, для которой проверяется условие.</param>
    public static void IsAchievementUnlocked(AchievementRequirementType type, Tavern tavern)
    {
        var achievements = AchievementCatalog.Achievements.Where(x => x.RequirementType == type);
        foreach (var achievement in achievements)
        {
            if (!achievement.IsUnlocked && tavern.TotalDishesCooked >= achievement.RequiredValue) Unlock(achievement,tavern);
        }
    }
    
    

    /// <summary>
    /// Разблокирует достижение.
    /// </summary>
    /// <param name="achievement">Достижение для разблокировки.</param>
    /// <param name="tavern">Таверна, для которой открывается достижение.</param>
    private static void Unlock(Achievement achievement, Tavern tavern)
    {
        achievement.IsUnlocked = true;
        if(achievement.RewardType == AchievementRewardType.Experience) tavern.Experience += achievement.RewardValue;
        if(achievement.RewardType == AchievementRewardType.Gold) tavern.Gold += achievement.RewardValue;
        Console.WriteLine($"\nДостижение разблокировано! \n{achievement.UnlockMessage}");
    }
}