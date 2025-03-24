using Frenet.Logistic.Application.Exceptions;
using Frenet.Logistic.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Frenet.Logistic.Infrastructure;

public sealed class Context : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;

    public Context(DbContextOptions options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            await PublishDomainEventsAsync();

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {

            throw new ConcurrencyException("Concurrency exception occurred!", ex);
        }
      
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Select(x => x.Entity)
            .SelectMany(e =>
            {
                var domainEvents = e.GetDomainEvents();
                e.ClearDomainEvents();
                return domainEvents;
            }).ToList();

        foreach (var entity in domainEntities)
        {
            await _publisher.Publish(entity);
        }

    }

}

