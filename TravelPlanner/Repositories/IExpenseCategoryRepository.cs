using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public interface IExpenseCategoryRepository
{
    Task<List<ExpenseCategory>> GetAllAsync();

    Task<ExpenseCategory?> GetByNameAsync(string name);

    Task AddAsync(ExpenseCategory category);
}