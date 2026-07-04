using Spectre.Console;
using TravelPlanner.Services;

namespace TravelPlanner.Menus;

public class CreateTripMenu
{
    private readonly TripService _tripService;
    private readonly CurrentUserService _currentUser;
    private readonly CountryService _countryService;
    private readonly CityService _cityService;

    public CreateTripMenu(
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

        AnsiConsole.Write(
            new FigletText("New Trip")
                .Centered()
                .Color(Color.Green));

        var title = AnsiConsole.Ask<string>("Назва подорожі:");

        var description = AnsiConsole.Ask<string>("Опис:");

        var countryName = AnsiConsole.Ask<string>("Країна:");

        var cityName = AnsiConsole.Ask<string>("Місто:");

        var budget = AnsiConsole.Prompt(
            new TextPrompt<decimal>("Бюджет:")
                .Validate(x =>
                    x >= 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Бюджет не може бути від'ємним[/]")));

        var startDate = AnsiConsole.Ask<DateTime>(
            "Дата початку (yyyy-MM-dd):");

        var endDate = AnsiConsole.Ask<DateTime>(
            "Дата завершення (yyyy-MM-dd):");

        if (endDate < startDate)
        {
            AnsiConsole.MarkupLine(
                "[red]Дата завершення не може бути раніше дати початку.[/]");

            Console.ReadKey();
            return;
        }

        int countryId =
            await _countryService.GetOrCreateCountryAsync(countryName);

        int cityId =
            await _cityService.GetOrCreateCityAsync(
                cityName,
                countryId);

        await _tripService.CreateTripAsync(
            title,
            description,
            startDate,
            endDate,
            budget,
            cityId,
            _currentUser.CurrentUser!.Id);

        AnsiConsole.MarkupLine(
            "\n[green]✓ Подорож успішно створена.[/]");

        Console.ReadKey();
    }
}