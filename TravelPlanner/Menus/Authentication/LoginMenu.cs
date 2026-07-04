using Spectre.Console;
using TravelPlanner.Models;
using TravelPlanner.Services;
using TravelPlanner.UI;

namespace TravelPlanner.Menus;

public class LoginMenu
{
    private readonly AuthenticationService _authenticationService;

    public LoginMenu(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<User?> ShowAsync()
    {
        Console.Clear();

        AnsiConsole.Write(
            new FigletText("Login")
                .Centered()
                .Color(Color.Green));

        Console.WriteLine();

        var login = AnsiConsole.Ask<string>("Логін:");

        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Пароль:")
                .Secret());

        var user = await _authenticationService.LoginAsync(login, password);

        if (user == null)
        {
            ConsoleUI.Error("Невірний логін або пароль.");
            ConsoleUI.Wait();
            return null;
        }

        ConsoleUI.Success($"Вітаємо, {user.FirstName}!");
        ConsoleUI.Wait();

        return user;
    }
}