using HealthMed.Keycloak.Constants;
using HealthMed.Keycloak.Enums;
using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Models;
using HealthMed.Keycloak.Saga.CreateUser.Commands;
using MediatR;

namespace HealthMed.Keycloak.Saga.CreateUser.Handlers;

public class AssignClientRoleCommandHandler(
    IClientRoleService clientRoleService, 
    IUserService userService) 
    : IRequestHandler<AssignClientRoleCommand>
{
    public async Task Handle(AssignClientRoleCommand request, CancellationToken cancellationToken)
    {
        var clientRole = await clientRoleService.GetRoleByNameAsync(ClientRoles.DefaultUmaProtection, cancellationToken)
                         ?? throw new Exception($"Failed to get client role {ClientRoles.DefaultUmaProtection}");

        await userService.AssignRoleAsync(request.UserId, new List<RoleRepresentation> { clientRole }, RoleType.Client, cancellationToken);
    }
}