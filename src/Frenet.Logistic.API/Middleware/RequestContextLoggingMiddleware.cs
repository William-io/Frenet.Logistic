using NLog;

namespace Frenet.Logistic.API.Middleware;

internal sealed class RequestContextLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public RequestContextLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Logger.Info("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);
        await _next(context);

        Logger.Info("Finished handling request:");
    }
}
