using Spectre.Console;

namespace TravelPlanner.UI;

public static class ConsoleUI
{
    public static void Success(string message)
    {
        AnsiConsole.MarkupLine($"[green]✔ {message}[/]");
    }

    public static void Error(string message)
    {
        AnsiConsole.MarkupLine($"[red]✘ {message}[/]");
    }

    public static void Warning(string message)
    {
        AnsiConsole.MarkupLine($"[yellow]⚠ {message}[/]");
    }

    public static void Info(string message)
    {
        AnsiConsole.MarkupLine($"[deepskyblue1]{message}[/]");
    }

    public static void Wait()
    {
        AnsiConsole.MarkupLine("\n[grey]Натисніть будь-яку клавішу...[/]");
        Console.ReadKey(true);
    }
}