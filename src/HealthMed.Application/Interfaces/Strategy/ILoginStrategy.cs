using HealthMed.Application.Features.Auth.Login;

namespace HealthMed.Application.Interfaces.Strategy;

public interface ILoginStrategy
{
    Task<(bool, Guid)> ValidarAsync(LoginCommand dadosLogin, CancellationToken cancellationToken = default);
}