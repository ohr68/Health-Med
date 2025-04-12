using HealthMed.Application.Features.Auth.Login;
using HealthMed.Application.Interfaces.Strategy;
using HealthMed.Domain.Interfaces.Helpers;
using HealthMed.Domain.ValueObjects;
using HealthMed.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Application.Strategy;

public class LoginMedicoStrategy(HealthMedDbContext context, IPasswordHasher passwordHasher) : ILoginStrategy
{
    public async Task<(bool, Guid)> ValidarAsync(LoginCommand dadosLogin, CancellationToken cancellationToken = default)
    {
        if(!Crm.TryParse(dadosLogin.Usuario!, out var crm))
            return (false, Guid.Empty);

        var medico = await context.Medicos
            .Include(m => m.Usuario)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Crm.Valor.Equals(crm!,StringComparison.OrdinalIgnoreCase), cancellationToken);
        
        if(medico is null)
            return (false, Guid.Empty);
        
        return !passwordHasher.VerifyPassword(medico.Usuario!.Senha!, dadosLogin.Senha!) ? (false, Guid.Empty) : (true, medico.UsuarioId);
    }
}