using System.Diagnostics;
using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.UnitTests.Bases;

public class BaseTesting
{
    public static T AfirmarQueEventoDominioFoiExecutado<T>(Entity entity) where T : IDomainEvent
    {
        var allEvents = entity.GetDomainEvents().ToList();

        // Informação para visualização no debug
        Debug.WriteLine($"[DEBUG] Eventos presentes na entidade ({entity.GetType().Name}):");
        foreach (var evt in allEvents)
        {
            Debug.WriteLine($"[DEBUG] - {evt.GetType().Name} em {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
        }

        var domainEvent = allEvents.OfType<T>().SingleOrDefault();

        if (domainEvent == null)
        {
            var eventosExistentes = string.Join(", ", allEvents.Select(e => e.GetType().Name));

            throw new Exception(
                $"O evento de domínio {typeof(T).Name} não foi executado. " +
                $"Eventos encontrados: {(string.IsNullOrEmpty(eventosExistentes) ? "Nenhum" : eventosExistentes)}");
        }

        Debug.WriteLine($"[DEBUG] Evento {typeof(T).Name} encontrado e validado com sucesso!");

        return domainEvent;
    }
}
