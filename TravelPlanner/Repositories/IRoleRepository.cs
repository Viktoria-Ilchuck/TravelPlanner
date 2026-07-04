using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public interface IRoleRepository
{
    Task<List<Role>> GetAllAsync();

    Task<Role?> GetByIdAsync(int id);

    Task<Role?> GetByNameAsync(string name);

    Task AddAsync(Role role);
}