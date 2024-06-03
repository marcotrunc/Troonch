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

namespace Troonch.User.Application.Services;

public class UserService
{
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly IEmailSender _emailSender;
    private readonly IUrlHelper _urlHelper;

    public UserService(
        ILogger<UserService> logger,
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        IEmailSender emailSender,
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor
    )
    {
        _logger = logger;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _emailSender = emailSender;
        _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
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

        //ADD VALIDATION

        var user = MapRequestDtoToApplicationUser(userRequest);


        await _userStore.SetUserNameAsync(user, user.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            return result;
        }

        //TODO -> Handle Role

        _logger.LogInformation($"UserService::GetUserByForUpdateAsync user {user.LastName} {user.Name} added correctly");

        await SendConfirmationMail(user);

        return result;
    }

    private async Task SendConfirmationMail(ApplicationUser user, string returnUrl = null)
    {
        returnUrl ??= _urlHelper.Content("~/");

        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        
        var callbackUrl = _urlHelper.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { area = "Identity", userId, code, returnUrl },
            protocol: "https");

        if (String.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentException(nameof(user.Email));    
        }

        await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
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


    #endregion
}
