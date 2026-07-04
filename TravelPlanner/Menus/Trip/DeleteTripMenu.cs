using Spectre.Console;
using TravelPlanner.Models;
using TravelPlanner.Services;

namespace TravelPlanner.Menus;

public class DeleteTripMenu
{
    private readonly TripService _tripService;
    private readonly CurrentUserService _currentUser;

    public DeleteTripMenu(
        TripService tripService,
        CurrentUserService currentUser)
    {
        _tripService = tripService;
        _currentUser = currentUser;
    }

    public async Task ShowAsync()
    {
        AnsiConsole.Clear();

        var trips = await _tripService.GetUserTripsAsync(
            _currentUser.CurrentUser!.Id);

        if (!trips.Any())
        {
            AnsiConsole.MarkupLine("[yellow]Немає подорожей для видалення.[/]");
            Console.ReadKey();
            return;
        }

        var trip = AnsiConsole.Prompt(
            new SelectionPrompt<Trip>()
                .Title("Оберіть подорож для видалення")
                .UseConverter(x => $"{x.Title} ({x.StartDate:dd.MM.yyyy})")
                .AddChoices(trips));

        var confirm = AnsiConsole.Confirm(
            $"Видалити подорож [red]{trip.Title}[/]?");

        if (!confirm)
            return;

        await _tripService.DeleteTripAsync(trip.Id);

        AnsiConsole.MarkupLine("[green]✓ Подорож видалено.[/]");
        Console.ReadKey();
    }
}