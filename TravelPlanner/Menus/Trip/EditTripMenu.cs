using Spectre.Console;
using TravelPlanner.Models;
using TravelPlanner.Services;

namespace TravelPlanner.Menus;

public class EditTripMenu
{
    private readonly TripService _tripService;
    private readonly CurrentUserService _currentUser;
    private readonly CountryService _countryService;
    private readonly CityService _cityService;

    public EditTripMenu(
        TripService tripService,
        CurrentUserService currentUser,
        CountryService countryService,
        CityService cityService)
    {
        _tripService = tripService;
        _currentUser = currentUser;
        _countryService = countryService;
        _cityService = cityService;
    }

    public async Task ShowAsync()
    {
        AnsiConsole.Clear();

        var trips = await _tripService.GetUserTripsAsync(
            _currentUser.CurrentUser!.Id);

        if (!trips.Any())
        {
            AnsiConsole.MarkupLine("[yellow]У вас немає подорожей.[/]");
            Console.ReadKey();
            return;
        }

        var trip = AnsiConsole.Prompt(
            new SelectionPrompt<Trip>()
                .Title("Оберіть подорож")
                .UseConverter(x => $"{x.Title} ({x.StartDate:dd.MM.yyyy})")
                .AddChoices(trips));

        trip.Title = AnsiConsole.Ask(
            $"Назва ({trip.Title})",
            trip.Title);

        trip.Description = AnsiConsole.Ask(
            $"Опис ({trip.Description})",
            trip.Description);

        var countryName =
            AnsiConsole.Ask<string>("Нова країна:");

        var cityName =
            AnsiConsole.Ask<string>("Нове місто:");

        int countryId =
            await _countryService.GetOrCreateCountryAsync(countryName);

        int cityId =
            await _cityService.GetOrCreateCityAsync(
                cityName,
                countryId);

        trip.CityId = cityId;

        trip.Budget = AnsiConsole.Ask(
            $"Бюджет ({trip.Budget})",
            trip.Budget);

        trip.StartDate = AnsiConsole.Ask(
            $"Дата початку ({trip.StartDate:yyyy-MM-dd})",
            trip.StartDate);

        trip.EndDate = AnsiConsole.Ask(
            $"Дата завершення ({trip.EndDate:yyyy-MM-dd})",
            trip.EndDate);

        if (trip.EndDate < trip.StartDate)
        {
            AnsiConsole.MarkupLine("[red]Дата завершення не може бути раніше дати початку.[/]");
            Console.ReadKey();
            return;
        }

        await _tripService.UpdateTripAsync(trip);

        AnsiConsole.MarkupLine("[green]✓ Подорож успішно оновлена.[/]");

        Console.ReadKey();
    }
}