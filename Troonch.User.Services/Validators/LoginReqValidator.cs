using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Troonch.User.Domain.DTOs.Requests;
using Troonch.User.Domain.Entities;
namespace Troonch.User.Application.Validators;

public class LoginReqValidator : AbstractValidator<LoginRequestDTO>
{
    private readonly IUserStore<ApplicationUser> _userStore;

    public LoginReqValidator(
        IUserStore<ApplicationUser> userStore
        )
    {
        _userStore = userStore;

        RuleFor(l => l.Email)
            .NotEmpty()
                .WithMessage("The email can't be empty")
            .EmailAddress()
                .WithMessage("The email is incorrectly");

        RuleFor(l => l.Password)
            .NotEmpty()
                .WithMessage("The password can't be empty")
            .NotNull()
                .WithMessage("The password can't be null");
    }
}
