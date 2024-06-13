using FluentValidation;
using Troonch.User.Domain.DTOs.Requests;

namespace Troonch.User.Application.Validators;

public class UserReqValidator : AbstractValidator<UserRequestDTO>
{
    public UserReqValidator()
    {
        RuleFor(u => u.Email)
            .NotNull()
                .WithMessage("The email can't be null")
            .NotEmpty()
                .WithMessage("The email can't be empty")
             .EmailAddress()
                .WithMessage("The email inserted is not valid")
            .MaximumLength(320)
                .WithMessage("The email can't have more than 320 characters")
            .MinimumLength(3)
                .WithMessage("The email can't have minus than 3 characters");

        RuleFor(u => u.PhoneNumber)
            .NotNull()
                .WithMessage("The phone number can't be null")
            .NotEmpty()
                .WithMessage("The phone number can't be empty")
            .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Phone number is not valid.");

        RuleFor(u => u.DateOfBirth)
            .NotEmpty()
                .WithMessage("The date can not be empty")
            .NotNull()
                .WithMessage("The date can not be null")
            .Must(u => BeAtLeast16YearsOld(u.Value))
                .When(u => u.DateOfBirth.HasValue || u.DateOfBirth is not null)
            .WithMessage("The user must be at least 16 years old.");

        RuleFor(u => u.Name)
            .NotNull()
                .WithMessage("The name can't be null")
            .NotEmpty()
                .WithMessage("The name can't be empty")
            .MinimumLength(1)
                .WithMessage("The name can't have less then 1 character")
            .MaximumLength(128)
                .WithMessage("The name can't have more then 128 character");


        RuleFor(u => u.LastName)
            .NotNull()
                .WithMessage("The lastname can't be null")
            .NotEmpty()
                .WithMessage("The lastname can't be empty")
            .MinimumLength(1)
                .WithMessage("The lastname can't have less then 1 character")
            .MaximumLength(128)
                .WithMessage("The lastname can't have more then 128 character");
    }

    private bool BeAtLeast16YearsOld(DateOnly dateOfBirth)
    {
        var currentYear = DateTime.UtcNow.Year;
        var age = currentYear - dateOfBirth.Year;
        return age >= 16;
    }
}
