using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public interface IExpenseRepository
{
    Task<List<Expense>> GetAllAsync();

    Task<Expense?> GetByIdAsync(int id);

    Task<List<Expense>> GetByTripAsync(int tripId);

    Task AddAsync(Expense expense);

    Task UpdateAsync(Expense expense);

    Task DeleteAsync(int id);
}