using Microsoft.Data.Sqlite;

namespace TravelPlanner.Data;

public class DatabaseContext
{
    private const string ConnectionString = "Data Source=TravelPlanner.db";

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(ConnectionString);
    }
}