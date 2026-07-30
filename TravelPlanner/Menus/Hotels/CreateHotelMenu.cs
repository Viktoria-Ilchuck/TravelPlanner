using Spectre.Console;
using TravelPlanner.Models;
using TravelPlanner.Services;
using TravelPlanner.Validators;

namespace TravelPlanner.Menus.Hotels;

public class CreateHotelMenu
{
    private readonly HotelService _hotelService;
    private readonly HotelValidator _validator;

    public CreateHotelMenu(
        HotelService hotelService,
        HotelValidator validator)
    {
        _hotelService = hotelService;
        _validator = validator;
    }

    public async Task ShowAsync()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new Rule("[yellow]Додавання готелю[/]")
                .Centered());

        var name = AnsiConsole.Ask<string>("Назва готелю:");

        var address = AnsiConsole.Ask<string>("Адреса:");

        var stars = AnsiConsole.Ask<int>("Кількість зірок (1-5):");

        if (stars < 1 || stars > 5)
        {
            AnsiConsole.MarkupLine("[red]Кількість зірок повинна бути від 1 до 5.[/]");
            Console.ReadKey();
            return;
        }

        var phone = AnsiConsole.Ask<string>("Телефон:");

        var email = AnsiConsole.Ask<string>("Email:");

        var website = AnsiConsole.Ask<string>("Вебсайт:");

        var price = AnsiConsole.Ask<decimal>("Ціна за ніч:");

        if (price < 0)
        {
            AnsiConsole.MarkupLine("[red]Ціна не може бути від'ємною.[/]");
            Console.ReadKey();
            return;
        }

        var cityName = AnsiConsole.Ask<string>("Місто:");

        var cityId = await _hotelService.GetOrCreateCityAsync(cityName);
        
        
        var hotel = new Hotel
        {
            Name = name,
            Address = address,
            Stars = stars,
            Phone = phone,
            Email = email,
            Website = website,
            PricePerNight = price,
            CityId = cityId,
            TripId = null
        };
        
        var result = _validator.Validate(hotel);

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

        await _hotelService.AddAsync(hotel);

        AnsiConsole.MarkupLine("[green]✓ Готель успішно створено![/]");
        Console.ReadKey();
    }
}