using HealthMed.Domain.Events;
using HealthMed.Domain.Exceptions;
using HealthMed.Keycloak.Saga.CreateUser;
using HealthMed.ORM.Context;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthMed.Consumers.Consumers;

public class PacienteCadastradoConsumer(IMediator mediator, HealthMedDbContext dbContext, ILogger<PacienteCadastradoConsumer> logger) : IConsumer<PacienteCadastradoEvent>
{
    public async Task Consume(ConsumeContext<PacienteCadastradoEvent> context)
    {
        logger.LogInformation("Iniciando consumer para o usuário {UserId}", context.Message.PacienteId);
        
        var message = context.Message;

        var paciente = await dbContext.Pacientes
                           .Include(p => p.Usuario)
                           .SingleOrDefaultAsync(x => x.Id == message.PacienteId, context.CancellationToken)
            ?? throw new NotFoundException($"Paciente {message.PacienteId} não encontrado.");
        
        var createUserSagaRequest = message.Adapt<CreateUserSagaRequest>();

        await mediator.Send(createUserSagaRequest, context.CancellationToken);

        paciente.Usuario!.CadastroSincronizado();
        dbContext.Entry(paciente).State = EntityState.Modified;

        var updateResult = await dbContext.SaveChangesAsync(context.CancellationToken) > 0;

        if (!updateResult)
            throw new BadRequestException("Houve uma falha ao atualizar o usuário do paciente.");
    }
}