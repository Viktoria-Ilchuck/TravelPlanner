using TravelPlanner.Models;
using TravelPlanner.Repositories;

namespace TravelPlanner.Services;

public class CountryService
{
    private readonly ICountryRepository _countryRepository;

    public CountryService(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        return await _countryRepository.GetAllAsync();
    }

    public async Task<Country?> GetByIdAsync(int id)
    {
        return await _countryRepository.GetByIdAsync(id);
    }

    public async Task<Country?> GetByNameAsync(string name)
    {
        return await _countryRepository.GetByNameAsync(name);
    }

    public async Task<int> GetOrCreateCountryAsync(string name)
    {
        name = name.Trim();

        var country = await _countryRepository.GetByNameAsync(name);

        if (country != null)
            return country.Id;

        await _countryRepository.AddAsync(new Country
        {
            Name = name
        });

        country = await _countryRepository.GetByNameAsync(name);

        return country!.Id;
    }

    public async Task AddAsync(Country country)
    {
        await _countryRepository.AddAsync(country);
    }

    public async Task UpdateAsync(Country country)
    {
        await _countryRepository.UpdateAsync(country);
    }

    public async Task DeleteAsync(int id)
    {
        await _countryRepository.DeleteAsync(id);
    }
}