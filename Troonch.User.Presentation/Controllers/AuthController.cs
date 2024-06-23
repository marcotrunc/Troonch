using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Troonch.Application.Base.Utilities;
using Troonch.Domain.Base.DTOs.Response;
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
        loginModel.Password = "Test1234??";
        loginModel.RememberMe = true;

        try
        {
            return View(loginModel);
        }
        catch (ValidationException ex)
        {
            ModelState.SetModelState(ex.Errors, _logger);

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

    [HttpGet("Auth/Logout")]
    public async Task<IActionResult> Logout()
    {

        await _signInManager.SignOutAsync();

        _logger.LogInformation($"AuthController::Logout GET -> User Logged Out");
        
        return RedirectToAction("Login", "Auth");
    }

    [Authorize(Roles = "admin")]
    [HttpGet("Auth/HandleTwoFactorAuthentication/{userId}/{enabled}")]
    public async Task<IActionResult> HandleTwoFactorAuthentication(string userId, bool enabled)
    {
        var responseModel = new ResponseModel<bool>();

        try
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                _logger.LogError("AuthController::EnableTwoFactorAuthentication GET -> User Id is null");
                throw new ArgumentException(nameof(userId));
            }

            var isTwoFactorAuthUpdated = await _authService.HandleTwoFactorAuthenticationAsync(userId, enabled);

            if (!isTwoFactorAuthUpdated) 
            { 
                throw new InvalidOperationException(nameof(isTwoFactorAuthUpdated));
            }

            return StatusCode(200, responseModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"AuthController::EnableTwoFactorAuthentication GET -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = ex.Message;
            return StatusCode(400, responseModel);
        }
        catch (Exception ex)
        {
            _logger.LogError($"AuthController::EnableTwoFactorAuthentication GET -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = "Internal Server Error";
            return StatusCode(500, responseModel);
        }
    }


}
