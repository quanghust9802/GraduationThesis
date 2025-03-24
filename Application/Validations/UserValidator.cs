using Application.DTOs.AuthDTOs;
using FluentValidation;
using System.Globalization;

namespace Application.Validations;

public class UserValidator : AbstractValidator<UserDTO>
{
    public UserValidator()
    {
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