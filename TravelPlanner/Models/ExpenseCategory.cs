namespace TravelPlanner.Models;

public class ExpenseCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return Name;
    }
}