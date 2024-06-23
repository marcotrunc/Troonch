using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Troonch.User.Domain.DTOs.Requests;
using Troonch.User.Domain.Entities;
namespace Troonch.User.Application.Validators;

public class LoginReqValidator : AbstractValidator<LoginRequestDTO>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginReqValidator(
                    UserManager<ApplicationUser> userManager
    )
    {
        _userManager = userManager;

        RuleFor(l => l.Email)
            .NotEmpty()
                .WithMessage("The email can't be empty")
            .EmailAddress()
                .WithMessage("The email is incorrectly")
            .MustAsync(async (_, email, CancellationToken) =>
            {
                if (String.IsNullOrWhiteSpace(email))
                {
                    return false;
                }

                var applicationUser = await _userManager.FindByNameAsync(email);

                if (applicationUser is null)
                {
                    return false;
                }
                return true;
            })
                .WithMessage("This user not existing");

        RuleFor(l => l.Password)
            .NotEmpty()
                .WithMessage("The password can't be empty")
            .NotNull()
                .WithMessage("The password can't be null")
            .MustAsync(async (_, password, CancellationToken) =>
            {
                if (String.IsNullOrWhiteSpace(password))
                {
                    return false;
                }

                var applicationUser = await _userManager.FindByNameAsync(_.Email);

                if (applicationUser is null)
                {
                    return false;
                }
                
                return await _userManager.CheckPasswordAsync(applicationUser, password);
            })
                .WithMessage("The password is wrong");
            
    }
}
