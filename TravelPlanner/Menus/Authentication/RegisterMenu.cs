using Spectre.Console;
using TravelPlanner.Services;
using TravelPlanner.UI;

namespace TravelPlanner.Menus;

public class RegisterMenu
{
    private readonly AuthenticationService _authenticationService;

    public RegisterMenu(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task ShowAsync()
    {
        Console.Clear();

        AnsiConsole.Write(
            new FigletText("Registration")
                .Centered()
                .Color(Color.Green));

        Console.WriteLine();

        var firstName = AnsiConsole.Ask<string>("Ім'я:");

        var lastName = AnsiConsole.Ask<string>("Прізвище:");

        var email = AnsiConsole.Ask<string>("Email:");

        var login = AnsiConsole.Ask<string>("Логін:");

        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Пароль:")
                .Secret());

        var repeatPassword = AnsiConsole.Prompt(
            new TextPrompt<string>("Повторіть пароль:")
                .Secret());

        if (password != repeatPassword)
        {
            ConsoleUI.Error("Паролі не співпадають.");
            ConsoleUI.Wait();
            return;
        }

        var error = await _authenticationService.RegisterAsync(
            firstName,
            lastName,
            email,
            login,
            password);

        if (error != null)
        {
            ConsoleUI.Error(error);
        }
        else
        {
            ConsoleUI.Success("Реєстрація успішна!");
        }

        ConsoleUI.Wait();
    }
}