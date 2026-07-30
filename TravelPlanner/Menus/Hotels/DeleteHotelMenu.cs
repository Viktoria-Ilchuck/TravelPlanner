using TravelPlanner.Services;

namespace TravelPlanner.Menus.Hotels;

public class DeleteHotelMenu
{
    private readonly HotelService _hotelService;

    public DeleteHotelMenu(HotelService hotelService)
    {
        _hotelService = hotelService;
    }

    public async Task ShowAsync()
    {
        Console.Clear();

        Console.WriteLine("=== Delete Hotel ===");
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

        Console.WriteLine();
        Console.WriteLine($"Delete hotel \"{hotel.Name}\" ? (Y/N)");

        var key = Console.ReadKey();

        if (char.ToUpper(key.KeyChar) != 'Y')
            return;

        await _hotelService.DeleteAsync(id);

        Console.WriteLine();
        Console.WriteLine("Hotel deleted successfully.");

        Console.ReadKey();
    }
}