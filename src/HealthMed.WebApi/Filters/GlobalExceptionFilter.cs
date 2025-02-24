using FluentValidation;
using HealthMed.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HealthMed.WebApi.Filters;

public class GlobalExceptionFilter: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var statusCode = context.Exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            ForbiddenException => StatusCodes.Status403Forbidden,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Result = new ObjectResult(new ProblemDetails
        {
            Title = "Ocorreu um erro",
            Detail = context.Exception.Message,
            Type = context.Exception.GetType().Name,
            Status = statusCode
        })
        {
            StatusCode = statusCode
        };
    }
}