using Dapper;
using TravelPlanner.Data;
using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly DatabaseContext _context;

    public HotelRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Hotel>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        const string sql = """
        SELECT
            h.Id,
            h.Name,
            h.Address,
            h.Stars,
            h.Phone,
            h.Email,
            h.Website,
            h.PricePerNight,
            h.CityId,
            c.Name AS CityName,
            h.TripId
        FROM Hotels h
        JOIN Cities c ON c.Id = h.CityId
        ORDER BY h.Name;
        """;

        var hotels = await connection.QueryAsync<Hotel>(sql);

        return hotels.ToList();
    }

    public async Task<Hotel?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
        SELECT
            h.Id,
            h.Name,
            h.Address,
            h.Stars,
            h.Phone,
            h.Email,
            h.Website,
            h.PricePerNight,
            h.CityId,
            c.Name AS CityName,
            h.TripId
        FROM Hotels h
        JOIN Cities c ON c.Id = h.CityId
        WHERE h.Id=@Id;
        """;

        return await connection.QueryFirstOrDefaultAsync<Hotel>(sql, new { Id = id });
    }

    public async Task AddAsync(Hotel hotel)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
        INSERT INTO Hotels
        (
            Name,
            Address,
            Stars,
            Phone,
            Email,
            Website,
            PricePerNight,
            CityId,
            TripId
        )
        VALUES
        (
            @Name,
            @Address,
            @Stars,
            @Phone,
            @Email,
            @Website,
            @PricePerNight,
            @CityId,
            @TripId
        );
        """;

        await connection.ExecuteAsync(sql, hotel);
    }

    public async Task UpdateAsync(Hotel hotel)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
        UPDATE Hotels
        SET
            Name=@Name,
            Address=@Address,
            Stars=@Stars,
            Phone=@Phone,
            Email=@Email,
            Website=@Website,
            PricePerNight=@PricePerNight,
            CityId=@CityId,
            TripId=@TripId
        WHERE Id=@Id;
        """;

        await connection.ExecuteAsync(sql, hotel);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
        DELETE FROM Hotels
        WHERE Id=@Id;
        """;

        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<List<Hotel>> SearchAsync(string name)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
        SELECT
            h.Id,
            h.Name,
            h.Address,
            h.Stars,
            h.Phone,
            h.Email,
            h.Website,
            h.PricePerNight,
            h.CityId,
            c.Name AS CityName,
            h.TripId
        FROM Hotels h
        JOIN Cities c ON c.Id = h.CityId
        WHERE h.Name LIKE @Name
        ORDER BY h.Name;
        """;

        var hotels = await connection.QueryAsync<Hotel>(
            sql,
            new { Name = $"%{name}%" });

        return hotels.ToList();
    }

    public async Task<List<Hotel>> GetByStarsAsync(int stars)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
        SELECT
            h.Id,
            h.Name,
            h.Address,
            h.Stars,
            h.Phone,
            h.Email,
            h.Website,
            h.PricePerNight,
            h.CityId,
            c.Name AS CityName,
            h.TripId
        FROM Hotels h
        JOIN Cities c ON c.Id = h.CityId
        WHERE h.Stars=@Stars
        ORDER BY h.Name;
        """;

        var hotels = await connection.QueryAsync<Hotel>(sql, new { Stars = stars });

        return hotels.ToList();
    }

    public async Task<List<Hotel>> GetSortedByNameAsync()
    {
        return await GetAllAsync();
    }

    public async Task<List<Hotel>> GetSortedByPriceAsync()
    {
        using var connection = _context.CreateConnection();

        const string sql = """
        SELECT
            h.Id,
            h.Name,
            h.Address,
            h.Stars,
            h.Phone,
            h.Email,
            h.Website,
            h.PricePerNight,
            h.CityId,
            c.Name AS CityName,
            h.TripId
        FROM Hotels h
        JOIN Cities c ON c.Id = h.CityId
        ORDER BY h.PricePerNight;
        """;

        var hotels = await connection.QueryAsync<Hotel>(sql);

        return hotels.ToList();
    }
    
    
    
    public async Task<List<Hotel>> GetByCityAsync(string cityName)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           SELECT
                               h.Id,
                               h.Name,
                               h.Address,
                               h.Stars,
                               h.Phone,
                               h.Email,
                               h.Website,
                               h.PricePerNight,
                               h.CityId,
                               c.Name AS CityName,
                               h.TripId
                           FROM Hotels h
                           JOIN Cities c ON c.Id = h.CityId
                           WHERE LOWER(c.Name)=LOWER(@CityName)
                           ORDER BY h.PricePerNight;
                           """;

        var hotels = await connection.QueryAsync<Hotel>(
            sql,
            new
            {
                CityName = cityName
            });

        return hotels.ToList();
    }
}