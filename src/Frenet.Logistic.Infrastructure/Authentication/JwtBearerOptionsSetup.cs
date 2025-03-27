using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Frenet.Logistic.Infrastructure.Authentication;

internal sealed class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly AutenticationOptions _autenticationOptions;

    public JwtBearerOptionsSetup(IOptions<AutenticationOptions> autenticationOptions)
    {
        _autenticationOptions = autenticationOptions.Value;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Desative se não estiver usando Issuer
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = _autenticationOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_autenticationOptions.SecretKey)),
            ClockSkew = TimeSpan.Zero // Remove tolerância de tempo para expiração
        };
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
} 
