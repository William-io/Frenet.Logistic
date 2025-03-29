namespace Frenet.Logistic.Infrastructure.Authentication;

public class JwtOptions
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SecretKey { get; init; }
    public int TokenExpirationInDays { get; init; } = 1; // Default é 1 dia
}
