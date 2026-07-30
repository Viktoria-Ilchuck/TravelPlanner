using Spectre.Console;
using TravelPlanner.Models;
using TravelPlanner.Services;

namespace TravelPlanner.Menus.Hotels;

public class HotelMenu
{
    private readonly HotelService _hotelService;
    private readonly CreateHotelMenu _createHotelMenu;
    private readonly EditHotelMenu _editHotelMenu;
    private readonly DeleteHotelMenu _deleteHotelMenu;

    public HotelMenu(
        HotelService hotelService,
        CreateHotelMenu createHotelMenu,
        EditHotelMenu editHotelMenu,
        DeleteHotelMenu deleteHotelMenu)
    {
        _hotelService = hotelService;
        _createHotelMenu = createHotelMenu;
        _editHotelMenu = editHotelMenu;
        _deleteHotelMenu = deleteHotelMenu;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.Clear();

            AnsiConsole.Write(
                new Rule("[yellow]Керування готелями[/]")
                    .Centered());

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Оберіть дію")
                    .AddChoices(
                        "Переглянути готелі",
                        "Пошук готелю",
                        "Фільтр за зірками",
                        "Сортувати за назвою",
                        "Сортувати за ціною",
                        "Додати готель",
                        "Редагувати готель",
                        "Видалити готель",
                        "Назад"));

            switch (choice)
            {
                case "Переглянути готелі":
                    await ShowHotelsAsync();
                    break;

                case "Пошук готелю":
                    await SearchHotelsAsync();
                    break;

                case "Фільтр за зірками":
                    await FilterByStarsAsync();
                    break;

                case "Сортувати за назвою":
                    await ShowSortedByNameAsync();
                    break;

                case "Сортувати за ціною":
                    await ShowSortedByPriceAsync();
                    break;

                case "Додати готель":
                    await _createHotelMenu.ShowAsync();
                    break;

                case "Редагувати готель":
                    await _editHotelMenu.ShowAsync();
                    break;

                case "Видалити готель":
                    await _deleteHotelMenu.ShowAsync();
                    break;

                case "Назад":
                    return;
            }
        }
    }

    private async Task ShowHotelsAsync()
    {
        Console.Clear();

        var hotels = await _hotelService.GetAllAsync();

        ShowTable(hotels);
    }

    private async Task SearchHotelsAsync()
    {
        Console.Clear();

        var name = AnsiConsole.Ask<string>("Введіть назву готелю:");

        var hotels = await _hotelService.SearchAsync(name);

        ShowTable(hotels);
    }

    private async Task FilterByStarsAsync()
    {
        Console.Clear();

        var stars = AnsiConsole.Ask<int>("Введіть кількість зірок:");

        var hotels = await _hotelService.GetByStarsAsync(stars);

        ShowTable(hotels);
    }

    private async Task ShowSortedByNameAsync()
    {
        Console.Clear();

        var hotels = await _hotelService.GetSortedByNameAsync();

        ShowTable(hotels);
    }

    private async Task ShowSortedByPriceAsync()
    {
        Console.Clear();

        var hotels = await _hotelService.GetSortedByPriceAsync();

        ShowTable(hotels);
    }

    private void ShowTable(List<Hotel> hotels)
    {
        Console.Clear();

        if (!hotels.Any())
        {
            AnsiConsole.MarkupLine("[red]Готелів не знайдено.[/]");
            Console.ReadKey();
            return;
        }

        var table = new Table();

        table.Border(TableBorder.Rounded);

        table.AddColumn("[yellow]ID[/]");
        table.AddColumn("[yellow]Назва[/]");
        table.AddColumn("[yellow]Зірки[/]");
        table.AddColumn("[yellow]Місто[/]");
        table.AddColumn("[yellow]Ціна за ніч[/]");

        foreach (var hotel in hotels)
        {
            table.AddRow(
                hotel.Id.ToString(),
                hotel.Name,
                $"{hotel.Stars}★",
                hotel.CityName,
                $"{hotel.PricePerNight:F2}");
        }

        AnsiConsole.Write(table);

        Console.WriteLine();
        AnsiConsole.MarkupLine("[grey]Натисніть будь-яку клавішу, щоб продовжити...[/]");
        Console.ReadKey();
    }
}