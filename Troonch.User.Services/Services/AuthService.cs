using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Troonch.User.Domain.DTOs.Requests;
using Troonch.User.Domain.Entities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Troonch.User.Application.Services;

public class AuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly IEmailSender _emailSender;
    private readonly IUrlHelper _urlHelper;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IValidator<LoginRequestDTO> _validator;

    public AuthService(
        ILogger<AuthService> logger,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor,
        IValidator<LoginRequestDTO> validator
    )
    {
        _logger = logger;
        _emailSender = emailSender;
        _signInManager = signInManager;
        _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        _validator = validator;
    }

   

    public async Task<SignInResult?> LoginUserAsync(LoginRequestDTO loginRequest)
    {
        if(loginRequest is null)
        {
            _logger.LogError("AuthService::LoginUserAsync loginRequest is null");
            throw new ArgumentNullException(nameof(loginRequest));
        }

        await _validator.ValidateAndThrowAsync(loginRequest);

        var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, loginRequest.RememberMe, lockoutOnFailure: false);

        return result;
    }
}
