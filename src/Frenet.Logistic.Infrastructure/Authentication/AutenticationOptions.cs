namespace Frenet.Logistic.Infrastructure.Authentication;

public sealed class AutenticationOptions
{
    public string Audience { get; init; } = string.Empty; // Público-alvo do token
    public string SecretKey { get; init; } = string.Empty; // Chave secreta para assinatura do token
    public int TokenExpirationInMinutes { get; init; } = 120; // Tempo de expiração do token em minutos
}