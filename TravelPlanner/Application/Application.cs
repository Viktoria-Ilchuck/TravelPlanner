using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using TravelPlanner.Configuration;
using TravelPlanner.Menus;
using TravelPlanner.Services;
using TravelPlanner.UI;

namespace TravelPlanner.Application;

public class Application
{
    private readonly ServiceProvider _provider;

    public Application()
    {
        _provider = DependencyInjection.ConfigureServices();
    }

    public async Task RunAsync()
    {
        using var scope = _provider.CreateScope();

        var startup =
            scope.ServiceProvider.GetRequiredService<StartupService>();

        await startup.InitializeAsync();

        while (true)
        {
            AnsiConsole.Clear();

            var choice = ShowMainMenu();

            switch (choice)
            {
                case 1:
                {
                    var loginMenu =
                        scope.ServiceProvider.GetRequiredService<LoginMenu>();

                    var user = await loginMenu.ShowAsync();

                    if (user != null)
                    {
                        var currentUser =
                            scope.ServiceProvider.GetRequiredService<CurrentUserService>();

                        currentUser.CurrentUser = user;

                        var userMenu =
                            scope.ServiceProvider.GetRequiredService<UserMenu>();

                        await userMenu.ShowAsync();
                    }

                    break;
                }

                case 2:
                {
                    var registerMenu =
                        scope.ServiceProvider.GetRequiredService<RegisterMenu>();

                    await registerMenu.ShowAsync();
                    break;
                }

                case 0:
                    ConsoleUI.Success("До побачення!");
                    return;
            }
        }
    }

    private int ShowMainMenu()
    {
        AnsiConsole.Write(
            new FigletText("Travel Planner")
                .Centered()
                .Color(Color.DeepSkyBlue1));

        AnsiConsole.MarkupLine("[grey]════════════════════════════════════════════[/]");
        AnsiConsole.MarkupLine("[yellow]Планування подорожей та витрат[/]");
        AnsiConsole.MarkupLine($"[grey]{DateTime.Now:dd.MM.yyyy HH:mm}[/]");
        AnsiConsole.MarkupLine("[grey]════════════════════════════════════════════[/]");

        return AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("\nОберіть дію")
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