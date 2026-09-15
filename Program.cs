using System;
using TavernSimulator.Menus;
using TavernSimulator.Models;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Tavern tavern = new ();
        tavern.CreateTavern();
        MainMenu.ShowMorningMenu(tavern);
        
    }
}