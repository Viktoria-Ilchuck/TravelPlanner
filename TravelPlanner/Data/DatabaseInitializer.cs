using Microsoft.Data.Sqlite;

namespace TravelPlanner.Data;

public static class DatabaseInitializer
{
    private const string ConnectionString = "Data Source=TravelPlanner.db";

    public static async Task InitializeAsync()
    {
        await using var connection = new SqliteConnection(ConnectionString);
        await connection.OpenAsync();

        var sql = """
PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Roles(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS Users(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    Login TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    RoleId INTEGER NOT NULL,
    RememberToken TEXT,
    RememberUntil TEXT,
    CreatedAt TEXT NOT NULL,
    FOREIGN KEY(RoleId) REFERENCES Roles(Id)
);

CREATE TABLE IF NOT EXISTS Countries(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS Cities(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    CountryId INTEGER NOT NULL,
    FOREIGN KEY(CountryId) REFERENCES Countries(Id)
);

CREATE TABLE IF NOT EXISTS Trips(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT,
    StartDate TEXT NOT NULL,
    EndDate TEXT NOT NULL,
    Budget REAL NOT NULL,
    Status TEXT NOT NULL,
    CityId INTEGER NOT NULL,
    OwnerId INTEGER NOT NULL,
    FOREIGN KEY(CityId) REFERENCES Cities(Id),
    FOREIGN KEY(OwnerId) REFERENCES Users(Id)
);

CREATE TABLE IF NOT EXISTS Hotels(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Address TEXT NOT NULL,
    Stars INTEGER NOT NULL,
    Phone TEXT NOT NULL,
    Email TEXT NOT NULL,
    Website TEXT NOT NULL,
    PricePerNight REAL NOT NULL,
    CityId INTEGER NOT NULL,
    TripId INTEGER,
    FOREIGN KEY(CityId) REFERENCES Cities(Id),
    FOREIGN KEY(TripId) REFERENCES Trips(Id)
);

CREATE TABLE IF NOT EXISTS HotelBookings
(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,

    HotelId INTEGER NOT NULL,

    TripId INTEGER NOT NULL,

    CheckIn TEXT NOT NULL,

    CheckOut TEXT NOT NULL,

    Guests INTEGER NOT NULL,

    FOREIGN KEY(HotelId) REFERENCES Hotels(Id),

    FOREIGN KEY(TripId) REFERENCES Trips(Id)
);

CREATE TABLE IF NOT EXISTS Activities(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT,
    Location TEXT,
    Date TEXT NOT NULL,
    StartTime TEXT,
    EndTime TEXT,
    Price REAL NOT NULL,
    TripId INTEGER NOT NULL,
    FOREIGN KEY(TripId) REFERENCES Trips(Id)
);

CREATE TABLE IF NOT EXISTS ExpenseCategories(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS Expenses(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Amount REAL NOT NULL,
    Currency TEXT NOT NULL,
    Date TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    Description TEXT,
    CategoryId INTEGER NOT NULL,
    TripId INTEGER NOT NULL,
    FOREIGN KEY(CategoryId) REFERENCES ExpenseCategories(Id),
    FOREIGN KEY(TripId) REFERENCES Trips(Id)
);

CREATE TABLE IF NOT EXISTS TripParticipants(
    TripId INTEGER NOT NULL,
    UserId INTEGER NOT NULL,
    PRIMARY KEY(TripId, UserId),
    FOREIGN KEY(TripId) REFERENCES Trips(Id),
    FOREIGN KEY(UserId) REFERENCES Users(Id)
);
""";

        var command = connection.CreateCommand();
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync();

        command.CommandText = """
INSERT INTO Roles(Id, Name)
VALUES
(1, 'Administrator'),
(2, 'User')
ON CONFLICT(Id) DO NOTHING;
""";
        await command.ExecuteNonQueryAsync();

        command.CommandText = """
INSERT INTO Countries(Id, Name)
VALUES
(1, 'Україна'),
(2, 'Канада'),
(3, 'США')
ON CONFLICT(Id) DO NOTHING;
""";
        await command.ExecuteNonQueryAsync();

        command.CommandText = """
INSERT INTO Cities(Id, Name, CountryId)
VALUES
(1, 'Київ', 1),
(2, 'Львів', 1),
(3, 'Торонто', 2),
(4, 'Ванкувер', 2),
(5, 'Монреаль', 2),
(6, 'Нью-Йорк', 3)
ON CONFLICT(Id) DO NOTHING;
""";
        await command.ExecuteNonQueryAsync();

        command.CommandText = """
INSERT INTO ExpenseCategories(Id, Name)
VALUES
(1, 'Харчування'),
(2, 'Транспорт'),
(3, 'Готель'),
(4, 'Розваги'),
(5, 'Покупки'),
(6, 'Інше')
ON CONFLICT(Id) DO NOTHING;
""";
        await command.ExecuteNonQueryAsync();
    }
}