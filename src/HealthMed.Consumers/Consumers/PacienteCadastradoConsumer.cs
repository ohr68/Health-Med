using HealthMed.Domain.Events;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Interfaces.Messaging;
using HealthMed.Keycloak.Saga.CreateUser;
using HealthMed.ORM.Context;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthMed.Consumers.Consumers;

public class PacienteCadastradoConsumer(IMediator mediator, IBusService busService, ApplicationDbContext dbContext, ILogger<PacienteCadastradoConsumer> logger) : IConsumer<PacienteCadastradoEvent>
{
    public async Task Consume(ConsumeContext<PacienteCadastradoEvent> context)
    {
        logger.LogInformation("Iniciando consumer para o usuário {UserId}", context.Message.PacienteId);
        
        var message = context.Message;

        var createUserSagaRequest = request.Adapt<CreateUserSagaRequest>();

        await mediator.Send(createUserSagaRequest, context.CancellationToken);

        userSync.Synchronized();
        dbContext.Entry(userSync).State = EntityState.Modified;

        var updateResult = await dbContext.SaveChangesAsync(context.CancellationToken) > 0;

        if (!updateResult)
            throw new BadRequestException("Houve uma falha ao atualizar o usuário do paciente.");
        
        logger.LogInformation("Sending UserSynchronized to queue.");
        await busService.Publish(message.Adapt<UserSynchronized>(), context.CancellationToken);
        logger.LogInformation("UserSynchronized sent to queue.");
    }
}