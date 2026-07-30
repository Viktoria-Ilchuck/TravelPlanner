using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TravelPlanner.DTO;
using TravelPlanner.Models;

namespace TravelPlanner.Services;

public class PdfReportService
{
    public void CreateTripReport(
        TripDto trip,
        List<Expense> expenses,
        List<HotelBooking> bookings,
        string filePath)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        decimal totalExpenses = expenses.Sum(x => x.Amount);
        decimal balance = trip.Budget - totalExpenses;

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                page.Header()
                    .Text("Travel Planner Report")
                    .FontSize(24)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);

                page.Content().Column(column =>
                {
                    column.Spacing(15);

                    column.Item().Text(trip.Title)
                        .FontSize(20)
                        .Bold();

                    column.Item().Text($"Опис: {trip.Description}");
                    column.Item().Text($"Країна: {trip.Country}");
                    column.Item().Text($"Місто: {trip.City}");
                    column.Item().Text($"Дата: {trip.StartDate:dd.MM.yyyy} - {trip.EndDate:dd.MM.yyyy}");
                    column.Item().Text($"Статус: {trip.Status}");
                    column.Item().Text($"Бюджет: {trip.Budget:F2} грн");
                    column.Item().Text($"Витрачено: {totalExpenses:F2} грн");

                    column.Item()
                        .Text($"Залишок: {balance:F2} грн")
                        .Bold()
                        .FontColor(balance >= 0
                            ? Colors.Green.Darken2
                            : Colors.Red.Darken2);

                    column.Item().PaddingTop(15);

                    column.Item()
                        .Text("Витрати")
                        .Bold()
                        .FontSize(18);

                    if (!expenses.Any())
                    {
                        column.Item().Text("Витрат немає.");
                    }
                    else
                    {
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(5);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Дата").Bold();
                                header.Cell().Text("Опис").Bold();
                                header.Cell().Text("Сума").Bold();
                                header.Cell().Text("Валюта").Bold();
                            });

                            foreach (var expense in expenses)
                            {
                                table.Cell().Text(expense.Date.ToString("dd.MM.yyyy"));
                                table.Cell().Text(expense.Description);
                                table.Cell().Text($"{expense.Amount:F2}");
                                table.Cell().Text(expense.Currency);
                            }
                        });
                    }

                    column.Item().PaddingTop(15);

                    column.Item()
                        .Text("Бронювання")
                        .Bold()
                        .FontSize(18);

                    if (!bookings.Any())
                    {
                        column.Item().Text("Бронювань немає.");
                    }
                    else
                    {
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Готель
                                columns.RelativeColumn(2); // Заїзд
                                columns.RelativeColumn(2); // Виїзд
                                columns.RelativeColumn(1); // Гостей
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Готель").Bold();
                                header.Cell().Text("Заїзд").Bold();
                                header.Cell().Text("Виїзд").Bold();
                                header.Cell().Text("Гостей").Bold();
                            });

                            foreach (var booking in bookings)
                            {
                                table.Cell().Text(booking.Hotel?.Name ?? "Невідомо");
                                table.Cell().Text(booking.CheckIn.ToString("dd.MM.yyyy"));
                                table.Cell().Text(booking.CheckOut.ToString("dd.MM.yyyy"));
                                table.Cell().Text(booking.Guests.ToString());
                            }
                        });
                    }
                });

                page.Footer()
                    .AlignCenter()
                    .Text($"Створено {DateTime.Now:dd.MM.yyyy HH:mm}");
            });
        })
        .GeneratePdf(filePath);
    }
}