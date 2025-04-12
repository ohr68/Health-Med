using MediatR;

namespace HealthMed.Application.Features.Auth.Logout;

public class LogoutCommand : IRequest<LogoutResult>
{
    public Guid UserId { get; set; }
}