using System;
using TavernSimulator.Game;
using TavernSimulator.Menus;
using TavernSimulator.Models;
using TavernSimulator.Service;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        for (int i = 0; i < 100; i++)
        {
            Console.Write(new Random().Next(0, 5));
        }
        Game.Start();
    }
}