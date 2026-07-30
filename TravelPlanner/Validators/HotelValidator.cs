using FluentValidation;
using TravelPlanner.Models;

namespace TravelPlanner.Validators;

public class HotelValidator : AbstractValidator<Hotel>
{
    public HotelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Назва готелю обов'язкова.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Адреса обов'язкова.");

        RuleFor(x => x.Stars)
            .InclusiveBetween(1, 5)
            .WithMessage("Кількість зірок повинна бути від 1 до 5.");

        RuleFor(x => x.Phone)
            .Matches(@"^\+?[0-9]{10,15}$")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("Номер телефону повинен містити від 10 до 15 цифр.");
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Некоректна електронна адреса.");

        RuleFor(x => x.Website)
            .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrWhiteSpace(x.Website))
            .WithMessage("Некоректна адреса сайту.");

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0)
            .WithMessage("Ціна повинна бути більшою за 0.");
    }
}