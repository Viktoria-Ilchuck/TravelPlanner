using TravelPlanner.Models;
using TravelPlanner.Repositories;
using TravelPlanner.Helpers;

namespace TravelPlanner.Services;

public class AuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public AuthenticationService(
        IUserRepository userRepository,
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<User?> LoginAsync(string login, string password)
    {
        var user = await _userRepository.GetByLoginAsync(login);

        if (user == null)
            return null;

        if (!PasswordHasher.Verify(password, user.PasswordHash))
            return null;

        return user;
    }

    public async Task<string?> RegisterAsync(
        string firstName,
        string lastName,
        string email,
        string login,
        string password)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return "Введіть ім'я.";

        if (string.IsNullOrWhiteSpace(lastName))
            return "Введіть прізвище.";

        if (string.IsNullOrWhiteSpace(login))
            return "Введіть логін.";

        if (string.IsNullOrWhiteSpace(password))
            return "Введіть пароль.";

        if (string.IsNullOrWhiteSpace(email))
            return "Введіть Email.";

        firstName = firstName.Trim();
        lastName = lastName.Trim();
        email = email.Trim();
        login = login.Trim();
        
        if (await _userRepository.GetByLoginAsync(login) != null)
            return "Користувач з таким логіном вже існує.";

        if (await _userRepository.GetByEmailAsync(email) != null)
            return "Користувач з таким Email вже існує.";

        var role = await _roleRepository.GetByNameAsync("User");

        if (role == null)
            return "Не знайдено роль User.";

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Login = login,
            PasswordHash = PasswordHasher.Hash(password),
            RoleId = role.Id,
            CreatedAt = DateTime.Now
        };

        await _userRepository.AddAsync(user);

        return null;
    }
}