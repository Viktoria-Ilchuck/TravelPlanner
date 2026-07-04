using TravelPlanner.Models;
using TravelPlanner.Repositories;

namespace TravelPlanner.Services;

public class CityService
{
    private readonly ICityRepository _cityRepository;

    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<List<City>> GetAllAsync()
    {
        return await _cityRepository.GetAllAsync();
    }

    public async Task<List<City>> GetByCountryAsync(int countryId)
    {
        return await _cityRepository.GetByCountryAsync(countryId);
    }

    public async Task<City?> GetByIdAsync(int id)
    {
        return await _cityRepository.GetByIdAsync(id);
    }

    public async Task<City?> GetByNameAsync(string name, int countryId)
    {
        return await _cityRepository.GetByNameAsync(name, countryId);
    }

    public async Task<int> GetOrCreateCityAsync(string name, int countryId)
    {
        name = name.Trim();

        var city = await _cityRepository.GetByNameAsync(name, countryId);

        if (city != null)
            return city.Id;

        await _cityRepository.AddAsync(new City
        {
            Name = name,
            CountryId = countryId
        });

        city = await _cityRepository.GetByNameAsync(name, countryId);

        return city!.Id;
    }

    public async Task AddAsync(City city)
    {
        await _cityRepository.AddAsync(city);
    }

    public async Task UpdateAsync(City city)
    {
        await _cityRepository.UpdateAsync(city);
    }

    public async Task DeleteAsync(int id)
    {
        await _cityRepository.DeleteAsync(id);
    }
}