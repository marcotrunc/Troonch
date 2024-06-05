using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Troonch.Application.Base.Utilities;
using Troonch.User.Application.Services;
using Troonch.User.Domain.DTOs.Requests;

namespace Troonch.Users.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly ILogger<UsersController> _logger;
    private readonly UserService _userService;

    [ActivatorUtilitiesConstructor]
    public UsersController(
        ILogger<UsersController> logger,
        UserService userService
    )
    {
        _logger = logger;
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

            if (!identityResult.Succeeded)
            {
                ModelState.SetModelState(identityResult.Errors, _logger);
                return View(identityResult.Errors);
            }

            return View("Index");
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
}
