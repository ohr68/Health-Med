using System.Text.Json.Serialization;

namespace HealthMed.Keycloak.Models;

public class UserRepresentation
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; }
    [JsonPropertyName("attributes")]
    public Dictionary<string, List<string>>? Attributes { get; set; }
    [JsonPropertyName("userProfileMetadata")]
    public UserProfileMetadata? UserProfileMetadata { get; set; }
    [JsonPropertyName("self")]
    public string? Self { get; set; }
    [JsonPropertyName("origin")]
    public string? Origin { get; set; }
    [JsonPropertyName("createdTimestamp")]
    public long CreatedTimestamp { get; set; }
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
    [JsonPropertyName("totp")]
    public bool Totp { get; set; }
    [JsonPropertyName("federationLink")]
    public string? FederationLink { get; set; }
    [JsonPropertyName("serviceAccountClientId")]
    public string? ServiceAccountClientId { get; set; }
    [JsonPropertyName("credentials")]
    public CredentialRepresentation[]? Credentials { get; set; }
    [JsonPropertyName("disableableCredentialTypes")]
    public string[]? DisableAbleCredentialTypes { get; set; }
    [JsonPropertyName("requiredActions")]
    public string[]? RequiredActions { get; set; }
    [JsonPropertyName("federatedIdentities")]
    public FederatedIdentityRepresentation[]? FederatedIdentities { get; set; }
    [JsonPropertyName("realmRoles")]
    public string[]? RealmRoles { get; set; }
    [JsonPropertyName("clientRoles")]
    public Dictionary<string, List<string>>? ClientRoles { get; set; }
    [JsonPropertyName("clientConsents")]
    public UserConsentRepresentation[]? ClientConsents { get; set; }
    [JsonPropertyName("notBefore")]
    public int NotBefore { get; set; }
    [JsonPropertyName("applicationRoles")]
    public Dictionary<string, List<string>>? ApplicationRoles { get; set; }
    [JsonPropertyName("socialLinks")]
    public SocialLinkRepresentation[]? SocialLinks { get; set; }
    [JsonPropertyName("groups")]
    public string[]? Groups { get; set; }
    [JsonPropertyName("access")]
    public Dictionary<string, bool>? Access { get; set; }
}