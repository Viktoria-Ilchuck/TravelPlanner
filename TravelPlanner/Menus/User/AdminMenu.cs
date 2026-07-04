using Spectre.Console;

namespace TravelPlanner.Menus;

public class AdminMenu
{
    public int Show()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("Travel Planner")
                .Centered()
                .Color(Color.DeepSkyBlue1));

        AnsiConsole.MarkupLine("[grey]════════════════════════════════════════════════════════════[/]");
        AnsiConsole.MarkupLine("[red]                 Панель адміністратора[/]");
        AnsiConsole.MarkupLine($"[grey]Дата: {DateTime.Now:dd.MM.yyyy}      Час: {DateTime.Now:HH:mm}[/]");
        AnsiConsole.MarkupLine("[grey]════════════════════════════════════════════════════════════[/]");
        AnsiConsole.WriteLine();

        return AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("[green]Оберіть розділ:[/]")
                .PageSize(15)
                .AddChoices(1,2,3,4,5,6,7,0)
                .UseConverter(x => x switch
                {
                    1 => "👥 Користувачі",
                    2 => "🌍 Країни",
                    3 => "🏙 Міста",
                    4 => "🏨 Готелі",
                    5 => "📂 Категорії витрат",
                    6 => "✈ Подорожі",
                    7 => "📄 Звіти",
                    0 => "🚪 Вийти",
                    _ => ""
                }));
    }
}