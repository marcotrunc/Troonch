using FluentValidation;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Troonch.Application.Base.Interfaces;
using Troonch.User.Domain.DTOs.Requests;
using Troonch.User.Domain.Entities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Troonch.User.Application.Services;

public class AuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly IEmailSender _emailSender;
    private readonly IUrlHelper _urlHelper;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IValidator<LoginRequestDTO> _validator;

    public AuthService(
            ILogger<AuthService> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
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
        _userManager = userManager;
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

    public async Task<bool> HandleTwoFactorAuthenticationAsync(string userId, bool enabled)
    {
        if (String.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentNullException(nameof(userId));
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) 
        {
            throw new ArgumentNullException(nameof(user));
        }
        
        var result = await _userManager.SetTwoFactorEnabledAsync(user, enabled);

        if (result is null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        if (!result.Succeeded)
        {
            return false;
        }

        if (enabled)
        {
            await _emailSender.SendEmailAsync(user.Email, "Troonch - Abilitazione Autenticazione a due fattori",
                $@"La tua organizzazione richiede che il prossimo accesso verrà gestito tramite autenticazione a due fattori,
                    quindi per poter accedere ti sarà inviata una mail con il codice otp su {user.Email}.   
                ");
        }
        else
        {
            await _emailSender.SendEmailAsync(user.Email, "Troonch - Disabilitazione Autenticazione a due fattori",
                    $@"La tua organizzazione ha disabilitato l'autenticazione a due fattori per il tuo account.   
                    ");
        }

        return true;
    }
}
