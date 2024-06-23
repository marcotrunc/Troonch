using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Troonch.User.Domain.DTOs.Requests;
using Troonch.User.Domain.Entities;

namespace Troonch.User.Application.Validators;

public class ChangePasswordReqValidator : AbstractValidator<ChangePasswordRequestDTO>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangePasswordReqValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;

        RuleFor(l => l.OldPassword)
            .NotEmpty()
                .WithMessage("The old password can't be empty")
            .NotNull()
                .WithMessage("The old password can't be null")
            .MustAsync(async (_, oldPassword, CancellationToken) =>
            {
                if (String.IsNullOrWhiteSpace(oldPassword))
                {
                    return false;
                }

                if (String.IsNullOrWhiteSpace(_.Email))
                {
                    return false;
                }

                var applicationUser = await _userManager.FindByNameAsync(_.Email);

                if (applicationUser is null)
                {
                    return false;
                }

                return await _userManager.CheckPasswordAsync(applicationUser, oldPassword);
            })
                .WithMessage("The old password is wrong");


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
