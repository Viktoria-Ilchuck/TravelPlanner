using Dapper;
using TravelPlanner.Data;
using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly DatabaseContext _context;

    public CountryRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Countries
            ORDER BY Name
            """;

        return await connection.QueryAsync<Country>(sql);
    }

    public async Task<Country?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Countries
            WHERE Id=@Id
            """;

        return await connection.QueryFirstOrDefaultAsync<Country>(sql, new
        {
            Id = id
        });
    }

    public async Task<Country?> GetByNameAsync(string name)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Countries
            WHERE lower(Name)=lower(@Name)
            LIMIT 1
            """;

        return await connection.QueryFirstOrDefaultAsync<Country>(sql, new
        {
            Name = name.Trim()
        });
    }

    public async Task AddAsync(Country country)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            INSERT INTO Countries(Name)
            VALUES(@Name)
            """;

        await connection.ExecuteAsync(sql, country);
    }

    public async Task UpdateAsync(Country country)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            UPDATE Countries
            SET Name=@Name
            WHERE Id=@Id
            """;

        await connection.ExecuteAsync(sql, country);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            DELETE FROM Countries
            WHERE Id=@Id
            """;

        await connection.ExecuteAsync(sql, new
        {
            Id = id
        });
    }
}