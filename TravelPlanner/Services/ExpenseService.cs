using TravelPlanner.Models;
using TravelPlanner.Repositories;

namespace TravelPlanner.Services;

public class ExpenseService
{
    private readonly IExpenseRepository _expenseRepository;

    public ExpenseService(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<List<Expense>> GetAllAsync()
    {
        return await _expenseRepository.GetAllAsync();
    }

    public async Task<List<Expense>> GetByTripAsync(int tripId)
    {
        return await _expenseRepository.GetByTripAsync(tripId);
    }

    public async Task<Expense?> GetByIdAsync(int id)
    {
        return await _expenseRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Expense expense)
    {
        ValidateAndPrepareExpense(expense);

        await _expenseRepository.AddAsync(expense);
    }

    public async Task UpdateAsync(Expense expense)
    {
        ValidateAndPrepareExpense(expense);

        await _expenseRepository.UpdateAsync(expense);
    }

    public async Task DeleteAsync(int id)
    {
        await _expenseRepository.DeleteAsync(id);
    }

    public async Task<decimal> GetTotalExpensesAsync(int tripId)
    {
        var expenses = await _expenseRepository.GetByTripAsync(tripId);

        return expenses.Sum(x => x.Amount);
    }
    
    private static void ValidateAndPrepareExpense(Expense expense)
    {
        if (expense.Amount <= 0)
            throw new Exception("Сума витрати повинна бути більшою за 0.");

        if (string.IsNullOrWhiteSpace(expense.Description))
            throw new Exception("Введіть опис витрати.");

        expense.Description = expense.Description.Trim();
    }
    
}