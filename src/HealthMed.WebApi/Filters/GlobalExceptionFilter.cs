using FluentValidation;
using HealthMed.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HealthMed.WebApi.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        
        context.Result = context.Exception switch
        {
            DomainException e => new ObjectResult(new ProblemDetails
            {
                Title = "Ocorreu um erro",
                Detail = e.Message,
                Type = e.GetType().Name,
                Status = StatusCodes.Status400BadRequest,
            })
            {
                StatusCode = StatusCodes.Status400BadRequest
            },
            ValidationException e => new ObjectResult(new ValidationProblemDetails
            {
                Title = "Ocorreu um erro",
                Detail = e.Message,
                Type = e.GetType().Name,
                Status = StatusCodes.Status422UnprocessableEntity,
                Errors = e.Errors
                    .GroupBy(k => k.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(k => k.Key, v => v.ToArray())
            })
            {
                StatusCode = StatusCodes.Status400BadRequest
            },
            _ => new ObjectResult(new ProblemDetails
            {
                Title = "Ocorreu um erro inesperado",
                Detail = isDevelopment ? context.Exception.ToString() : null,
                Type = context.Exception.GetType().Name,
                Status = StatusCodes.Status500InternalServerError,
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}