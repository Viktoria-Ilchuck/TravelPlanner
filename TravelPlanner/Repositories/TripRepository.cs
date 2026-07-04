using Dapper;
using TravelPlanner.Data;
using TravelPlanner.DTO;
using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public class TripRepository : ITripRepository
{
    private readonly DatabaseContext _context;

    public TripRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Trip>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Trips
            ORDER BY StartDate
            """;

        var trips = await connection.QueryAsync<Trip>(sql);

        return trips.ToList();
    }

    public async Task<Trip?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Trips
            WHERE Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<Trip>(sql, new { Id = id });
    }

    public async Task AddAsync(Trip trip)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           INSERT INTO Trips
                           (
                               Title,
                               Description,
                               StartDate,
                               EndDate,
                               Budget,
                               Status,
                               CityId,
                               OwnerId
                           )
                           VALUES
                           (
                               @Title,
                               @Description,
                               @StartDate,
                               @EndDate,
                               @Budget,
                               @Status,
                               @CityId,
                               @OwnerId
                           )
                           """;

        await connection.ExecuteAsync(sql, trip);
    }

    public async Task UpdateAsync(Trip trip)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           UPDATE Trips
                           SET
                               Title = @Title,
                               Description = @Description,
                               StartDate = @StartDate,
                               EndDate = @EndDate,
                               Budget = @Budget,
                               Status = @Status,
                               CityId = @CityId,
                               OwnerId = @OwnerId
                           WHERE Id = @Id
                           """;

        await connection.ExecuteAsync(sql, trip);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            DELETE FROM Trips
            WHERE Id=@Id
            """;

        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<List<Trip>> SearchAsync(string text)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Trips
            WHERE Title LIKE @Text
               OR Description LIKE @Text
            ORDER BY StartDate
            """;

        var trips = await connection.QueryAsync<Trip>(sql, new
        {
            Text = $"%{text}%"
        });

        return trips.ToList();
    }

    public async Task<List<Trip>> GetByOwnerAsync(int ownerId)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Trips
            WHERE OwnerId=@OwnerId
            ORDER BY StartDate
            """;

        var trips = await connection.QueryAsync<Trip>(sql, new
        {
            OwnerId = ownerId
        });

        return trips.ToList();
    }

    public async Task<List<Trip>> GetByCityAsync(int cityId)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Trips
            WHERE CityId=@CityId
            ORDER BY StartDate
            """;

        var trips = await connection.QueryAsync<Trip>(sql, new
        {
            CityId = cityId
        });

        return trips.ToList();
    }

    public async Task<List<Trip>> GetByDateAsync(DateTime start, DateTime end)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM Trips
            WHERE StartDate>=@Start
            AND EndDate<=@End
            ORDER BY StartDate
            """;

        var trips = await connection.QueryAsync<Trip>(sql, new
        {
            Start = start,
            End = end
        });

        return trips.ToList();
    }
    public async Task<List<TripDto>> GetDetailedTripsAsync(int ownerId)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           SELECT
                               t.Id,
                               t.Title,
                               t.Description,
                               t.StartDate,
                               t.EndDate,
                               t.Budget,
                               t.Status,
                               c.Name AS City
                           FROM Trips t
                           INNER JOIN Cities c
                               ON t.CityId = c.Id
                           WHERE t.OwnerId = @OwnerId
                           ORDER BY t.StartDate
                           """;

        var trips = await connection.QueryAsync<TripDto>(sql, new
        {
            OwnerId = ownerId
        });

        return trips.ToList();
    }
}