using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public interface IHotelRepository
{
    Task<List<Hotel>> GetAllAsync();

    Task<Hotel?> GetByIdAsync(int id);

    Task AddAsync(Hotel hotel);

    Task UpdateAsync(Hotel hotel);

    Task DeleteAsync(int id);

    Task<List<Hotel>> SearchAsync(string name);

    Task<List<Hotel>> GetByStarsAsync(int stars);

    Task<List<Hotel>> GetSortedByNameAsync();

    Task<List<Hotel>> GetSortedByPriceAsync();

    Task<List<Hotel>> GetByCityAsync(string cityName);
}