using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Troonch.Application.Base.Utilities;

public static class FluentValidationUtility
{
    public static ModelStateDictionary SetModelState<T>(this ModelStateDictionary modelStateDictionary, IEnumerable<ValidationFailure> validationFailure, ILogger<T> _logger)
    {
        foreach (var failer in validationFailure)
        {
            _logger.LogError($"ProductController::Create -> ${failer.ErrorMessage}");
            modelStateDictionary.AddModelError(failer.PropertyName, failer.ErrorMessage);
        }
        return modelStateDictionary;
    }
}
