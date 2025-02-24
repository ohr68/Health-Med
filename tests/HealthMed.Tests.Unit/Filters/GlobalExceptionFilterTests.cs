using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using FluentValidation;
using HealthMed.Domain.Exceptions;
using HealthMed.WebApi.Filters;

namespace HealthMed.Tests.Unit.Filters;

public class GlobalExceptionFilterTests
{
    private ActionContext _actionContext = new()
    {
        HttpContext = new DefaultHttpContext(),
        RouteData = new RouteData(),
        ActionDescriptor = new ActionDescriptor()
    };

    [Fact(DisplayName = "Bad Request Exception")]
    public void OnException_ShouldSetCorrectStatusCodeForBadRequestException()
    {
        // Arrange
        var ex = new BadRequestException("Bad request");
        var exceptionContext = new ExceptionContext(this._actionContext, new List<IFilterMetadata>())
        {
            Exception = ex
        };
        var filter = new GlobalExceptionFilter();

        // Act
        filter.OnException(exceptionContext);

        // Assert
        var objectResult = exceptionContext.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status400BadRequest, (objectResult.Value as ProblemDetails)!.Status);
    }

    [Fact(DisplayName = "Not Found Exception")]
    public void OnException_ShouldSetCorrectStatusCodeForNotFoundException()
    {
        // Arrange
        var ex = new NotFoundException("Not found");
        var exceptionContext = new ExceptionContext(this._actionContext, new List<IFilterMetadata>())
        {
            Exception = ex
        };
        var filter = new GlobalExceptionFilter();

        // Act
        filter.OnException(exceptionContext);

        // Assert
        var objectResult = exceptionContext.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status404NotFound, (objectResult.Value as ProblemDetails)!.Status);
    }


    [Fact(DisplayName = "Validation Exception")]
    public void OnException_ShouldSetCorrectStatusCodeForValidationException()
    {
        // Arrange
        var ex = new ValidationException("Validation error");
        var exceptionContext = new ExceptionContext(this._actionContext, new List<IFilterMetadata>())
        {
            Exception = ex
        };
        var filter = new GlobalExceptionFilter();

        // Act
        filter.OnException(exceptionContext);

        // Assert
        var objectResult = exceptionContext.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, (objectResult.Value as ProblemDetails)!.Status);
    }

    [Fact(DisplayName = "Internal Exception")]
    public void OnException_ShouldSetCorrectStatusCodeForInternalException()
    {
        // Arrange
        var ex = new Exception("Internal error");
        var exceptionContext = new ExceptionContext(this._actionContext, new List<IFilterMetadata>())
        {
            Exception = ex
        };
        var filter = new GlobalExceptionFilter();

        // Act
        filter.OnException(exceptionContext);

        // Assert
        var objectResult = exceptionContext.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, (objectResult.Value as ProblemDetails)!.Status);
    }
}