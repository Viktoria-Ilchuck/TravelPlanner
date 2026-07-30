using TravelPlanner.Models;
using TravelPlanner.Services;

namespace TravelPlanner.Menus.Hotels;

public class EditHotelMenu
{
    private readonly HotelService _hotelService;

    public EditHotelMenu(HotelService hotelService)
    {
        _hotelService = hotelService;
    }

    public async Task ShowAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Edit Hotel ===");
        Console.WriteLine();

        Console.Write("Hotel Id: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid Id.");
            Console.ReadKey();
            return;
        }

        var hotel = await _hotelService.GetByIdAsync(id);

        if (hotel == null)
        {
            Console.WriteLine("Hotel not found.");
            Console.ReadKey();
            return;
        }

        Console.Write($"Name ({hotel.Name}): ");
        var name = Console.ReadLine();

        Console.Write($"Address ({hotel.Address}): ");
        var address = Console.ReadLine();

        Console.Write($"Stars ({hotel.Stars}): ");
        var starsText = Console.ReadLine();

        Console.Write($"Phone ({hotel.Phone}): ");
        var phone = Console.ReadLine();

        Console.Write($"Email ({hotel.Email}): ");
        var email = Console.ReadLine();

        Console.Write($"Website ({hotel.Website}): ");
        var website = Console.ReadLine();

        Console.Write($"Price ({hotel.PricePerNight}): ");
        var priceText = Console.ReadLine();

        Console.Write($"City Id ({hotel.CityId}): ");
        var cityText = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(name))
            hotel.Name = name;

        if (!string.IsNullOrWhiteSpace(address))
            hotel.Address = address;

        if (int.TryParse(starsText, out int stars))
            hotel.Stars = stars;

        if (!string.IsNullOrWhiteSpace(phone))
            hotel.Phone = phone;

        if (!string.IsNullOrWhiteSpace(email))
            hotel.Email = email;

        if (!string.IsNullOrWhiteSpace(website))
            hotel.Website = website;

        if (decimal.TryParse(priceText, out decimal price))
            hotel.PricePerNight = price;

        if (int.TryParse(cityText, out int cityId))
            hotel.CityId = cityId;

        await _hotelService.UpdateAsync(hotel);

        Console.WriteLine();
        Console.WriteLine("Hotel updated successfully.");

        Console.ReadKey();
    }
}