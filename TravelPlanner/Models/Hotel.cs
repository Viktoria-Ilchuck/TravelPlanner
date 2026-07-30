namespace TravelPlanner.Models;

public class Hotel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public int Stars { get; set; }

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;

    public decimal PricePerNight { get; set; }

    public int CityId { get; set; }

    public string CityName { get; set; } = string.Empty;

    public int? TripId { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Stars}★)";
    }
}