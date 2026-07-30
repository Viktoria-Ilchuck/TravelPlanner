using Spectre.Console;
using TravelPlanner.Services;
using TravelPlanner.DTO;

namespace TravelPlanner.Menus.Expenses;

public class MyExpensesMenu
{
    private readonly ExpenseService _expenseService;
    private readonly TripService _tripService;
    private readonly CurrentUserService _currentUser;

    public MyExpensesMenu(
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
            AnsiConsole.MarkupLine("[red]У вас ще немає подорожей.[/]");
            Console.ReadKey();
            return;
        }

        var trip = AnsiConsole.Prompt(
            new SelectionPrompt<TripDto>()
                .Title("Оберіть подорож")
                .UseConverter(x => $"{x.Title} ({x.City})")
                .AddChoices(trips));

        var expenses = await _expenseService.GetByTripAsync(trip.Id);

        Console.Clear();

        if (!expenses.Any())
        {
            AnsiConsole.MarkupLine("[yellow]Для цієї подорожі ще немає витрат.[/]");
            Console.ReadKey();
            return;
        }

        var table = new Table();

        table.Border(TableBorder.Rounded);

        table.AddColumn("Дата");
        table.AddColumn("Опис");
        table.AddColumn("Сума");
        table.AddColumn("Валюта");

        foreach (var expense in expenses)
        {
            table.AddRow(
                expense.Date.ToString("dd.MM.yyyy"),
                expense.Description,
                expense.Amount.ToString("F2"),
                expense.Currency);
        }

        AnsiConsole.Write(table);

        var total = await _expenseService.GetTotalExpensesAsync(trip.Id);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[green]Загальна сума витрат: {total:F2} грн[/]");

        Console.ReadKey();
    }
}