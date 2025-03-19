using Application.DTOs.AuthDTOs;
using FluentValidation;
using System.Globalization;

namespace Application.Validations;

public class UserValidator : AbstractValidator<UserDTO>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Matches("^[a-zA-Z]+$").WithMessage("First name must contain only alphabetical characters.")
            .MaximumLength(20).WithMessage("First name must not exceed 20 characters.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Matches("^[a-zA-Z\\s]+$").WithMessage("Last name must contain only alphabetical characters and spaces.")
            .MaximumLength(20).WithMessage("Last name must not exceed 20 characters.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.");






    }
    private bool BeAValidDate(DateTime date)
    {
        string dateString = date.ToString("dd/MM/yyyy");
        string[] formats = { "dd/MM/yyyy" };
        return DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
    private bool IsOlderThan(DateTime dob, int age)
    {
        var today = DateTime.Today;
        var calculatedAge = today.Year - dob.Year;
        if (dob.Date > today.AddYears(-calculatedAge)) calculatedAge--;
        return calculatedAge >= age;
    }
}