using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Text;
using Troonch.Application.Base.Utilities;
using Troonch.User.Application.Services;
using Troonch.User.Domain.DTOs.Requests;
using Troonch.User.Domain.Entities;

namespace Troonch.Users.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly ILogger<UsersController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserService _userService;
    private string _returnUrl = "/Home/Index";

    [ActivatorUtilitiesConstructor]
    public UsersController(
        ILogger<UsersController> logger,
        SignInManager<ApplicationUser> signInManager,
        UserService userService
    )
    {
        _logger = logger;
        _signInManager = signInManager;
        _userService = userService;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Users/Register/{id?}")]
    public async Task<IActionResult> Register(string? id)
    {
        var userRequest = new UserRequestDTO();
        
        try
        {

            if (!String.IsNullOrWhiteSpace(id))
            {
                userRequest = await _userService.GetUserByForUpdateAsync(id);
            }

            return View(userRequest);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::Register GET -> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::Register GET -> {ex.Message}");
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(UserRequestDTO request)
    {
        try
        {
            var identityResult = await _userService.RegisterUserAsync(request);

            if (identityResult is null) 
            { 
                throw new ArgumentNullException(nameof(identityResult));
            }

            if (!identityResult.Succeeded)
            {
                ModelState.SetModelState(identityResult.Errors, _logger);
                return View(request);
            }

            return RedirectToAction("Index", "Users");
        }
        catch (ValidationException ex)
        {
            ModelState.SetModelState(ex.Errors, _logger);

            return View(request);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::Register POST-> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::Register POST-> {ex.Message}");
            throw;
        }
    }


    [AllowAnonymous]
    [HttpGet("Users/ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromQuery]string userId, [FromQuery] string code, [FromQuery] string returnUrl)
    {
        try
        {

            if (String.IsNullOrWhiteSpace(userId) || String.IsNullOrWhiteSpace(code))
            {
                _logger.LogError(userId is null ? "UserController::ConfirmEmail User Id is null" : "UserController::ConfirmEmail code is null");
                return Redirect(returnUrl ?? _returnUrl);
            }


            var isEmailConfirmed = await _userService.ConfirmEmailAsync(userId, code);

            string pwdResetToken = string.Empty;

            if (isEmailConfirmed) 
            {
                pwdResetToken = await _userService.GeneratePasswordResetTokenAsync(userId);
            }

            if (String.IsNullOrEmpty(pwdResetToken)) 
            {
                throw new ArgumentException(nameof(pwdResetToken));
            }
            
            ViewBag.StatusMessage = isEmailConfirmed ? "Grazie Per Aver confermato la tua email." : "Errore durante la riconferma della tua email, ricontatta l'amministratore";
            ViewBag.UserId = userId;
            ViewBag.Code =  pwdResetToken;
            
            return View("ConfirmEmail");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::ConfirmEmail GET -> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::ConfirmEmail GET -> {ex.Message}");
            throw;
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> SetPassword([FromQuery] string userId, [FromQuery] string code)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(userId) || String.IsNullOrWhiteSpace(code))
            {
                _logger.LogError(userId is null ? "UserController::SetPassword User Id is null" : "UserController::SetPassword code is null");
                return Redirect(_returnUrl);
            }

            var setPasswordModel = new SetPasswordRequestDTO()
            {
                Id = userId,
                Code = code
            };

            return View("SetPassword", setPasswordModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::SetPassword GET -> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::SetPassword GET -> {ex.Message}");
            throw;
        }
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordRequestDTO request)
    {
        try
        {
            var identityResult = await _userService.SetPasswordAsync(request);

            if (identityResult is null)
            {
                throw new ArgumentNullException(nameof(identityResult));
            }

            if (!identityResult.Succeeded)
            {
                ModelState.SetModelState(identityResult.Errors, _logger);
                return View(request);
            }

            return RedirectToAction("Login", "Auth");

        }
        catch (ValidationException ex)
        {
            ModelState.SetModelState(ex.Errors, _logger);

            return View(request);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::SetPassword POST-> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::SetPassword POST-> {ex.Message}");
            throw;
        }
    }

}
