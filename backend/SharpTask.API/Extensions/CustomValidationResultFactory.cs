using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using SharpTask.Domain.Common;
using SharpTask.Domain.Exceptions;

namespace SharpTask.API.Extensions;

/// <summary>
/// Implementación personalizada de IFluentValidationAutoValidationResultFactory
/// para adaptar los errores de validación a nuestro formato de respuesta estandarizado (ApiResponse).
/// </summary>
public class CustomValidationResultFactory : IFluentValidationAutoValidationResultFactory
{
    /// <summary>
    /// Implementación del método CreateActionResult que se llama
    /// cuando FluentValidation detecta errores de validación.
    /// </summary>
    /// <param name="context">El contexto de la acción en ejecución.</param>
    /// <param name="validationProblemDetails">Los detalles del problema de validación.</param>
    /// <param name="validationResults">Los resultados de la validación.</param>
    /// <returns>Una tarea que representa el resultado de la acción.</returns>
    public Task<IActionResult?> CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails validationProblemDetails,
        IDictionary<IValidationContext, ValidationResult> validationResults
    )
    {
        // Extraemos los errores de validación del ValidationProblemDetails
        var errors = validationProblemDetails.Errors;

        // Creamos un ApiError personalizado con el código de error y el mensaje general
        var apiError = new ApiError(
            ErrorCodes.ValidationError,
            "Uno o más campos tienen errores de validación.",
            errors
        );

        // Creamos una respuesta de error utilizando nuestro formato ApiResponse, indicando que la operación ha fallado
        var response = ApiResponse<object>.Failure(apiError);

        // Devolvemos un BadRequestObjectResult con la respuesta de error personalizada
        return Task.FromResult<IActionResult?>(new BadRequestObjectResult(response));
    }
}
