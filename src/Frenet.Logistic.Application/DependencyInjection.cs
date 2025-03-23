using Frenet.Logistic.Domain.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace Frenet.Logistic.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });
        
        services.AddTransient<ShippingPriceService>();
        
        return services;
    }
}