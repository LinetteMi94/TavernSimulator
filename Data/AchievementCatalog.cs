using TavernSimulator.Enums;
using TavernSimulator.Models;

namespace TavernSimulator.Data;

/// <summary>
/// Содержит каталог достижений, доступных в игре.
/// </summary>
public static class AchievementCatalog
{
    public static List<Achievement> Achievements { get; } =
    [
        new() {
            Id = "first-dish",
            Name = "Первое блюдо",
            Description = "Приготовьте своё первое блюдо.",
            UnlockMessage = "🏆 Вы приготовили своё первое блюдо! Таверна начинает оживать.",
            RewardType = AchievementRewardType.Gold,
            RewardValue = 10,
            RequirementType = AchievementRequirementType.DishesCooked,
            RequiredValue = 1 },

        new() {
            Id = "ten-dishes",
            Name = "Поварёнок",
            Description = "Приготовьте 10 блюд.",
            UnlockMessage = "🏆 Вы приготовили уже 10 блюд!",
            RewardType = AchievementRewardType.Experience,
            RewardValue = 25,
            RequirementType = AchievementRequirementType.DishesCooked,
            RequiredValue = 10 },
        new() {
            Id = "beginner-cook",
            Name = "Начинающий повар",
            Description = "Приготовьте 25 блюд.",
            UnlockMessage = "🏆 25 блюд! Вы уже явно не новичок у плиты.",
            RewardType = AchievementRewardType.Gold,
            RewardValue = 25,
            RequirementType = AchievementRequirementType.DishesCooked,
            RequiredValue = 25
        },
        new() {
            Id = "skilled-cook",
            Name = "Умелый повар",
            Description = "Приготовьте 50 блюд.",
            UnlockMessage = "🏆 50 блюд приготовлено! На кухне вас уже начинают уважать.",
            RewardType = AchievementRewardType.Experience,
            RewardValue = 50,
            RequirementType = AchievementRequirementType.DishesCooked,
            RequiredValue = 50 },
        new() {
            Id = "busy-kitchen",
            Name = "Кухня работает",
            Description = "Приготовьте 75 блюд.",
            UnlockMessage = "🏆 75 блюд! Похоже, ваша кухня никогда не пустует.",
            RewardType = AchievementRewardType.Gold,
            RewardValue = 50,
            RequirementType = AchievementRequirementType.DishesCooked,
            RequiredValue = 75 },
        new() {
            Id = "master-of-kitchen",
            Name = "Мастер кухни",
            Description = "Приготовьте 100 блюд.",
            UnlockMessage = "🏆 100 блюд! Теперь вас действительно можно назвать мастером кухни.",
            RewardType = AchievementRewardType.Experience,
            RewardValue = 100,
            RequirementType = AchievementRequirementType.DishesCooked,
            RequiredValue = 100 },
        new() {
            Id = "skilled-chef",
            Name = "Искусный повар",
            Description = "Приготовьте 150 блюд.",
            UnlockMessage = "🏆 150 блюд! Ваша кухня заслужила отличную репутацию.",
            RewardType = AchievementRewardType.Gold,
            RewardValue = 100,
            RequirementType = AchievementRequirementType.DishesCooked,
            RequiredValue = 150 },
        new() {
            Id = "legend-of-kitchen",
            Name = "Легенда кухни",
            Description = "Приготовьте 200 блюд.",
            UnlockMessage = "🏆 200 блюд! Ваши кулинарные способности уже становятся легендой.",
            RewardType = AchievementRewardType.Experience,
            RewardValue = 200,
            RequirementType = AchievementRequirementType.DishesCooked,
            RequiredValue = 200}
    ];
}