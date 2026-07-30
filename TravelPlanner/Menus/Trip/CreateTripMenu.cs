using Spectre.Console;
using TravelPlanner.Models;
using TravelPlanner.Services;
using TravelPlanner.Validators;

namespace TravelPlanner.Menus;

public class CreateTripMenu
{
    private readonly TripService _tripService;
    private readonly CurrentUserService _currentUser;
    private readonly CountryService _countryService;
    private readonly CityService _cityService;
    private readonly TripValidator _validator;

    public CreateTripMenu(
        TripService tripService,
        CurrentUserService currentUser,
        CountryService countryService,
        CityService cityService,
        TripValidator validator)
    {
        _tripService = tripService;
        _currentUser = currentUser;
        _countryService = countryService;
        _cityService = cityService;
        _validator = validator;
    }

    public async Task ShowAsync()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("New Trip")
                .Centered()
                .Color(Color.Green));

        var title = AnsiConsole.Ask<string>("Назва подорожі:");

        var description = AnsiConsole.Ask<string>("Опис:");

        var countryName = AnsiConsole.Ask<string>("Країна:");

        var cityName = AnsiConsole.Ask<string>("Місто:");

        var budget = AnsiConsole.Ask<decimal>("Бюджет:");

        var startDate = AnsiConsole.Ask<DateTime>(
            "Дата початку (yyyy-MM-dd):");

        var endDate = AnsiConsole.Ask<DateTime>(
            "Дата завершення (yyyy-MM-dd):");

        int countryId =
            await _countryService.GetOrCreateCountryAsync(countryName);

        int cityId =
            await _cityService.GetOrCreateCityAsync(
                cityName,
                countryId);

        var trip = new Trip
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            Budget = budget,
            Status = "Запланована",
            CityId = cityId,
            OwnerId = _currentUser.CurrentUser!.Id
        };

        var result = _validator.Validate(trip);

        if (!result.IsValid)
        {
            AnsiConsole.MarkupLine("[red]Помилки:[/]");

            foreach (var error in result.Errors)
            {
                AnsiConsole.MarkupLine($"[red]- {error.ErrorMessage}[/]");
            }

            Console.ReadKey();
            return;
        }

        await _tripService.CreateTripAsync(
            trip.Title,
            trip.Description,
            trip.StartDate,
            trip.EndDate,
            trip.Budget,
            trip.CityId,
            trip.OwnerId);

        AnsiConsole.MarkupLine(
            "\n[green]✓ Подорож успішно створена.[/]");

        Console.ReadKey();
    }
}