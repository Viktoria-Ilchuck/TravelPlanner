using Spectre.Console;
using TravelPlanner.DTO;
using TravelPlanner.Services;

namespace TravelPlanner.Menus.Expenses;

public class DeleteExpenseMenu
{
    private readonly ExpenseService _expenseService;
    private readonly TripService _tripService;
    private readonly CurrentUserService _currentUser;

    public DeleteExpenseMenu(
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

        var trip = AnsiConsole.Prompt(
            new SelectionPrompt<TripDto>()
                .Title("Оберіть подорож")
                .UseConverter(x => $"{x.Title} ({x.City})")
                .AddChoices(trips));

        var expenses = await _expenseService.GetByTripAsync(trip.Id);

        if (!expenses.Any())
        {
            AnsiConsole.MarkupLine("[yellow]У цій подорожі немає витрат.[/]");
            Console.ReadKey();
            return;
        }

        var expense = AnsiConsole.Prompt(
            new SelectionPrompt<Models.Expense>()
                .Title("Оберіть витрату для видалення")
                .UseConverter(x =>
                    $"{x.Date:dd.MM.yyyy} | {x.Description} | {x.Amount:F2} {x.Currency}")
                .AddChoices(expenses));

        var confirm = AnsiConsole.Confirm(
            "Ви впевнені, що хочете видалити цю витрату?");

        if (!confirm)
            return;

        try
        {
            await _expenseService.DeleteAsync(expense.Id);

            AnsiConsole.MarkupLine("[green]Витрату успішно видалено![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Console.ReadKey();
    }
}