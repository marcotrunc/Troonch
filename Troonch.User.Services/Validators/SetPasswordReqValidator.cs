using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Troonch.User.Domain.DTOs.Requests;
using Troonch.User.Domain.Entities;

namespace Troonch.User.Application.Validators;

public class SetPasswordReqValidator : AbstractValidator<SetPasswordRequestDTO>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public SetPasswordReqValidator(
            UserManager<ApplicationUser> userManager
    )
    {
        _userManager = userManager;

        RuleFor(sp => sp.Id)
            .Must(Id => !String.IsNullOrWhiteSpace(Id))
                .WithMessage("Identifier empty in setting password (first time) page, contact administrator")
            .MustAsync(async (_, Id, CancellationToken) => await _userManager.FindByIdAsync(Id) is not null)
                .WithMessage("The user not exist");
            

        RuleFor(sp => sp.NewPassword)
            .NotEmpty()
                .WithMessage("Password is required.")
            .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
            .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
                .WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password must contain at least one special character.");

        RuleFor(sp => sp.ConfirmPassword)
            .NotEmpty()
                .WithMessage("Confirm Password is required")
            .Must((_, confirmPassword) => confirmPassword.Equals(_.NewPassword))
                .WithMessage("New Password and Confirm Password must be equals");
    }
}
