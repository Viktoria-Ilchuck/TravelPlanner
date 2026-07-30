using Spectre.Console;
using TravelPlanner.DTO;
using TravelPlanner.Services;

namespace TravelPlanner.Menus.Expenses;

public class EditExpenseMenu
{
    private readonly ExpenseService _expenseService;
    private readonly TripService _tripService;
    private readonly CurrentUserService _currentUser;

    public EditExpenseMenu(
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
            new SelectionPrompt<TravelPlanner.Models.Expense>()
                .Title("Оберіть витрату")
                .UseConverter(x => $"{x.Date:dd.MM.yyyy} | {x.Description} | {x.Amount:F2} грн")
                .AddChoices(expenses));

        expense.Amount = AnsiConsole.Ask(
            "Нова сума:",
            expense.Amount);

        expense.Description = AnsiConsole.Ask(
            "Новий опис:",
            expense.Description);

        try
        {
            await _expenseService.UpdateAsync(expense);

            AnsiConsole.MarkupLine("[green]Витрату успішно оновлено![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Console.ReadKey();
    }
}