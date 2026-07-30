using TravelPlanner.DTO;
using TravelPlanner.Models;
using TravelPlanner.Repositories;

namespace TravelPlanner.Services;

public class TripService
{
    private readonly ITripRepository _tripRepository;
    private readonly HotelBookingService _hotelBookingService;

    public TripService(
        ITripRepository tripRepository,
        HotelBookingService hotelBookingService)
    {
        _tripRepository = tripRepository;
        _hotelBookingService = hotelBookingService;
    }

    public async Task CreateTripAsync(
        string title,
        string description,
        DateTime startDate,
        DateTime endDate,
        decimal budget,
        int cityId,
        int ownerId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new Exception("Введіть назву подорожі.");

        if (budget <= 0)
            throw new Exception("Бюджет повинен бути більшим за 0.");

        if (startDate >= endDate)
            throw new Exception("Дата початку повинна бути раніше дати завершення.");

        var trip = new Trip
        {
            Title = title.Trim(),
            Description = description?.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            Budget = budget,
            Status = "Запланована",
            CityId = cityId,
            OwnerId = ownerId
        };

        await _tripRepository.AddAsync(trip);
    }

    public async Task<List<Trip>> GetAllTripsAsync()
    {
        return await _tripRepository.GetAllAsync();
    }

    public async Task<List<Trip>> GetMyTripsAsync(int ownerId)
    {
        return await _tripRepository.GetByOwnerAsync(ownerId);
    }

    public async Task<Trip?> GetTripAsync(int id)
    {
        return await _tripRepository.GetByIdAsync(id);
    }

    public async Task UpdateTripAsync(Trip trip)
    {
        if (string.IsNullOrWhiteSpace(trip.Title))
            throw new Exception("Введіть назву подорожі.");

        if (trip.Budget <= 0)
            throw new Exception("Бюджет повинен бути більшим за 0.");

        if (trip.StartDate >= trip.EndDate)
            throw new Exception("Дата початку повинна бути раніше дати завершення.");

        trip.Title = trip.Title.Trim();

        if (!string.IsNullOrWhiteSpace(trip.Description))
            trip.Description = trip.Description.Trim();

        await _tripRepository.UpdateAsync(trip);
    }

    public async Task DeleteTripAsync(int id)
    {
        await _tripRepository.DeleteAsync(id);
    }

    public async Task<List<Trip>> SearchTripsAsync(string text)
    {
        return await _tripRepository.SearchAsync(text);
    }

    public async Task<List<Trip>> GetTripsByCityAsync(int cityId)
    {
        return await _tripRepository.GetByCityAsync(cityId);
    }

    public async Task<List<Trip>> GetTripsByDateAsync(DateTime start, DateTime end)
    {
        return await _tripRepository.GetByDateAsync(start, end);
    }

    public async Task<List<TripDto>> GetDetailedTripsAsync(int ownerId)
    {
        var trips = await _tripRepository.GetDetailedTripsAsync(ownerId);

        foreach (var trip in trips)
        {
            trip.HotelBookings =
                await _hotelBookingService.GetByTripAsync(trip.Id);
        }

        return trips;
    }
    
}