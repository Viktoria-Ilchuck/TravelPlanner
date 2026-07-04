public class Trip
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Budget { get; set; }

    public string Status { get; set; } = "Запланована";

    public int CityId { get; set; }

    public int OwnerId { get; set; }

    public override string ToString()
    {
        return $"{Title} ({StartDate:dd.MM.yyyy})";
    }
}