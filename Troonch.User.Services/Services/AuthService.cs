using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Troonch.User.Domain.DTOs.Requests;
using Troonch.User.Domain.Entities;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

    public AuthService(
        ILogger<AuthService> logger,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor
    )
    {
        _logger = logger;
        _emailSender = emailSender;
        _signInManager = signInManager;
        _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
    }

   

    public async Task<SignInResult?> LoginUserAsync(LoginRequestDTO loginRequest)
    {
        if(loginRequest is null)
        {
            _logger.LogError("AuthService::LoginUserAsync loginRequest is null");
            throw new ArgumentNullException(nameof(loginRequest));
        }

        // Add Validation

        var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, loginRequest.RememberMe, lockoutOnFailure: false);

        return result;
    }


    
}
