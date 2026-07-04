using Dapper;
using TravelPlanner.Data;
using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public class CityRepository : ICityRepository
{
    private readonly DatabaseContext _context;

    public CityRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<City>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Cities
            ORDER BY Name
            """;

        var cities = await connection.QueryAsync<City>(sql);

        return cities.ToList();
    }

    public async Task<List<City>> GetByCountryAsync(int countryId)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Cities
            WHERE CountryId=@CountryId
            ORDER BY Name
            """;

        var cities = await connection.QueryAsync<City>(sql, new
        {
            CountryId = countryId
        });

        return cities.ToList();
    }

    public async Task<City?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Cities
            WHERE Id=@Id
            """;

        return await connection.QueryFirstOrDefaultAsync<City>(sql, new
        {
            Id = id
        });
    }

    public async Task<City?> GetByNameAsync(string name, int countryId)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Cities
            WHERE lower(Name)=lower(@Name)
            AND CountryId=@CountryId
            LIMIT 1
            """;

        return await connection.QueryFirstOrDefaultAsync<City>(sql, new
        {
            Name = name.Trim(),
            CountryId = countryId
        });
    }

    public async Task AddAsync(City city)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            INSERT INTO Cities
            (
                Name,
                CountryId
            )
            VALUES
            (
                @Name,
                @CountryId
            )
            """;

        await connection.ExecuteAsync(sql, city);
    }

    public async Task UpdateAsync(City city)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            UPDATE Cities
            SET
                Name=@Name,
                CountryId=@CountryId
            WHERE Id=@Id
            """;

        await connection.ExecuteAsync(sql, city);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            DELETE FROM Cities
            WHERE Id=@Id
            """;

        await connection.ExecuteAsync(sql, new
        {
            Id = id
        });
    }
}