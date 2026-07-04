using Dapper;
using TravelPlanner.Data;
using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DatabaseContext _context;

    public RoleRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           SELECT *
                           FROM Roles
                           ORDER BY Name
                           """;

        var roles = await connection.QueryAsync<Role>(sql);

        return roles.ToList();
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           SELECT *
                           FROM Roles
                           WHERE Id=@Id
                           """;

        return await connection.QueryFirstOrDefaultAsync<Role>(sql, new
        {
            Id = id
        });
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           SELECT *
                           FROM Roles
                           WHERE Name=@Name
                           """;

        return await connection.QueryFirstOrDefaultAsync<Role>(sql, new
        {
            Name = name
        });
    }

    public async Task AddAsync(Role role)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                           INSERT INTO Roles(Name)
                           VALUES(@Name)
                           """;

        await connection.ExecuteAsync(sql, role);
    }
}