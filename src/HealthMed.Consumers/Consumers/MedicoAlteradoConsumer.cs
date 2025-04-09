using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Interfaces.Messaging;
using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Models;
using HealthMed.ORM.Context;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthMed.Consumers.Consumers;

public class MedicoAlteradoConsumer(IMediator mediator, IBusService busService, ApplicationDbContext dbContext, IUserService userService, ILogger<MedicoAlteradoConsumer> logger) : IConsumer<UserUpdated>
{
    public async Task Consume(ConsumeContext<UserUpdated> context)
    {
        logger.LogInformation("Starting UserCreatedConsumer for {UserId}", context.Message.Id);

        var message = context.Message;
        
        var userRepresentation = request.Adapt<UserRepresentation>();

        await userService.UpdateUserAsync(message.Crm, userRepresentation, context.CancellationToken);

        userSync.Synchronized();
        dbContext.Entry(userSync).State = EntityState.Modified;

        var updateResult = await dbContext.SaveChangesAsync(context.CancellationToken) > 0;

        if (!updateResult)
            throw new BadRequestException("Houve uma falha ao atualizar o usuário.");

        logger.LogInformation("Sending UserSynchronized to queue.");
        await busService.Publish(message.Adapt<UserSynchronized>(), context.CancellationToken);
        logger.LogInformation("UserSynchronized sent to queue.");
    }
}