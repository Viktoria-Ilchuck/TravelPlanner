using ClosedXML.Excel;
using TravelPlanner.DTO;
using TravelPlanner.Models;

namespace TravelPlanner.Services;

public class ExcelReportService
{
    public void CreateTripReport(
        TripDto trip,
        List<Expense> expenses,
        List<HotelBooking> bookings,
        string filePath)
    {
        using var workbook = new XLWorkbook();

        var sheet = workbook.Worksheets.Add("Trip");

        int row = 1;

        sheet.Cell(row++, 1).Value = "Travel Planner Report";

        row++;

        sheet.Cell(row++, 1).Value = "Назва";
        sheet.Cell(row - 1, 2).Value = trip.Title;

        sheet.Cell(row++, 1).Value = "Опис";
        sheet.Cell(row - 1, 2).Value = trip.Description;

        sheet.Cell(row++, 1).Value = "Країна";
        sheet.Cell(row - 1, 2).Value = trip.Country;

        sheet.Cell(row++, 1).Value = "Місто";
        sheet.Cell(row - 1, 2).Value = trip.City;

        sheet.Cell(row++, 1).Value = "Початок";
        sheet.Cell(row - 1, 2).Value =
            trip.StartDate.ToString("dd.MM.yyyy");

        sheet.Cell(row++, 1).Value = "Кінець";
        sheet.Cell(row - 1, 2).Value =
            trip.EndDate.ToString("dd.MM.yyyy");

        sheet.Cell(row++, 1).Value = "Бюджет";
        sheet.Cell(row - 1, 2).Value = trip.Budget;

        row += 2;

        sheet.Cell(row++, 1).Value = "Витрати";

        sheet.Cell(row, 1).Value = "Дата";
        sheet.Cell(row, 2).Value = "Опис";
        sheet.Cell(row, 3).Value = "Сума";
        sheet.Cell(row, 4).Value = "Валюта";

        row++;

        foreach (var expense in expenses)
        {
            sheet.Cell(row, 1).Value =
                expense.Date.ToString("dd.MM.yyyy");

            sheet.Cell(row, 2).Value =
                expense.Description;

            sheet.Cell(row, 3).Value =
                expense.Amount;

            sheet.Cell(row, 4).Value =
                expense.Currency;

            row++;
        }

        row++;

        sheet.Cell(row++, 1).Value = "Бронювання";

        sheet.Cell(row, 1).Value = "Готель";
        sheet.Cell(row, 2).Value = "Заїзд";
        sheet.Cell(row, 3).Value = "Виїзд";
        sheet.Cell(row, 4).Value = "Гостей";

        row++;

        foreach (var booking in bookings)
        {
            sheet.Cell(row, 1).Value =
                booking.Hotel?.Name ?? "";

            sheet.Cell(row, 2).Value =
                booking.CheckIn.ToString("dd.MM.yyyy");

            sheet.Cell(row, 3).Value =
                booking.CheckOut.ToString("dd.MM.yyyy");

            sheet.Cell(row, 4).Value =
                booking.Guests;

            row++;
        }

        sheet.Columns().AdjustToContents();

        workbook.SaveAs(filePath);
    }
}