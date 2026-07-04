using TravelPlanner.Models;
using TravelPlanner.Repositories;
using TravelPlanner.DTO;

namespace TravelPlanner.Services;

public class TripService
{
    private readonly ITripRepository _tripRepository;

    public TripService(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
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
        var trip = new Trip
        {
            Title = title,
            Description = description,
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
        return await _tripRepository.GetDetailedTripsAsync(ownerId);
    }
    
    public async Task<List<Trip>> GetUserTripsAsync(int ownerId)
    {
        return await _tripRepository.GetByOwnerAsync(ownerId);
    }
}