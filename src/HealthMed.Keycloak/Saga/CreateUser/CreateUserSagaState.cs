namespace HealthMed.Keycloak.Saga.CreateUser;

public class CreateUserSagaState
{
    public Guid SagaId { get; set; } = Guid.NewGuid();
    public string? UserId { get; set; }
    public string? KeycloakUserId { get; set; }
    public string? Username { get; set; }
    public bool UserCreated { get; set; }
    public bool ClientRoleAssigned { get; set; }
    public bool RealmRoleAssigned { get; set; }
}