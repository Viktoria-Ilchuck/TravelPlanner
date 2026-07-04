using Spectre.Console;
using TravelPlanner.Services;

namespace TravelPlanner.Menus;

public class MyTripsMenu
{
    private readonly TripService _tripService;
    private readonly CurrentUserService _currentUser;

    public MyTripsMenu(
        TripService tripService,
        CurrentUserService currentUser)
    {
        _tripService = tripService;
        _currentUser = currentUser;
    }

    public async Task ShowAsync()
    {
        AnsiConsole.Clear();

        var trips = await _tripService.GetDetailedTripsAsync(
            _currentUser.CurrentUser!.Id);

        if (!trips.Any())
        {
            AnsiConsole.MarkupLine("[yellow]У вас ще немає подорожей.[/]");

            Console.ReadKey();
            return;
        }

        var table = new Table();

        table.Border(TableBorder.Rounded);

        table.AddColumn("№");
        table.AddColumn("Назва");
        table.AddColumn("Місто");
        table.AddColumn("Початок");
        table.AddColumn("Кінець");
        table.AddColumn("Бюджет");
        table.AddColumn("Статус");

        foreach (var trip in trips)
        {
            table.AddRow(
                trip.Id.ToString(),
                trip.Title,
                trip.City,
                trip.StartDate.ToString("dd.MM.yyyy"),
                trip.EndDate.ToString("dd.MM.yyyy"),
                $"{trip.Budget:N0} грн",
                trip.Status);
        }

        AnsiConsole.Write(table);

        Console.ReadKey();
    }
}