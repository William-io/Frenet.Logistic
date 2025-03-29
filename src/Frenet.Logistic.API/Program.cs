using Asp.Versioning.ApiExplorer;
using Frenet.Logistic.API.Extensions;
using Frenet.Logistic.API.OpenApi;
using Frenet.Logistic.Application;
using Frenet.Logistic.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.ConfigureOptions<SettingsSwagger>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // Obter o provedor de descrição de versões da API
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwaggerUI(c =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }

        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Frenet Logistic API v1");
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // Opção para não expandir todos os endpoints
        c.EnableDeepLinking(); // Permite links diretos para operações específicas
        c.DisplayRequestDuration(); // Mostra a duração das requisições
    });

    app.ApplyMigrations();

    //Remover para interromper a execução do seed;
    //app.SeedData();
}

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
