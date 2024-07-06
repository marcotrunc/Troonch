using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Troonch.Application.Base.Utilities;
using Troonch.Domain.Base.DTOs.Response;
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
    public async Task<IActionResult> Index()
    {
        try
        {
            var users = await _userService.GetUsersAsync();

            if (users is null) 
            {
                throw new ArgumentNullException(nameof(users));
            }

            return View("Index", users);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::Index GET -> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::Index GET -> {ex.Message}");
            throw;
        }

    }

    [HttpGet("Users/Register")]
    public async Task<IActionResult> Register()
    {
        
        try
        {
            var userRequest = new UserRequestDTO();

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

            TempData["succeeded"] = identityResult.Succeeded;
            TempData["message"] = $"L' utente {request.Name} {request.LastName} è stato  registrato, una mail gli sarà inviata";

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

    [HttpGet("Users/{id?}/Update")]
    public async Task<IActionResult> Update(string? id)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var userRequest = await _userService.GetUserByIdForUpdateAsync(id);

            if(userRequest is null)
            {
                throw new ArgumentNullException(nameof(userRequest));
            }

            return View(userRequest);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::Update GET -> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::Update GET -> {ex.Message}");
            throw;
        }

    }

    [HttpPost("Users/{id?}/Update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(string? id, UserRequestDTO request)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            request.Id = id;

            var identityResult = await _userService.UpdateUserAsync(request);

            if (identityResult is null)
            {
                throw new ArgumentNullException(nameof(identityResult));
            }

            TempData["succeeded"] = identityResult.Succeeded;
            TempData["message"] = "Il profilo è stato Aggiornato!";

            if (!identityResult.Succeeded)
            {
                ModelState.SetModelState(identityResult.Errors, _logger);
                return View(request);
            }

            var userUpdated = await _userService.GetUserByIdForUpdateAsync(id);
            
            return View(userUpdated);

        }
        catch (ValidationException ex)
        {
            ModelState.SetModelState(ex.Errors, _logger);

            return View(request);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::Update POST-> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::Update POST-> {ex.Message}");
            throw;
        }
    }

    [HttpGet("Users/{id?}/ChangePassword")]
    public async Task<IActionResult> ChangePassword(string? id)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var userRequest = await _userService.GetUserByIdForUpdateAsync(id);

            if (userRequest is null)
            {
                throw new ArgumentNullException(nameof(userRequest));
            }

            return View(new ChangePasswordRequestDTO());
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::ChangePassword GET -> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::ChangePassword GET -> {ex.Message}");
            throw;
        }
    }

    [HttpPost("Users/{id?}/ChangePassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string id,ChangePasswordRequestDTO request)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }


            var isPasswordUpdated = await _userService.ChangePasswordAsync(id, request);

            if (isPasswordUpdated is null)
            {
                throw new ArgumentNullException(nameof(isPasswordUpdated));
            }

            if (!isPasswordUpdated.Succeeded)
            {
                ModelState.SetModelState(isPasswordUpdated.Errors, _logger);
            }
            else
            {
                TempData["succeeded"] = isPasswordUpdated.Succeeded;
                TempData["message"] = "La password è stata modificata con successo";
            }

            return View(new ChangePasswordRequestDTO());
        }
        catch (ValidationException ex)
        {
            ModelState.SetModelState(ex.Errors, _logger);

            return View(request);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::ChangePassword POST-> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::ChangePassword POST-> {ex.Message}");
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
    [Authorize(Roles = "admin")]
    [HttpGet("Users/ConfirmEmail/{userId?}")]
    public async Task<IActionResult> ConfirmEmail(string userId)
    {
        var responseModel = new ResponseModel<bool>();
        try
        {

            if (String.IsNullOrWhiteSpace(userId))
            {
                _logger.LogError("UserController::ConfirmEmail Authorize -> User Id is null");
                throw new ArgumentException(nameof(userId));
            }


            var isEmailConfirmed = await _userService.ConfirmEmailFromAdminAsync(userId);

            if (!isEmailConfirmed) 
            {
                throw new ArgumentException(nameof(isEmailConfirmed));
            }

            return StatusCode(200, responseModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::ConfirmEmail Authorize -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = ex.Message;
            return StatusCode(400, responseModel);
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::ConfirmEmail Authorize -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = "Internal Server Error";
            return StatusCode(500, responseModel);
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet("Users/ConfirmPhoneNumber/{userId?}")]
    public async Task<IActionResult> ConfirmPhoneNumber(string userId)
    {
        var responseModel = new ResponseModel<bool>();
        try
        {

            if (String.IsNullOrWhiteSpace(userId))
            {
                _logger.LogError("UserController::ConfirmPhoneNumber Authorize -> User Id is null");
                throw new ArgumentException(nameof(userId));
            }


            var isPhoneNumberConfirmed = await _userService.ConfirmPhoneNumberFromAdminAsync(userId);

            if (!isPhoneNumberConfirmed)
            {
                throw new ArgumentException(nameof(isPhoneNumberConfirmed));
            }

            return StatusCode(200, responseModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::ConfirmPhoneNumber Authorize -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = ex.Message;
            return StatusCode(400, responseModel);
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::ConfirmPhoneNumber Authorize -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = "Internal Server Error";
            return StatusCode(500, responseModel);
        }
    }
    [Authorize(Roles = "admin")]
    [HttpDelete("Users/{userId?}")]
    public async Task<IActionResult> Delete(string userId)
    {
        var responseModel = new ResponseModel<bool>();
        try
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                _logger.LogError("UserController::DeleteUser DeleteUser -> User Id is null");
                throw new ArgumentNullException(nameof(userId));
            }


            var isUserDeleted = await _userService.DeleteUserAsync(userId);

            if (!isUserDeleted)
            {
                throw new InvalidOperationException(nameof(isUserDeleted));
            }

            return StatusCode(200, responseModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::DeleteUser DeleteUser -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = ex.Message;
            return StatusCode(400, responseModel);
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::DeleteUser DELETE -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = "Internal Server Error";
            return StatusCode(500, responseModel);
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet("Users/{userId}/PromoteToAdmin")]
    public async Task<IActionResult> PromoteToAdmin(string userId)
    {
        var responseModel = new ResponseModel<bool>();
        try
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                _logger.LogError("UserController::PromoteToAdmin -> User Id is null");
                throw new ArgumentNullException(nameof(userId));
            }

            var isUserPromoted = await _userService.PromoteToAdminAsync(userId);

            if (!isUserPromoted)
            {
                throw new InvalidOperationException(nameof(isUserPromoted));
            }

            return StatusCode(200, responseModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::PromoteToAdmin -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = ex.Message;
            return StatusCode(400, responseModel);
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::PromoteToAdmin -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = "Internal Server Error";
            return StatusCode(500, responseModel);
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

            var hasAlreadyPassword = await _userService.HasAlreadyPasswordAsync(userId);

            var setPasswordModel = new SetPasswordRequestDTO()
            {
                Id = userId,
                Code = code,
                IsFirstSetPassword = !hasAlreadyPassword
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

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> ForgotPassword()
    {
        try
        { 
            var model = new ForgotPasswordRequestDTO();

            return View("ForgotPassword", model);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::ForgotPassword GET-> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::ForgotPassword GET-> {ex.Message}");
            throw;
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDTO request)
    {
        try
        {
            await _userService.InitializeForgotPasswordProcessAsync(request);

            return RedirectToAction("ForgotPasswordConfiramtion","Users");
        }
        catch (ValidationException ex)
        {
            ModelState.SetModelState(ex.Errors, _logger);

            return View(request);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::ForgotPassword POST-> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::ForgotPassword POST-> {ex.Message}");
            throw;
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> ForgotPasswordConfiramtion()
    {
        try
        {
            return View("ForgotPasswordConfirmation");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::ForgotPasswordConfiramtion GET-> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::ForgotPasswordConfiramtion GET-> {ex.Message}");
            throw;
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> ResetPassword([FromQuery] string userId, [FromQuery] string code)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(userId) || String.IsNullOrWhiteSpace(code))
            {
                _logger.LogError(userId is null ? "UserController::ResetPassword User Id is null" : "UserController::SetPassword code is null");
                return Redirect(_returnUrl);
            }

            var setPasswordModel = new SetPasswordRequestDTO()
            {
                Id = userId,
                Code = code,
                IsFirstSetPassword = false
            };

            return View("SetPassword", setPasswordModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::ResetPassword GET -> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::ResetPassword GET -> {ex.Message}");
            throw;
        }
    }

    [HttpGet("RenderUserCard/{userId?}")]
    public async Task<IActionResult> RenderUserCard(string userId)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var userModel = await _userService.GetUserByIdAsync(userId);

            if (userModel is null) 
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            return PartialView("_UserCard", userModel);

        }
         catch (ArgumentNullException ex)
        {
            _logger.LogError($"UsersController::RenderUserCard GET -> {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UsersController::RenderUserCard GET -> {ex.Message}");
            throw;
        }
    }
}
