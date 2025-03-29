using Asp.Versioning;
using Dapper;
using Frenet.Logistic.Application.Abstractions.Clock;
using Frenet.Logistic.Application.Abstractions.DataFactory;
using Frenet.Logistic.Application.Abstractions.Email;
using Frenet.Logistic.Application.Authentication;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Customers;
using Frenet.Logistic.Domain.Dispatchs;
using Frenet.Logistic.Domain.Orders;
using Frenet.Logistic.Infrastructure.Authentication;
using Frenet.Logistic.Infrastructure.Clock;
using Frenet.Logistic.Infrastructure.Data;
using Frenet.Logistic.Infrastructure.Email;
using Frenet.Logistic.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frenet.Logistic.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        var connectionString = configuration.GetConnectionString("Database") ??
            throw new ArgumentNullException(nameof(configuration),"Database connection string não localizada!");


        services.AddDbContext<Context>(options =>
        {
            options.UseSqlServer(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IDispatchRepository, DispatchRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<IUnitOfWork>(ui => ui.GetRequiredService<Context>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DateHandler());


        #region
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddTransient<IPermissionService, PermissionService>();
        #endregion

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
