using TravelPlanner.Models;
using TravelPlanner.Repositories;

namespace TravelPlanner.Services;

public class HotelBookingService
{
    private readonly IHotelBookingRepository _repository;

    public HotelBookingService(IHotelBookingRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<HotelBooking>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<List<HotelBooking>> GetByTripAsync(int tripId)
    {
        return await _repository.GetByTripAsync(tripId);
    }

    public async Task<HotelBooking?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(HotelBooking booking)
    {
        if (booking.Guests <= 0)
            throw new Exception("Кількість гостей повинна бути більшою за 0.");

        if (booking.CheckOut <= booking.CheckIn)
            throw new Exception("Дата виїзду повинна бути пізніше дати заїззду.");

        await _repository.AddAsync(booking);
    }

    public async Task UpdateAsync(HotelBooking booking)
    {
        await _repository.UpdateAsync(booking);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}