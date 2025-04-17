using HealthMed.Application.Features.Auth.Login;
using HealthMed.Application.Features.Auth.Logout;
using HealthMed.Application.Features.Auth.RefreshToken;
using HealthMed.AuthApi.Common;
using KeycloakAuthIntegration.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace HealthMed.AuthApi.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api").WithTags("Auth");

        group.MapPost("/login", async (
            [FromBody] LoginCommand loginRequest,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(loginRequest, cancellationToken);
            return Results.Ok(new ApiResponseWithData<LoginResult>
            {
                Message = "Login efetuado com sucesso.",
                Data = result,
                Success = true
            });
        })
        .WithName("Login")
        .Produces<ApiResponseWithData<LoginResult>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status422UnprocessableEntity);

        group.MapPost("/refresh-token", async (
            [FromBody] RefreshTokenCommand refreshTokenRequest,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(refreshTokenRequest, cancellationToken);
            return Results.Ok(new ApiResponseWithData<RefreshTokenResult>
            {
                Message = "Refresh token obtido com sucesso.",
                Data = result,
                Success = true
            });
        })
        .WithName("RefreshToken")
        .Produces<ApiResponseWithData<RefreshTokenResult>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status422UnprocessableEntity);

        group.MapPost("/logout", async (
            [FromBody] LogoutCommand logoutRequest,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(logoutRequest, cancellationToken);
            return Results.Ok(new ApiResponse
            {
                Message = "Logout efetuado com sucesso.",
                Success = result.Success
            });
        })
        .WithName("Logout")
        .Produces<ApiResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status422UnprocessableEntity);
    }
}