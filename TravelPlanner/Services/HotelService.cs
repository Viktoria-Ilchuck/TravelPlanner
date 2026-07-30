using TravelPlanner.Models;
using TravelPlanner.Repositories;

namespace TravelPlanner.Services;

public class HotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly CityService _cityService;

    public HotelService(
        IHotelRepository hotelRepository,
        CityService cityService)
    {
        _hotelRepository = hotelRepository;
        _cityService = cityService;
    }

    public async Task<List<Hotel>> GetAllAsync()
    {
        return await _hotelRepository.GetAllAsync();
    }

    public async Task<List<Hotel>> SearchAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new List<Hotel>();

        return await _hotelRepository.SearchAsync(name.Trim());
    }

    public async Task<List<Hotel>> GetByStarsAsync(int stars)
    {
        return await _hotelRepository.GetByStarsAsync(stars);
    }

    public async Task<List<Hotel>> GetSortedByNameAsync()
    {
        return await _hotelRepository.GetSortedByNameAsync();
    }

    public async Task<List<Hotel>> GetSortedByPriceAsync()
    {
        return await _hotelRepository.GetSortedByPriceAsync();
    }

    public async Task AddAsync(Hotel hotel)
    {
        hotel.Name = hotel.Name.Trim();
        hotel.Address = hotel.Address.Trim();

        await _hotelRepository.AddAsync(hotel);
    }

    public async Task UpdateAsync(Hotel hotel)
    {
        hotel.Name = hotel.Name.Trim();
        hotel.Address = hotel.Address.Trim();

        await _hotelRepository.UpdateAsync(hotel);
    }

    public async Task DeleteAsync(int id)
    {
        await _hotelRepository.DeleteAsync(id);
    }

    public async Task<Hotel?> GetByIdAsync(int id)
    {
        return await _hotelRepository.GetByIdAsync(id);
    }
    
    
    public async Task<List<Hotel>> GetByCityAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return new List<Hotel>();

        return await _hotelRepository.GetByCityAsync(city.Trim());
    }
    public async Task<int> GetOrCreateCityAsync(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            throw new Exception("Введіть місто.");

        return await _cityService.GetOrCreateCityAsync(cityName.Trim());
    }
}