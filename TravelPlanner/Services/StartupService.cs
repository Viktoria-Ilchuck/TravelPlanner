using TravelPlanner.Models;
using TravelPlanner.Repositories;

namespace TravelPlanner.Services;

public class StartupService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IExpenseCategoryRepository _expenseCategoryRepository;

    public StartupService(
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IExpenseCategoryRepository expenseCategoryRepository)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _expenseCategoryRepository = expenseCategoryRepository;
    }

    public async Task InitializeAsync()
    {
        await CreateRolesAsync();
        await CreateAdminAsync();
        await CreateExpenseCategoriesAsync();
    }

    private async Task CreateRolesAsync()
    {
        var adminRole = await _roleRepository.GetByNameAsync("Administrator");

        if (adminRole == null)
        {
            await _roleRepository.AddAsync(new Role
            {
                Name = "Administrator"
            });
        }

        var userRole = await _roleRepository.GetByNameAsync("User");

        if (userRole == null)
        {
            await _roleRepository.AddAsync(new Role
            {
                Name = "User"
            });
        }
    }

    private async Task CreateAdminAsync()
    {
        var admin = await _userRepository.GetByLoginAsync("admin");

        if (admin != null)
            return;

        var adminRole = await _roleRepository.GetByNameAsync("Administrator");

        if (adminRole == null)
            return;

        await _userRepository.AddAsync(new User
        {
            FirstName = "System",
            LastName = "Administrator",
            Email = "admin@travelplanner.local",
            Login = "admin",
            PasswordHash = "admin123",
            RoleId = adminRole.Id,
            
        });
    }

    private async Task CreateExpenseCategoriesAsync()
    {
        string[] categories =
        {
            "Транспорт",
            "Проживання",
            "Харчування",
            "Розваги",
            "Покупки",
            "Інше"
        };

        foreach (var categoryName in categories)
        {
            var category =
                await _expenseCategoryRepository.GetByNameAsync(categoryName);

            if (category == null)
            {
                await _expenseCategoryRepository.AddAsync(
                    new ExpenseCategory
                    {
                        Name = categoryName
                    });
            }
        }
    }
}