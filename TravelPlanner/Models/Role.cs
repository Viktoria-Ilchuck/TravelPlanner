namespace TravelPlanner.Models;

public class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return Name;
    }
}