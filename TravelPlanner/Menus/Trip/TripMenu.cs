using Spectre.Console;

namespace TravelPlanner.Menus;

public class TripMenu
{
    public int Show()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("Trips")
                .Centered()
                .Color(Color.DeepSkyBlue1));

        return AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("Подорожі")
                .PageSize(10)
                .AddChoices(1,2,3,4,0)
                .UseConverter(x => x switch
                {
                    1 => "➕ Створити подорож",
                    2 => "📋 Мої подорожі",
                    3 => "✏ Редагувати",
                    4 => "🗑 Видалити",
                    0 => "⬅ Назад",
                    _ => ""
                }));
    }
}