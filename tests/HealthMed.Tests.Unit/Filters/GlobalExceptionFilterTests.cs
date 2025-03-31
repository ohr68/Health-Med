using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using FluentValidation;
using HealthMed.Common.Filters;
using HealthMed.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

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
        var loggerMock = new Mock<ILogger<GlobalExceptionFilter>>();
        var ex = new BadRequestException("Bad request");
        var exceptionContext = new ExceptionContext(this._actionContext, new List<IFilterMetadata>())
        {
            Exception = ex
        };
        var filter = new GlobalExceptionFilter(loggerMock.Object);

        // Act
        filter.OnException(exceptionContext);

        // Assert
        var objectResult = exceptionContext.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status400BadRequest, (objectResult.Value as ProblemDetails)!.Status);
        
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
            ),
            Times.AtLeast(1)
        );
    }

    [Fact(DisplayName = "Not Found Exception")]
    public void OnException_ShouldSetCorrectStatusCodeForNotFoundException()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionFilter>>();
        var ex = new NotFoundException("Not found");
        var exceptionContext = new ExceptionContext(this._actionContext, new List<IFilterMetadata>())
        {
            Exception = ex
        };
        var filter = new GlobalExceptionFilter(loggerMock.Object);

        // Act
        filter.OnException(exceptionContext);

        // Assert
        var objectResult = exceptionContext.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status404NotFound, (objectResult.Value as ProblemDetails)!.Status);
        
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
            ),
            Times.AtLeast(1)
        );
    }


    [Fact(DisplayName = "Validation Exception")]
    public void OnException_ShouldSetCorrectStatusCodeForValidationException()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionFilter>>();
        var ex = new ValidationException("Validation error");
        var exceptionContext = new ExceptionContext(this._actionContext, new List<IFilterMetadata>())
        {
            Exception = ex
        };
        var filter = new GlobalExceptionFilter(loggerMock.Object);

        // Act
        filter.OnException(exceptionContext);

        // Assert
        var objectResult = exceptionContext.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, (objectResult.Value as ProblemDetails)!.Status);
        
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
            ),
            Times.AtLeast(1)
        );
    }

    [Fact(DisplayName = "Internal Exception")]
    public void OnException_ShouldSetCorrectStatusCodeForInternalException()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionFilter>>();
        var ex = new Exception("Internal error");
        var exceptionContext = new ExceptionContext(this._actionContext, new List<IFilterMetadata>())
        {
            Exception = ex
        };
        var filter = new GlobalExceptionFilter(loggerMock.Object);

        // Act
        filter.OnException(exceptionContext);

        // Assert
        var objectResult = exceptionContext.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, (objectResult.Value as ProblemDetails)!.Status);
        
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
            ),
            Times.AtLeast(1)
        );
    }
}