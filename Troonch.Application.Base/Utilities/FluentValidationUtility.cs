using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Troonch.Domain.Base.DTOs.Response;

namespace Troonch.Application.Base.Utilities;

public static class FluentValidationUtility
{
    public static ModelStateDictionary SetModelState<T>(this ModelStateDictionary modelStateDictionary, IEnumerable<ValidationFailure> validationFailure, ILogger<T> _logger)
    {
        foreach (var failer in validationFailure)
        {
            _logger.LogError($"ModelStateDictionary -> ${failer.ErrorMessage}");
            modelStateDictionary.AddModelError(failer.PropertyName, failer.ErrorMessage);
        }
        return modelStateDictionary;
    }

    public static IEnumerable<ValidationError> SetValidationErrors<T>(IEnumerable<ValidationFailure> validationFailure, ILogger<T> _logger)
    {
        var validationErrors = new List<ValidationError>();
        foreach (var failer in validationFailure)
        {
            var validationError = new ValidationError();

            _logger.LogError($"API/ValidationErrors -> ${failer.ErrorMessage}");
            validationError.Field = failer.PropertyName;
            validationError.Message = failer.ErrorMessage;
            validationErrors.Add(validationError);
        }
        return validationErrors;
    }
}
