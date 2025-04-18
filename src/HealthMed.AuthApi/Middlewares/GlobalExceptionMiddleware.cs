using FluentValidation;
using HealthMed.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.AuthApi.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            var statusCode = ex switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                ForbiddenException => StatusCodes.Status403Forbidden,
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                DbUpdateException { InnerException: SqlException { Number: 2601 } } => StatusCodes.Status409Conflict,
                DbUpdateException { InnerException: SqlException { Number: 547 } } => StatusCodes.Status400BadRequest,
                DbUpdateException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            var message = ex switch
            {
                BadRequestException or UnauthorizedException or ForbiddenException or NotFoundException or ValidationException => ex.Message,
                DbUpdateException { InnerException: SqlException { Number: 2601 } } => ex.InnerException?.Message,
                DbUpdateException { InnerException: SqlException { Number: 547 } } => ex.InnerException?.Message,
                DbUpdateException => ex.InnerException?.Message ?? ex.Message,
                _ => isDevelopment ? ex.ToString() : null
            };

            logger.LogError("Ocorreu um erro.");
            logger.LogError(ex, "Detalhes: {message}", message ?? "Consulte os logs de telemetria para mais detalhes");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            object problem = ex is ValidationException validationException
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
                    Type = ex.GetType().Name,
                    Status = statusCode
                };

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}