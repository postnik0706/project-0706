using System;

static class Utilities
{
    static public void ShowInColor(string Message, ConsoleColor? Color = null)
    {
        ConsoleColor norm = Console.ForegroundColor;
        if (Color.HasValue)
            Console.ForegroundColor = Color.Value;
        Console.WriteLine(Message);
        Console.ForegroundColor = norm;
    }
}
