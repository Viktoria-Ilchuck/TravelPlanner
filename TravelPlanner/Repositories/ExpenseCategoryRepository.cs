using Dapper;
using TravelPlanner.Data;
using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public class ExpenseCategoryRepository : IExpenseCategoryRepository
{
    private readonly DatabaseContext _context;

    public ExpenseCategoryRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<ExpenseCategory>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           SELECT *
                           FROM ExpenseCategories
                           ORDER BY Name
                           """;

        var categories = await connection.QueryAsync<ExpenseCategory>(sql);

        return categories.ToList();
    }

    public async Task<ExpenseCategory?> GetByNameAsync(string name)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           SELECT *
                           FROM ExpenseCategories
                           WHERE Name=@Name
                           """;

        return await connection.QueryFirstOrDefaultAsync<ExpenseCategory>(
            sql,
            new
            {
                Name = name
            });
    }

    public async Task AddAsync(ExpenseCategory category)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           INSERT INTO ExpenseCategories(Name)
                           VALUES(@Name)
                           """;

        await connection.ExecuteAsync(sql, category);
    }
}
