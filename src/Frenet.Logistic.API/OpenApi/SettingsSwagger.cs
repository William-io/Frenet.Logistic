using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Frenet.Logistic.API.OpenApi;

internal sealed class SettingsSwagger : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public SettingsSwagger(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    public void Configure(SwaggerGenOptions options)
    {
        // Adiciona documentação para cada versão de API descoberta
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
        }

        // Configuração de segurança JWT
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Digite 'Bearer' [espaço] e seu token. Exemplo: 'Bearer abcdef123456'",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        };


        options.AddSecurityDefinition("Bearer", securityScheme);

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });

        // Outras configurações como XML comments, etc
        var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
        foreach (var xmlFile in xmlFiles)
        {
            options.IncludeXmlComments(xmlFile);
        }
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Frenet Logistic API",
            Version = description.ApiVersion.ToString(),
            Description = "API para sistema de logística com recursos de rastreamento, despacho e gerenciamento de pedidos.",
            Contact = new OpenApiContact
            {
                Name = "API Support",
                Email = "williamgsilva@live.com"
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " Esta versão da API está obsoleta e será descontinuada em breve.";
        }

        return info;
    }
}

