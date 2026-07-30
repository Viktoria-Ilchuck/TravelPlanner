using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public interface IHotelBookingRepository
{
    Task<List<HotelBooking>> GetAllAsync();

    Task<List<HotelBooking>> GetByTripAsync(int tripId);

    Task<HotelBooking?> GetByIdAsync(int id);

    Task AddAsync(HotelBooking booking);

    Task UpdateAsync(HotelBooking booking);

    Task DeleteAsync(int id);
}