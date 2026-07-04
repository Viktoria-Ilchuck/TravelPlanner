using TravelPlanner.DTO;
using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public interface ITripRepository
{
    Task<List<Trip>> GetAllAsync();

    Task<Trip?> GetByIdAsync(int id);

    Task AddAsync(Trip trip);

    Task UpdateAsync(Trip trip);

    Task DeleteAsync(int id);

    Task<List<Trip>> SearchAsync(string text);

    Task<List<Trip>> GetByOwnerAsync(int ownerId);

    Task<List<TripDto>> GetDetailedTripsAsync(int ownerId);

    Task<List<Trip>> GetByCityAsync(int cityId);

    Task<List<Trip>> GetByDateAsync(DateTime start, DateTime end);
}