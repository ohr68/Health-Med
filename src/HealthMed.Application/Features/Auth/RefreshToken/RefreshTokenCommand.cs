using HealthMed.Application.Models.Requests;
using MediatR;

namespace HealthMed.Application.Features.Auth.RefreshToken;

public class RefreshTokenCommand : AuthRequestModel, IRequest<RefreshTokenResult>
{
    public string? RefreshToken { get; set; }
}