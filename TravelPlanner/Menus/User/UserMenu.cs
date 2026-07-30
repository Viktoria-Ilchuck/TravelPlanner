using Spectre.Console;
using TravelPlanner.Menus.Expenses;
using TravelPlanner.Menus.Hotels;
using TravelPlanner.Menus.Reports;
using TravelPlanner.Services;

namespace TravelPlanner.Menus;

public class UserMenu
{
    private readonly TripMenu _tripMenu;
    private readonly CreateTripMenu _createTripMenu;
    private readonly MyTripsMenu _myTripsMenu;
    private readonly EditTripMenu _editTripMenu;
    private readonly DeleteTripMenu _deleteTripMenu;
    private readonly HotelMenu _hotelMenu;
    private readonly ExpenseMenu _expenseMenu;
    private readonly ReportMenu _reportMenu;

    public UserMenu(
        TripMenu tripMenu,
        CreateTripMenu createTripMenu,
        MyTripsMenu myTripsMenu,
        EditTripMenu editTripMenu,
        DeleteTripMenu deleteTripMenu,
        HotelMenu hotelMenu,
        ExpenseMenu expenseMenu,
        ReportMenu reportMenu)
    {
        _tripMenu = tripMenu;
        _createTripMenu = createTripMenu;
        _myTripsMenu = myTripsMenu;
        _editTripMenu = editTripMenu;
        _deleteTripMenu = deleteTripMenu;
        _hotelMenu = hotelMenu;
        _expenseMenu = expenseMenu;
        _reportMenu = reportMenu;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new FigletText("Travel Planner")
                    .Centered()
                    .Color(Color.DeepSkyBlue1));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("Головне меню")
                    .PageSize(10)
                    .AddChoices(1, 2, 3, 4, 5, 6, 7, 0)
                    .UseConverter(x => x switch
                    {
                        1 => "✈ Подорожі",
                        2 => "🏨 Готелі",
                        3 => "🎯 Активності",
                        4 => "💰 Витрати",
                        5 => "📄 Звіти",
                        6 => "💱 Валюта",
                        7 => "👤 Профіль",
                        0 => "🚪 Вийти",
                        _ => ""
                    }));

            switch (choice)
            {
                case 1:
                    await ShowTripsAsync();
                    break;

                case 2:
                    await _hotelMenu.ShowAsync();
                    break;

                case 3:
                    AnsiConsole.MarkupLine("[yellow]Розділ активностей ще не реалізований[/]");
                    Console.ReadKey();
                    break;

                case 4:
                    await _expenseMenu.ShowAsync();
                    break;

                case 5:
                    await _reportMenu.ShowAsync();
                    break;

                case 6:
                    AnsiConsole.MarkupLine("[yellow]Розділ валют ще не реалізований[/]");
                    Console.ReadKey();
                    break;

                case 7:
                    AnsiConsole.MarkupLine("[yellow]Профіль ще не реалізований[/]");
                    Console.ReadKey();
                    break;

                case 0:
                    return;
            }
        }
    }

    private async Task ShowTripsAsync()
    {
        while (true)
        {
            var choice = _tripMenu.Show();

            switch (choice)
            {
                case 1:
                    await _createTripMenu.ShowAsync();
                    break;

                case 2:
                    await _myTripsMenu.ShowAsync();
                    break;

                case 3:
                    await _editTripMenu.ShowAsync();
                    break;

                case 4:
                    await _deleteTripMenu.ShowAsync();
                    break;

                case 0:
                    return;
            }
        }
    }
}