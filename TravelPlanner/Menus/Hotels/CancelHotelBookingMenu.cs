using Spectre.Console;
using TravelPlanner.Models;
using TravelPlanner.Services;

namespace TravelPlanner.Menus.Hotels;

public class CancelHotelBookingMenu
{
    private readonly HotelBookingService _bookingService;

    public CancelHotelBookingMenu(
        HotelBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task ShowAsync(int tripId)
    {
        AnsiConsole.Clear();

        var bookings =
            await _bookingService.GetByTripAsync(tripId);

        if (!bookings.Any())
        {
            AnsiConsole.MarkupLine(
                "[yellow]У цієї подорожі немає бронювань.[/]");

            Console.ReadKey();
            return;
        }

        var booking = AnsiConsole.Prompt(
            new SelectionPrompt<HotelBooking>()
                .Title("Оберіть бронювання")
                .UseConverter(x =>
                    $"{x.Hotel!.Name} | {x.CheckIn:dd.MM.yyyy} - {x.CheckOut:dd.MM.yyyy} | {x.Guests} гостей")
                .AddChoices(bookings));

        var confirm = AnsiConsole.Confirm(
            "Скасувати це бронювання?");

        if (!confirm)
            return;

        await _bookingService.DeleteAsync(booking.Id);

        AnsiConsole.MarkupLine(
            "[green]✓ Бронювання успішно скасовано.[/]");

        Console.ReadKey();
    }
}