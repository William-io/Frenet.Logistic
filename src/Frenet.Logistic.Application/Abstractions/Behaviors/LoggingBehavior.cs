using MediatR;
using Microsoft.Extensions.Logging;

namespace Frenet.Logistic.Application.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation("Executando comando {Command}", name);
            var result = await next();

            _logger.LogInformation("Comando {Command} executado com sucesso", name);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erro ao executar comando {Command}", name);
            throw;
        }
    }
}
