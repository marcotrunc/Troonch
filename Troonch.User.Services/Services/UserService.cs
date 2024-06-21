using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Troonch.Application.Base.Interfaces;
using Troonch.User.Domain.DTOs.Requests;
using Troonch.User.Domain.DTOs.Response;
using Troonch.User.Domain.Entities;

namespace Troonch.User.Application.Services;

public class UserService
{
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly IEmailSender _emailSender;
    private readonly IUrlHelper _urlHelper;
    private readonly IValidator<UserRequestDTO> _validator;
    private readonly IValidator<SetPasswordRequestDTO> _setPasswordvalidator;

    public UserService(
                ILogger<UserService> logger,
                UserManager<ApplicationUser> userManager,
                IUserStore<ApplicationUser> userStore,
                IEmailSender emailSender,
                IUrlHelperFactory urlHelperFactory,
                IActionContextAccessor actionContextAccessor,
                IValidator<UserRequestDTO> validator,
                IValidator<SetPasswordRequestDTO> setPasswordvalidator
    )
    {
        _logger = logger;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _emailSender = emailSender;
        _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        _validator = validator;
        _setPasswordvalidator = setPasswordvalidator;
    }

    public async Task<List<UserResponseDTO>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        if(users is null)
        {
            throw new ArgumentNullException(nameof(users));
        }

        return users.Select(u => MapApplicationUserToUserResponseDto(u)).ToList();
    }

    public async Task<UserRequestDTO> GetUserByForUpdateAsync(string? id)
    {
        if (String.IsNullOrWhiteSpace(id))
        {
            _logger.LogError("UserService::GetUserByForUpdateAsync id is not valid");
            throw new ArgumentNullException(nameof(id));
        }

        var applicationUser = await _userManager.FindByIdAsync(id);

        if (applicationUser is null) 
        {
            _logger.LogError("UserService::GetUserByForUpdateAsync applicationUser is null");
            throw new ArgumentNullException(nameof(applicationUser));
        }

        return MapApplicationUserToRequestDto(applicationUser);
    }

    public async Task<IdentityResult> RegisterUserAsync(UserRequestDTO userRequest)
    {
        if(userRequest is null)
        {
            _logger.LogError("UserService::GetUserByForUpdateAsync userRequest is null");
            throw new ArgumentNullException(nameof(userRequest));
        }

        await _validator.ValidateAndThrowAsync(userRequest);

        var user = MapRequestDtoToApplicationUser(userRequest);

        await _userStore.SetUserNameAsync(user, user.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            return result;
        }

        var claimNameResult = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, user.Name));
        var claimSurnameResult = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, user.LastName));

        if (claimNameResult is null || claimSurnameResult is null)
        {
            throw new ArgumentNullException(claimNameResult is null ? nameof(claimNameResult) : nameof(claimSurnameResult));
        }

        if (!claimNameResult.Succeeded || !claimSurnameResult.Succeeded)
        {
            throw new InvalidOperationException(claimNameResult.Succeeded ? nameof(claimNameResult) : nameof(claimSurnameResult));
        }

        //TODO -> Handle Role

        await _userManager.AddToRoleAsync(user, "user");

        _logger.LogInformation($"UserService::GetUserByForUpdateAsync user {user.LastName} {user.Name} added correctly");

        await SendConfirmationMail(user);

        return result;
    }

    public async Task<bool> HasAlreadyPasswordAsync(string userId)
    {
        if (String.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentNullException(nameof(userId));
        }

        var applicationUser = await _userManager.FindByIdAsync(userId);

        if(applicationUser is null)
        {
            throw new ArgumentNullException(nameof(applicationUser));
        }

        return await _userManager.HasPasswordAsync(applicationUser);
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string code)
    {
        if (String.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentNullException(nameof(userId));
        }

        if (String.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentNullException(nameof(code));
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result is null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        return result.Succeeded;
    }
    public async Task<string> GeneratePasswordResetTokenAsync(string userId)
    {
        if (String.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentNullException(nameof(userId));
        }

        var applicationUser = await _userManager.FindByIdAsync(userId);

        if (applicationUser is null) 
        { 
            throw new ArgumentNullException(nameof(applicationUser));
        }
        string token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

        if (String.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentNullException(nameof(token));
        }
        
        return token;
    }
    public async Task<IdentityResult?> SetPasswordAsync(SetPasswordRequestDTO setPasswordRequest)
    {
        if (setPasswordRequest is null) 
        {
            throw new ArgumentNullException(nameof(setPasswordRequest));    
        }

        await _setPasswordvalidator.ValidateAndThrowAsync(setPasswordRequest);

        var applicationUser = await _userManager.FindByIdAsync(setPasswordRequest.Id);

        if (applicationUser is null)
        {
            throw new ArgumentNullException(nameof(applicationUser));
        }

        var hasPassword = await _userManager.HasPasswordAsync(applicationUser);

        if (setPasswordRequest.IsFirstSetPassword && hasPassword) 
        {
            _logger.LogError($"UserService::SetPasswordAsync -> The user Has already the pwd");
            throw new InvalidOperationException(nameof(hasPassword));
        }

        var result = await _userManager.ResetPasswordAsync(applicationUser, setPasswordRequest.Code, setPasswordRequest.NewPassword);

        if (result is null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        return result;
    }
    public async Task InitializeForgotPasswordProcessAsync(ForgotPasswordRequestDTO forgotPasswordRequest)
    {

        if(forgotPasswordRequest is null)
        {
            throw new ArgumentException(nameof(forgotPasswordRequest)); 
        }
        
        var user = await _userManager.FindByEmailAsync(forgotPasswordRequest.Email);

        if(user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        
        if (!(await _userManager.IsEmailConfirmedAsync(user)))
        {
            await SendConfirmationMail(user);
            return;
        }

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);

        string callbackUrl = _urlHelper.Action("ResetPassword", "Users", values: new {userId = user.Id, code}, protocol: "https")
                                                                        .Replace("&amp", "&")
                                                                        .Replace("%2F", "/");
        

        await _emailSender.SendEmailAsync(
            forgotPasswordRequest.Email,
            "Reset Password",
            $"Per favore Resetta la tua password <a href='{callbackUrl}'>cliccando qui</a>.");
    }
    private async Task SendConfirmationMail(ApplicationUser user, string returnUrl = null)
    {
        returnUrl ??= _urlHelper.Content("~/");

        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        string callbackUrl = _urlHelper.Action("ConfirmEmail", "Users", values: new { userId, code, returnUrl }, protocol: "https")
                                                                        .Replace("&amp","&")
                                                                        .Replace("%2F","/");

        if (String.IsNullOrWhiteSpace(callbackUrl))
        {
            throw new ArgumentNullException(nameof(callbackUrl));
        }

        if (String.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentException(nameof(user.Email));    
        }

        await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
            $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");
    }
    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }

    private int CalculateProgressOfDataComplete (ApplicationUser applicationUser)
    {
        var modelType = typeof(ApplicationUser);
        var properties = modelType.GetProperties();
        var propertiesName = properties
                                .Where(p => p.CanWrite)
                                .Select(p => p.Name);

        var valueMaxOfProgress = 100;
        var singleProgressItem = valueMaxOfProgress / propertiesName.Count();

        int progressOfDataComplete = 0;
        
        foreach (var property in properties)
        {
            var value = property.GetValue(applicationUser);

            var defaultValue = property.PropertyType.IsValueType
                                ? Activator.CreateInstance(property.PropertyType)
                                : null;

            bool isDifferentFromDefault = !Equals(value, defaultValue);

            if (isDifferentFromDefault)
            {
                progressOfDataComplete += singleProgressItem;
            }
        }


        if (progressOfDataComplete >= 95)
        {
            progressOfDataComplete = 100;
        }

        return progressOfDataComplete;
    }

    #region Mapping
    private UserRequestDTO MapApplicationUserToRequestDto(ApplicationUser applicationUser) => new UserRequestDTO()
    {
        Id = Guid.TryParse(applicationUser.Id, out var userId) ? userId : Guid.Empty,
        Email = applicationUser.Email,
        Name = applicationUser.Name,
        LastName = applicationUser.LastName,
        DateOfBirth = applicationUser.DateOfBirth,
        PhoneNumber = applicationUser.PhoneNumber
    };

    private ApplicationUser MapRequestDtoToApplicationUser(UserRequestDTO request) => new ApplicationUser()
    {
        Id = Guid.NewGuid().ToString(),
        Email = request.Email,
        Name = request.Name,
        LastName = request.LastName,
        DateOfBirth = request.DateOfBirth ?? DateOnly.MaxValue,
        PhoneNumber = request.PhoneNumber,
        CreatedOn = DateTime.UtcNow
    };

    private UserResponseDTO MapApplicationUserToUserResponseDto(ApplicationUser applicationUser) => new UserResponseDTO()
    {
        Id = applicationUser.Id,
        Name = applicationUser.Name,
        LastName = applicationUser.LastName,
        Email = applicationUser.Email ?? throw new ArgumentNullException(nameof(applicationUser.Email)),
        IsEmailConfirmed = applicationUser.EmailConfirmed,
        PhoneNumber = applicationUser.PhoneNumber,
        ProgressOfDataComplete = CalculateProgressOfDataComplete(applicationUser),
        TwoFactorEnabled = applicationUser.TwoFactorEnabled,
        DateOfBirth = applicationUser.DateOfBirth,
        CreatedOn = applicationUser.CreatedOn,
        UpdatedOn = applicationUser.UpdatedOn,
    };

    #endregion
}
