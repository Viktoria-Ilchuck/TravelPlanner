using FluentValidation;
using TravelPlanner.Models;

namespace TravelPlanner.Validators;

public class TripValidator : AbstractValidator<Trip>
{
    public TripValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Введіть назву подорожі.");

        RuleFor(x => x.Budget)
            .GreaterThan(0)
            .WithMessage("Бюджет повинен бути більшим за 0.");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("Дата початку повинна бути раніше дати завершення.");
    }
}