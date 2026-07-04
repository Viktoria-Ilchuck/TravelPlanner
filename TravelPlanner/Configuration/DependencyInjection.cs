using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Data;
using TravelPlanner.Repositories;
using TravelPlanner.Services;
using TravelPlanner.Menus;
using TravelPlanner.Controllers;


namespace TravelPlanner.Configuration;

public static class DependencyInjection
{
    public static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<DatabaseContext>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ITripRepository, TripRepository>();
        services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();

        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICityRepository, CityRepository>();

        // Services
        services.AddScoped<MainMenu>();
        services.AddScoped<LoginMenu>();
        services.AddScoped<RegisterMenu>();
        services.AddScoped<UserMenu>();
        services.AddScoped<AdminMenu>();
        services.AddScoped<TripMenu>();
        services.AddScoped<CreateTripMenu>();
        services.AddScoped<MyTripsMenu>();
        services.AddScoped<EditTripMenu>();
        services.AddScoped<DeleteTripMenu>();
        services.AddScoped<TripController>();
        services.AddSingleton<CurrentUserService>();
        services.AddScoped<AuthenticationService>();
        services.AddScoped<StartupService>();
        services.AddScoped<TripService>();
        services.AddScoped<CountryService>();
        services.AddScoped<CityService>();
        services.AddScoped<ExpenseService>();
        

        return services.BuildServiceProvider();
    }
}