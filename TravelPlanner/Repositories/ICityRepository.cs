using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public interface ICityRepository
{
    Task<List<City>> GetAllAsync();

    Task<List<City>> GetByCountryAsync(int countryId);

    Task<City?> GetByIdAsync(int id);

    Task<City?> GetByNameAsync(string name, int countryId);

    Task AddAsync(City city);

    Task UpdateAsync(City city);

    Task DeleteAsync(int id);
}