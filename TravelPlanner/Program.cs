using System.Text;
using TravelPlanner.Application;
using TravelPlanner.Data;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

try
{
    await DatabaseInitializer.InitializeAsync();

    var application = new Application();

    await application.RunAsync();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex);
    Console.ResetColor();

    Console.WriteLine();
    Console.WriteLine("Натисніть Enter...");
    Console.ReadLine();
}