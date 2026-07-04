using TravelPlanner.Models;
using TravelPlanner.Repositories;

namespace TravelPlanner.Services;

public class ExpenseService
{
    private readonly IExpenseRepository _repository;

    public ExpenseService(IExpenseRepository repository)
    {
        _repository = repository;
    }

    public async Task AddExpenseAsync(
        int tripId,
        int categoryId,
        decimal amount,
        string description,
        DateTime date)
    {
        await _repository.AddAsync(new Expense
        {
            TripId = tripId,
            CategoryId = categoryId,
            Amount = amount,
            Description = description,
            Date = date
        });
    }

    public async Task<List<Expense>> GetExpensesAsync(int tripId)
    {
        return await _repository.GetByTripAsync(tripId);
    }

    public async Task DeleteExpenseAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}