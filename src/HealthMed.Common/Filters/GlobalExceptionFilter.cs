using FluentValidation;
using HealthMed.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthMed.Common.Filters;

public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        var statusCode = context.Exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            ForbiddenException => StatusCodes.Status403Forbidden,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            DbUpdateException
            {
                InnerException: SqlException { Number: 2601 }
            } => StatusCodes.Status409Conflict, // Unique constraint violation
            DbUpdateException
            {
                InnerException: SqlException { Number: 547 }
            } => StatusCodes.Status400BadRequest, // Foreign key violation
            DbUpdateException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        var message = context.Exception switch
        {
            BadRequestException or UnauthorizedException or ForbiddenException or NotFoundException
                or ValidationException => context.Exception.Message,
            DbUpdateException { InnerException: SqlException { Number: 2601 } } => context.Exception.InnerException
                .Message,
            DbUpdateException { InnerException: SqlException { Number: 547 } } => context.Exception.InnerException
                .Message,
            DbUpdateException => context.Exception.InnerException?.Message ?? context.Exception.Message,
            _ => isDevelopment ? context.Exception.ToString() : null
        };

        logger.LogError("Ocorreu um erro.");
        logger.LogError(context.Exception, "Detalhes: {message}", string.IsNullOrEmpty(message) ? "Consulte os logs de telemetria para mais detalhes" : message);
        
        context.Result = new ObjectResult(context.Exception is ValidationException validationException
            ? new ValidationProblemDetails
            {
                Title = "Ocorreu um erro",
                Detail = message,
                Type = validationException.GetType().Name,
                Status = statusCode,
                Errors = validationException.Errors
                    .GroupBy(k => k.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(k => k.Key, v => v.ToArray())
            }
            : new ProblemDetails
            {
                Title = "Ocorreu um erro",
                Detail = message,
                Type = context.Exception.GetType().Name,
                Status = statusCode,
            })
        {
            StatusCode = statusCode
        };
    }
}