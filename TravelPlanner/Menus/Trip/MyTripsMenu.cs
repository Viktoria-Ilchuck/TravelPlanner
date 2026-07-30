using Spectre.Console;
using TravelPlanner.DTO;
using TravelPlanner.Models;
using TravelPlanner.Services;
using TravelPlanner.Menus.Hotels;

namespace TravelPlanner.Menus;

public class MyTripsMenu
{
    private readonly TripService _tripService;
    private readonly HotelBookingService _hotelBookingService;
    private readonly BookHotelMenu _bookHotelMenu;
    private readonly CancelHotelBookingMenu _cancelHotelBookingMenu;
    private readonly CurrentUserService _currentUser;

    public MyTripsMenu(
        TripService tripService,
        HotelBookingService hotelBookingService,
        BookHotelMenu bookHotelMenu,
        CancelHotelBookingMenu cancelHotelBookingMenu,
        CurrentUserService currentUser)
    {
        _tripService = tripService;
        _hotelBookingService = hotelBookingService;
        _bookHotelMenu = bookHotelMenu;
        _cancelHotelBookingMenu = cancelHotelBookingMenu;
        _currentUser = currentUser;
    }

    public async Task ShowAsync()
    {
        while (true)
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

            var table = new Table()
                .Border(TableBorder.Rounded);

            table.AddColumn("[yellow]ID[/]");
            table.AddColumn("[yellow]Назва[/]");
            table.AddColumn("[yellow]Місто[/]");
            table.AddColumn("[yellow]Початок[/]");
            table.AddColumn("[yellow]Кінець[/]");
            table.AddColumn("[yellow]Бюджет[/]");
            table.AddColumn("[yellow]Статус[/]");

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

            Console.WriteLine();

            var id = AnsiConsole.Ask<int>(
                "Введіть ID подорожі (0 - назад):");

            if (id == 0)
                return;

            var selectedTrip = trips.FirstOrDefault(x => x.Id == id);

            if (selectedTrip == null)
            {
                AnsiConsole.MarkupLine("[red]Подорож не знайдена.[/]");
                Console.ReadKey();
                continue;
            }

            await ShowTripInfo(selectedTrip);
        }
    }

    private async Task ShowTripInfo(TripDto trip)
    {
        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new Rule($"[yellow]{trip.Title}[/]"));

            var info = new Table()
                .Border(TableBorder.Rounded);

            info.AddColumn("[yellow]Поле[/]");
            info.AddColumn("[yellow]Значення[/]");

            info.AddRow("Назва", trip.Title);
            info.AddRow("Опис", trip.Description);
            info.AddRow("Країна", trip.Country);
            info.AddRow("Місто", trip.City);
            info.AddRow("Дата початку", trip.StartDate.ToString("dd.MM.yyyy"));
            info.AddRow("Дата завершення", trip.EndDate.ToString("dd.MM.yyyy"));
            info.AddRow("Бюджет", $"{trip.Budget:N0} грн");
            info.AddRow("Статус", trip.Status);

            AnsiConsole.Write(info);

            Console.WriteLine();

            await ShowBookings(trip.Id);

            Console.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Оберіть дію[/]")
                    .AddChoices(
                        "🏨 Забронювати готель",
                        "❌ Скасувати бронювання",
                        "⬅ Повернутися"));

            switch (choice)
            {
                case "🏨 Забронювати готель":
                    await _bookHotelMenu.ShowAsync(
                        trip.Id,
                        trip.City);
                    break;

                case "❌ Скасувати бронювання":
                    await _cancelHotelBookingMenu.ShowAsync(
                        trip.Id);
                    break;

                case "⬅ Повернутися":
                    return;
            }
        }
    }
    
    private async Task ShowBookings(int tripId)
    {
        var bookings = await _hotelBookingService.GetByTripAsync(tripId);

        AnsiConsole.Write(new Rule("[yellow]Бронювання готелів[/]"));

        if (!bookings.Any())
        {
            AnsiConsole.MarkupLine(
                "[grey]Для цієї подорожі ще немає бронювань.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded);

        table.AddColumn("[yellow]Готель[/]");
        table.AddColumn("[yellow]Заїзд[/]");
        table.AddColumn("[yellow]Виїзд[/]");
        table.AddColumn("[yellow]Гостей[/]");

        foreach (var booking in bookings)
        {
            table.AddRow(
                booking.Hotel?.Name ?? "-",
                booking.CheckIn.ToString("dd.MM.yyyy"),
                booking.CheckOut.ToString("dd.MM.yyyy"),
                booking.Guests.ToString());
        }

        AnsiConsole.Write(table);
    }
}