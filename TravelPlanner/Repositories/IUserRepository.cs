using TravelPlanner.Models;

namespace TravelPlanner.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();

    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByLoginAsync(string login);

    Task<User?> GetByEmailAsync(string email);

    Task AddAsync(User user);

    Task UpdateAsync(User user);

    Task DeleteAsync(int id);
}