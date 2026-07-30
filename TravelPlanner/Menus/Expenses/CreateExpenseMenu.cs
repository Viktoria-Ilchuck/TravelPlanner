using Spectre.Console;
using TravelPlanner.Models;
using TravelPlanner.Services;

namespace TravelPlanner.Menus.Expenses;

public class CreateExpenseMenu
{
    private readonly ExpenseService _expenseService;
    private readonly TripService _tripService;
    private readonly CurrentUserService _currentUser;

    public CreateExpenseMenu(
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
            AnsiConsole.MarkupLine("[red]Спочатку створіть подорож.[/]");
            Console.ReadKey();
            return;
        }

        var table = new Table();

        table.Border(TableBorder.Rounded);

        table.AddColumn("ID");
        table.AddColumn("Назва");
        table.AddColumn("Місто");

        foreach (var trip in trips)
        {
            table.AddRow(
                trip.Id.ToString(),
                trip.Title,
                trip.City);
        }

        AnsiConsole.Write(table);

        Console.WriteLine();

        var tripId = AnsiConsole.Ask<int>("ID подорожі:");

        var amount = AnsiConsole.Ask<decimal>("Сума:");

        var description = AnsiConsole.Ask<string>("Опис:");

        var expense = new Expense
        {
            TripId = tripId,
            Amount = amount,
            Currency = "UAH",
            Description = description,
            Date = DateTime.Now,
            CategoryId = 1
        };

        try
        {
            await _expenseService.AddAsync(expense);

            AnsiConsole.MarkupLine("[green]Витрату успішно додано![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Console.ReadKey();
    }
}