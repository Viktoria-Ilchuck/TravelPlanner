namespace TravelPlanner.Models;

public class HotelBooking
{
    public int Id { get; set; }

    public int HotelId { get; set; }

    public int TripId { get; set; }

    public DateTime CheckIn { get; set; }

    public DateTime CheckOut { get; set; }

    public int Guests { get; set; }

    public Hotel? Hotel { get; set; }
}