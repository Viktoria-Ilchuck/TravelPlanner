using Dapper;
using TravelPlanner.Data;
using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public class HotelBookingRepository : IHotelBookingRepository
{
    private readonly DatabaseContext _context;

    public HotelBookingRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<HotelBooking>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM HotelBookings
            ORDER BY CheckIn
            """;

        var bookings = await connection.QueryAsync<HotelBooking>(sql);

        return bookings.ToList();
    }

   

    public async Task<HotelBooking?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            SELECT *
            FROM HotelBookings
            WHERE Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<HotelBooking>(
            sql,
            new
            {
                Id = id
            });
    }

    public async Task AddAsync(HotelBooking booking)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            INSERT INTO HotelBookings
            (
                HotelId,
                TripId,
                CheckIn,
                CheckOut,
                Guests
            )
            VALUES
            (
                @HotelId,
                @TripId,
                @CheckIn,
                @CheckOut,
                @Guests
            )
            """;

        await connection.ExecuteAsync(sql, booking);
    }

    public async Task UpdateAsync(HotelBooking booking)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            UPDATE HotelBookings
            SET
                HotelId = @HotelId,
                TripId = @TripId,
                CheckIn = @CheckIn,
                CheckOut = @CheckOut,
                Guests = @Guests
            WHERE Id = @Id
            """;

        await connection.ExecuteAsync(sql, booking);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
            DELETE FROM HotelBookings
            WHERE Id = @Id
            """;

        await connection.ExecuteAsync(sql, new
        {
            Id = id
        });
    }
    
    public async Task<List<HotelBooking>> GetByTripAsync(int tripId)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           SELECT
                               hb.Id,
                               hb.HotelId,
                               hb.TripId,
                               hb.CheckIn,
                               hb.CheckOut,
                               hb.Guests,

                               h.Id,
                               h.Name,
                               h.Address,
                               h.Stars,
                               h.Phone,
                               h.Email,
                               h.Website,
                               h.PricePerNight,
                               h.CityId,
                               h.TripId

                           FROM HotelBookings hb
                           INNER JOIN Hotels h
                               ON hb.HotelId = h.Id
                           WHERE hb.TripId = @TripId
                           ORDER BY hb.CheckIn
                           """;

        var result = await connection.QueryAsync<HotelBooking, Hotel, HotelBooking>(
            sql,
            (booking, hotel) =>
            {
                booking.Hotel = hotel;
                return booking;
            },
            new { TripId = tripId },
            splitOn: "Id");

        return result.ToList();
    }
}