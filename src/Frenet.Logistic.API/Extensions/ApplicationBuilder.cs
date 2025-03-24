using Frenet.Logistic.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Frenet.Logistic.API.Extensions;

internal static class ApplicationBuilder
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using Context dbContext = scope.ServiceProvider.GetRequiredService<Context>();

        dbContext.Database.Migrate();
    }

    //public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    //{
    //    app.UseMiddleware<ExceptionHandlingMiddleware>();
    //}

    //public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    //{
    //    app.UseMiddleware<RequestContextLoggingMiddleware>();

    //    return app;
    //}
}
