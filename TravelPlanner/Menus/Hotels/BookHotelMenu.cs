using Spectre.Console;
using TravelPlanner.Models;
using TravelPlanner.Services;

namespace TravelPlanner.Menus.Hotels;

public class BookHotelMenu
{
    private readonly HotelService _hotelService;
    private readonly HotelBookingService _bookingService;

    public BookHotelMenu(
        HotelService hotelService,
        HotelBookingService bookingService)
    {
        _hotelService = hotelService;
        _bookingService = bookingService;
    }

    public async Task ShowAsync(int tripId, string city)
    {
        AnsiConsole.Clear();

        var hotels = await _hotelService.GetAllAsync();

        hotels = hotels
            .Where(x => x.CityName.Equals(city,
                StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!hotels.Any())
        {
            AnsiConsole.MarkupLine(
                "[red]У цьому місті немає доступних готелів.[/]");

            Console.ReadKey();
            return;
        }

        var table = new Table();

        table.Border(TableBorder.Rounded);

        table.AddColumn("ID");
        table.AddColumn("Назва");
        table.AddColumn("Зірки");
        table.AddColumn("Ціна");

        foreach (var hotel in hotels)
        {
            table.AddRow(
                hotel.Id.ToString(),
                hotel.Name,
                $"{hotel.Stars}★",
                $"{hotel.PricePerNight:N0} грн");
        }

        AnsiConsole.Write(table);

        Console.WriteLine();

        var hotelId = AnsiConsole.Ask<int>(
            "Введіть ID готелю (0 - назад):");

        if (hotelId == 0)
            return;

        var selectedHotel =
            hotels.FirstOrDefault(x => x.Id == hotelId);

        if (selectedHotel == null)
        {
            AnsiConsole.MarkupLine("[red]Готель не знайдено.[/]");
            Console.ReadKey();
            return;
        }

        var checkIn = AnsiConsole.Ask<DateTime>(
            "Дата заїзду (yyyy-MM-dd):");

        var checkOut = AnsiConsole.Ask<DateTime>(
            "Дата виїзду (yyyy-MM-dd):");

        if (checkOut <= checkIn)
        {
            AnsiConsole.MarkupLine(
                "[red]Дата виїзду повинна бути пізніше дати заїзду.[/]");

            Console.ReadKey();
            return;
        }

        var guests = AnsiConsole.Prompt(
            new TextPrompt<int>("Кількість гостей:")
                .Validate(x =>
                    x > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Кількість гостей повинна бути більшою за 0.[/]")));
        

        var booking = new HotelBooking
        {
            HotelId = selectedHotel.Id,
            TripId = tripId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            Guests = guests
        };

        try
        {
            await _bookingService.AddAsync(booking);

            AnsiConsole.MarkupLine("[green]✓ Готель успішно заброньовано![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Console.ReadKey();
    }
}