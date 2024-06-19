using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;
using Troonch.Application.Base.Utilities;
using Troonch.User.Application.Services;
using Troonch.User.Domain.DTOs.Requests;
using Troonch.User.Domain.Entities;

namespace Troonch.Users.Controllers;

[Authorize]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AuthService _authService;
    private string _returnUrl = "/Home/Index";

    [ActivatorUtilitiesConstructor]
    public AuthController(
        ILogger<AuthController> logger,
        SignInManager<ApplicationUser> signInManager,
        AuthService authService
    )
    {
        _logger = logger;
        _signInManager = signInManager;
        _authService = authService;
    }
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Login()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        var loginModel = new LoginRequestDTO();

        loginModel.Email = "marcotrunc@gmail.com";
        loginModel.Password = "Test1234?";
        loginModel.RememberMe = true;

        try
        {
            return View(loginModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"AuthController::Login GET -> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"AuthController::Login GET -> {ex.Message}");
            throw;
        }
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequestDTO request)
    {

        try
        {
            var signInResult = await _authService.LoginUserAsync(request);

            if (signInResult is null) 
            {
                throw new ArgumentNullException(nameof(signInResult));
            }

            if (signInResult.RequiresTwoFactor)
            {
               return RedirectToPage("./LoginWith2fa", new { ReturnUrl = _returnUrl, RememberMe = request.RememberMe });
            }

            if (signInResult.Succeeded)
            {
                _logger.LogInformation($"AuthController::Login POST -> user {request.Email} logged in");
                return Redirect(_returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                _logger.LogWarning($"AuthController::Login POST -> user {request.Email} locked out");
                return RedirectToPage("./Lockout");
            }
           
            View(request);

        }
        catch (ValidationException ex)
        {
            ModelState.SetModelState(ex.Errors, _logger);

            return View(request);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"AuthController::Login POST-> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"AuthController::Login POST-> {ex.Message}");
            throw;
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();


        _logger.LogInformation($"AuthController::Logout POST -> User Logged Out");
        if (_returnUrl != null)
        {
            return Redirect(_returnUrl);
        }
        else
        {
            return View();
        }
    }


}
