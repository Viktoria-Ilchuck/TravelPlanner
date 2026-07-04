using Spectre.Console;

namespace TravelPlanner.Menus;

public class MainMenu
{
    public int Show()
    {
        AnsiConsole.Clear();
        
        AnsiConsole.Write(
            new FigletText("Travel Planner")
                .Centered()
                .Color(Color.DeepSkyBlue1));
        
        AnsiConsole.MarkupLine("[grey]════════════════════════════════════════════════════════════[/]");
        AnsiConsole.MarkupLine("[yellow]        Планування подорожей та витрат[/]");
        AnsiConsole.MarkupLine($"[grey]        Дата: {DateTime.Now:dd.MM.yyyy}     Час: {DateTime.Now:HH:mm}[/]");
        AnsiConsole.MarkupLine("[grey]════════════════════════════════════════════════════════════[/]");
        AnsiConsole.WriteLine();

        return AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("[green]Оберіть дію:[/]")
                .PageSize(10)
                .AddChoices(1, 2, 0)
                .UseConverter(x => x switch
                {
                    1 => "🔑 Вхід",
                    2 => "📝 Реєстрація",
                    0 => "🚪 Вихід",
                    _ => ""
                }));
    }
}