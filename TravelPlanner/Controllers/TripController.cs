using TravelPlanner.Models;
using TravelPlanner.Services;

namespace TravelPlanner.Controllers;

public class TripController
{
    private readonly TripService _tripService;
    private readonly CurrentUserService _currentUser;

    public TripController(
        TripService tripService,
        CurrentUserService currentUser)
    {
        _tripService = tripService;
        _currentUser = currentUser;
    }

    public async Task CreateTripAsync(
        string title,
        string description,
        DateTime startDate,
        DateTime endDate,
        decimal budget,
        int cityId)
    {
        await _tripService.CreateTripAsync(
            title,
            description,
            startDate,
            endDate,
            budget,
            cityId,
            _currentUser.CurrentUser!.Id);
    }

    public async Task<List<Trip>> GetMyTripsAsync()
    {
        return await _tripService.GetMyTripsAsync(
            _currentUser.CurrentUser!.Id);
    }

    public async Task DeleteTripAsync(int id)
    {
        await _tripService.DeleteTripAsync(id);
    }

    public async Task UpdateTripAsync(Trip trip)
    {
        await _tripService.UpdateTripAsync(trip);
    }

    public async Task<Trip?> GetTripAsync(int id)
    {
        return await _tripService.GetTripAsync(id);
    }
}