using HealthMed.Keycloak.Models.Dtos;
using MediatR;

namespace HealthMed.Keycloak.Saga.CreateUser;

public class CreateUserSagaRequest(CreateUserFlowDto userDto) : IRequest
{
    public CreateUserFlowDto UserDto { get; } = userDto;
}