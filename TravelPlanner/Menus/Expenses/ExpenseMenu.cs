using Spectre.Console;

namespace TravelPlanner.Menus.Expenses;

public class ExpenseMenu
{
    private readonly CreateExpenseMenu _createExpenseMenu;
    private readonly MyExpensesMenu _myExpensesMenu;
    private readonly EditExpenseMenu _editExpenseMenu;
    private readonly DeleteExpenseMenu _deleteExpenseMenu;

    public ExpenseMenu(
        CreateExpenseMenu createExpenseMenu,
        MyExpensesMenu myExpensesMenu,
        EditExpenseMenu editExpenseMenu,
        DeleteExpenseMenu deleteExpenseMenu)
    {
        _createExpenseMenu = createExpenseMenu;
        _myExpensesMenu = myExpensesMenu;
        _editExpenseMenu = editExpenseMenu;
        _deleteExpenseMenu = deleteExpenseMenu;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Витрати")
                    .AddChoices(
                        "➕ Додати витрату",
                        "📋 Переглянути витрати",
                        "✏ Редагувати витрату",
                        "🗑 Видалити витрату",
                        "⬅ Назад"));

            switch (choice)
            {
                case "➕ Додати витрату":
                    await _createExpenseMenu.ShowAsync();
                    break;

                case "📋 Переглянути витрати":
                    await _myExpensesMenu.ShowAsync();
                    break;

                case "✏ Редагувати витрату":
                    await _editExpenseMenu.ShowAsync();
                    break;

                case "🗑 Видалити витрату":
                    await _deleteExpenseMenu.ShowAsync();
                    break;

                case "⬅ Назад":
                    return;
            }
        }
    }
}