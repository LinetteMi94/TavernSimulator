using TavernSimulator.Enums;

namespace TavernSimulator.Models;

/// <summary>
/// Представляет достижение, которое игрок может получить во время игры.
/// </summary>
public class Achievement
{
    /// <summary>
    /// Уникальный идентификатор достижения.
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// Название достижения.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание условия получения достижения.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Сообщение, отображаемое при получении достижения.
    /// </summary>
    public string UnlockMessage { get; set; }
    
    /// <summary>
    /// Тип награды за получение достижения.
    /// </summary>
    public AchievementRewardType RewardType { get; set; }
    
    /// <summary>
    /// Количество награды, выдаваемой за достижение.
    /// </summary>
    public int RewardValue { get; set; }
    
    /// <summary>
    /// Тип условия, необходимого для получения достижения.
    /// </summary>
    public AchievementRequirementType RequirementType { get; set; }
    
    /// <summary>
    /// Значение, необходимое для выполнения условия достижения.
    /// </summary>
    public int RequiredValue { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор объекта, выдаваемого в качестве награды.
    /// Используется для рецептов и продуктов.
    /// </summary>
    public string? RewardId { get; set; }
    
    /// <summary>
    /// Показывает, получено ли достижение игроком.
    /// </summary>
    public bool IsUnlocked { get; set; } = false;
}