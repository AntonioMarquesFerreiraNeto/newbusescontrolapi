using System.Diagnostics;
using BusesControl.Filters.Notification;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Service.Api.Utils;

public class ValidateModel
{
    public static async Task<ValidationProblemDetails?> CheckIsValid<R, V>(R request, PathString path, ModelStateDictionary modelState, V validator)
        where V : IValidator<R>
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            validation.AddToModelState(modelState);
            var result = new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = NotificationTitle.BadRequest,
                Detail = "Lista de erro de validações.",
                Instance = path,
                Extensions = { { "traceId", Activity.Current?.Id } }
            };

            var errorList = modelState.ToDictionary(
                          kvp => kvp.Key,
                          kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

            foreach (var error in errorList)
            {
                result.Errors.Add(error);
            }

            return result;
        }

        return null;
    }
}