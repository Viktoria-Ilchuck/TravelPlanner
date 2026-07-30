using Spectre.Console;
using TravelPlanner.DTO;
using TravelPlanner.Services;

namespace TravelPlanner.Menus.Reports;

public class ReportMenu
{
    private readonly TripService _tripService;
    private readonly ExpenseService _expenseService;
    private readonly HotelBookingService _hotelBookingService;
    private readonly PdfReportService _pdfReportService;
    private readonly ExcelReportService _excelReportService;
    private readonly CurrentUserService _currentUser;

    public ReportMenu(
        TripService tripService,
        ExpenseService expenseService,
        HotelBookingService hotelBookingService,
        PdfReportService pdfReportService,
        ExcelReportService excelReportService,
        CurrentUserService currentUser)
    {
        _tripService = tripService;
        _expenseService = expenseService;
        _hotelBookingService = hotelBookingService;
        _pdfReportService = pdfReportService;
        _excelReportService = excelReportService;
        _currentUser = currentUser;
    }

    public async Task ShowAsync()
    {
        Console.Clear();

        var trips = await _tripService.GetDetailedTripsAsync(
            _currentUser.CurrentUser!.Id);

        if (!trips.Any())
        {
            AnsiConsole.MarkupLine("[red]У вас немає подорожей.[/]");
            Console.ReadKey();
            return;
        }

        var trip = AnsiConsole.Prompt(
            new SelectionPrompt<TripDto>()
                .Title("Оберіть подорож")
                .UseConverter(x => $"{x.Title} ({x.City})")
                .AddChoices(trips));

        var expenses = await _expenseService.GetByTripAsync(trip.Id);
        var bookings = await _hotelBookingService.GetByTripAsync(trip.Id);

        var reportType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Який звіт створити?")
                .AddChoices(
                    "PDF",
                    "Excel"));

        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "TravelReports");

        Directory.CreateDirectory(folder);

        var extension = reportType == "PDF"
            ? "pdf"
            : "xlsx";

        var fileName =
            $"{trip.Title.Replace(" ", "_")}_{DateTime.Now:yyyyMMddHHmmss}.{extension}";

        var filePath = Path.Combine(folder, fileName);

        if (reportType == "PDF")
        {
            _pdfReportService.CreateTripReport(
                trip,
                expenses,
                bookings,
                filePath);
        }
        else
        {
            _excelReportService.CreateTripReport(
                trip,
                expenses,
                bookings,
                filePath);
        }

        AnsiConsole.MarkupLine($"[green]{reportType} звіт успішно створено![/]");
        AnsiConsole.MarkupLine($"[yellow]{filePath}[/]");

        Console.ReadKey();
    }
}