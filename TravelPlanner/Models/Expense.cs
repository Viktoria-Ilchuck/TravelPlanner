namespace TravelPlanner.Models;

public class Expense
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "UAH";

    public DateTime Date { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string Description { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public int TripId { get; set; }
}