using System;

namespace TavernSimulator.Input;

/// <summary>
/// Содержит методы для проверки и обработки пользовательского ввода.
/// </summary>
public static class InputValidator
{
    
    /// <summary>
    /// Проверяет введённое значение и возвращает число в допустимом диапазоне.
    /// </summary>
    /// <param name="text">Строка, введённая пользователем.</param>
    /// <param name="max">Максимально допустимое значение.</param>
    /// <param name="min">Минимально допустимое значение.</param>
    /// <returns>Корректное числовое значение.</returns>
    public static int GetValidInput(int max, int min = 1)
    {
        while (true)
        {
            string? input = Console.ReadLine();

            if (int.TryParse(input, out var choice) && (choice >= min && choice <= max))
            {
                return choice;
            }
            Console.WriteLine($"Пожалуйста, введите число от {min} до {max}!");
        }
    }

    public static void Continue()
    {
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}