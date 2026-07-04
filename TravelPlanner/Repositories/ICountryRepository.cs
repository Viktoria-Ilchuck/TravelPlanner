using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public interface ICountryRepository
{
    Task<IEnumerable<Country>> GetAllAsync();

    Task<Country?> GetByIdAsync(int id);

    Task<Country?> GetByNameAsync(string name);

    Task AddAsync(Country country);

    Task UpdateAsync(Country country);

    Task DeleteAsync(int id);
}