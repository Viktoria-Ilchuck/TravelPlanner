using Dapper;
using TravelPlanner.Data;
using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly DatabaseContext _context;

    public ExpenseRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Expense>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           SELECT *
                           FROM Expenses
                           ORDER BY Date
                           """;

        var expenses = await connection.QueryAsync<Expense>(sql);

        return expenses.ToList();
    }

    public async Task<Expense?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           SELECT *
                           FROM Expenses
                           WHERE Id=@Id
                           """;

        return await connection.QueryFirstOrDefaultAsync<Expense>(sql, new
        {
            Id = id
        });
    }

    public async Task<List<Expense>> GetByTripAsync(int tripId)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           SELECT *
                           FROM Expenses
                           WHERE TripId=@TripId
                           ORDER BY Date
                           """;

        var expenses = await connection.QueryAsync<Expense>(sql, new
        {
            TripId = tripId
        });

        return expenses.ToList();
    }

    public async Task AddAsync(Expense expense)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           INSERT INTO Expenses
                           (
                               Amount,
                               Date,
                               Description,
                               CategoryId,
                               TripId
                           )
                           VALUES
                           (
                               @Amount,
                               @Date,
                               @Description,
                               @CategoryId,
                               @TripId
                           )
                           """;

        await connection.ExecuteAsync(sql, expense);
    }

    public async Task UpdateAsync(Expense expense)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           UPDATE Expenses
                           SET
                               Amount=@Amount,
                               Date=@Date,
                               Description=@Description,
                               CategoryId=@CategoryId,
                               TripId=@TripId
                           WHERE Id=@Id
                           """;

        await connection.ExecuteAsync(sql, expense);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           DELETE FROM Expenses
                           WHERE Id=@Id
                           """;

        await connection.ExecuteAsync(sql, new
        {
            Id = id
        });
    }
}