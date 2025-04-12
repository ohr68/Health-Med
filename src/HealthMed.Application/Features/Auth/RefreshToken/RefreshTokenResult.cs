
namespace HealthMed.Application.Features.Auth.RefreshToken;

public class RefreshTokenResult(
    string? accessToken,
    TimeSpan expiresIn,
    TimeSpan refreshTokenExpiresIn,
    string? refreshToken,
    string? tokenType,
    int notValidBeforePolicy,
    string? sessionState,
    string? scope)
{
    public string? AccessToken { get; set; } = accessToken;
    public TimeSpan ExpiresIn { get; set; } = expiresIn;
    public TimeSpan RefreshTokenExpiresIn { get; set; } = refreshTokenExpiresIn;
    public string? RefreshToken { get; set; } = refreshToken;
    public string? TokenType { get; set; } = tokenType;
    public int NotValidBeforePolicy  { get; set; } = notValidBeforePolicy;
    public string? SessionState { get; set; } = sessionState;
    public string? Scope { get; set; } = scope;
}