using HealthMed.Application.Features.Auth.Login;
using HealthMed.Application.Interfaces.Strategy;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Helpers;
using HealthMed.Domain.ValueObjects;
using HealthMed.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Application.Strategy;

public class LoginPacienteStrategy(HealthMedDbContext context, IPasswordHasher passwordHasher): ILoginStrategy
{
    public async Task<(bool, Guid)> ValidarAsync(LoginCommand dadosLogin, CancellationToken cancellationToken = default)
    {
        Paciente? paciente = null;
        if (Cpf.TryParse(dadosLogin.Usuario!))
        {
            paciente = await context.Pacientes
                .Include(m => m.Usuario)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Cpf.Valor.Equals(dadosLogin.Usuario, StringComparison.OrdinalIgnoreCase),
                    cancellationToken);
        }

        if (Email.TryParse(dadosLogin.Usuario!))
        {
            paciente = await context.Pacientes
                .Include(m => m.Usuario)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Email.Valor.Equals(dadosLogin.Usuario, StringComparison.OrdinalIgnoreCase),
                    cancellationToken);
        }

        if(paciente is null)
            return (false, Guid.Empty);
        
        return !passwordHasher.VerifyPassword(paciente.Usuario!.Senha!, dadosLogin.Senha!) ? (false, Guid.Empty) : (true, paciente.UsuarioId);
    }
}