using Dapper;
using TravelPlanner.Data;
using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        var users = await connection.QueryAsync<User>(
            """
            SELECT *
            FROM Users
            ORDER BY LastName
            """
        );

        return users.ToList();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT *
            FROM Users
            WHERE Id = @Id
            """,
            new { Id = id });
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT *
            FROM Users
            WHERE Login = @Login
            """,
            new { Login = login });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT *
            FROM Users
            WHERE Email = @Email
            """,
            new { Email = email });
    }

    public async Task AddAsync(User user)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            """
            INSERT INTO Users
            (
                FirstName,
                LastName,
                Email,
                Login,
                PasswordHash,
                RoleId,
                RememberToken,
                RememberUntil,
                CreatedAt
            )
            VALUES
            (
                @FirstName,
                @LastName,
                @Email,
                @Login,
                @PasswordHash,
                @RoleId,
                @RememberToken,
                @RememberUntil,
                @CreatedAt
            )
            """,
            user);
    }

    public async Task UpdateAsync(User user)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            """
            UPDATE Users
            SET
                FirstName = @FirstName,
                LastName = @LastName,
                Email = @Email,
                Login = @Login,
                PasswordHash = @PasswordHash,
                RoleId = @RoleId,
                RememberToken = @RememberToken,
                RememberUntil = @RememberUntil
            WHERE Id = @Id
            """,
            user);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            """
            DELETE FROM Users
            WHERE Id = @Id
            """,
            new { Id = id });
    }
}