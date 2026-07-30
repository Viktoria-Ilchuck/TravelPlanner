using Spectre.Console;
using TravelPlanner.Services;

namespace TravelPlanner.Menus.Expenses;

public class ShowExpensesMenu
{
    private readonly ExpenseService _expenseService;
    private readonly TripService _tripService;
    private readonly CurrentUserService _currentUser;

    public ShowExpensesMenu(
        ExpenseService expenseService,
        TripService tripService,
        CurrentUserService currentUser)
    {
        _expenseService = expenseService;
        _tripService = tripService;
        _currentUser = currentUser;
    }

    public async Task ShowAsync()
    {
        Console.Clear();

        var trips = await _tripService.GetDetailedTripsAsync(
            _currentUser.CurrentUser!.Id);

        if (!trips.Any())
        {
            AnsiConsole.MarkupLine("[red]У вас немає подорожей.[/]");
            Console.ReadKey();
            return;
        }

        var tripTable = new Table();

        tripTable.Border(TableBorder.Rounded);

        tripTable.AddColumn("ID");
        tripTable.AddColumn("Назва");
        tripTable.AddColumn("Місто");

        foreach (var trip in trips)
        {
            tripTable.AddRow(
                trip.Id.ToString(),
                trip.Title,
                trip.City);
        }

        AnsiConsole.Write(tripTable);

        Console.WriteLine();

        var tripId = AnsiConsole.Ask<int>("ID подорожі:");

        var expenses = await _expenseService.GetByTripAsync(tripId);

        Console.Clear();

        if (!expenses.Any())
        {
            AnsiConsole.MarkupLine("[yellow]Для цієї подорожі ще немає витрат.[/]");
            Console.ReadKey();
            return;
        }

        var table = new Table();

        table.Border(TableBorder.Rounded);

        table.AddColumn("ID");
        table.AddColumn("Дата");
        table.AddColumn("Опис");
        table.AddColumn("Сума");
        table.AddColumn("Валюта");

        foreach (var expense in expenses)
        {
            table.AddRow(
                expense.Id.ToString(),
                expense.Date.ToShortDateString(),
                expense.Description,
                expense.Amount.ToString("F2"),
                expense.Currency);
        }

        AnsiConsole.Write(table);

        Console.WriteLine();

        Console.ReadKey();
    }
}