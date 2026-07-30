using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Controllers;
using TravelPlanner.Data;
using TravelPlanner.Menus;
using TravelPlanner.Menus.Expenses;
using TravelPlanner.Menus.Hotels;
using TravelPlanner.Menus.Reports;
using TravelPlanner.Repositories;
using TravelPlanner.Services;
using TravelPlanner.Validators;

namespace TravelPlanner.Configuration;

public static class DependencyInjection
{
    public static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Database
        services.AddSingleton<DatabaseContext>();

        // Validators
        services.AddScoped<HotelValidator>();
        services.AddScoped<TripValidator>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ITripRepository, TripRepository>();
        services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IHotelBookingRepository, HotelBookingRepository>();

        // Controllers
        services.AddScoped<TripController>();

        // Services
        services.AddSingleton<CurrentUserService>();

        services.AddScoped<AuthenticationService>();
        services.AddScoped<StartupService>();
        services.AddScoped<TripService>();
        services.AddScoped<ExpenseService>();
        services.AddScoped<CountryService>();
        services.AddScoped<CityService>();
        services.AddScoped<HotelService>();
        services.AddScoped<HotelBookingService>();

        services.AddScoped<PdfReportService>();
        services.AddScoped<ExcelReportService>();

        // Menus
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

        services.AddScoped<HotelMenu>();
        services.AddScoped<CreateHotelMenu>();
        services.AddScoped<EditHotelMenu>();
        services.AddScoped<DeleteHotelMenu>();
        services.AddScoped<BookHotelMenu>();
        services.AddScoped<CancelHotelBookingMenu>();

        services.AddScoped<ExpenseMenu>();
        services.AddScoped<CreateExpenseMenu>();
        services.AddScoped<ViewExpensesMenu>();
        services.AddScoped<ShowExpensesMenu>();
        services.AddScoped<MyExpensesMenu>();
        services.AddScoped<EditExpenseMenu>();
        services.AddScoped<DeleteExpenseMenu>();

        services.AddScoped<ReportMenu>();

        return services.BuildServiceProvider();
    }
}