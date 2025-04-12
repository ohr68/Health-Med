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

public class MedicoCadastradoConsumer(IMediator mediator, HealthMedDbContext dbContext, ILogger<MedicoCadastradoConsumer> logger) : IConsumer<MedicoCadastradoEvent>
{
    public async Task Consume(ConsumeContext<MedicoCadastradoEvent> context)
    {
        logger.LogInformation("Iniciando consumer para o usuário {UserId}", context.Message.MedicoId);
        
        var message = context.Message;
        
        var medico = await dbContext.Medicos
                         .Include(m => m.Usuario)
                         .SingleOrDefaultAsync(x => x.Id == message.MedicoId)
            ?? throw new NotFoundException($"Médico {message.MedicoId} não encontrado.");
        
        var createUserSagaRequest = message.Adapt<CreateUserSagaRequest>();

        await mediator.Send(createUserSagaRequest, context.CancellationToken);

        medico.Usuario!.CadastroSincronizado();
        dbContext.Entry(medico).State = EntityState.Modified;

        var updateResult = await dbContext.SaveChangesAsync(context.CancellationToken) > 0;

        if (!updateResult)
            throw new BadRequestException("Houve uma falha ao atualizar o usuário do médico.");
    }
}