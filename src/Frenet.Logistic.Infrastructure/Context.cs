using Frenet.Logistic.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Frenet.Logistic.Infrastructure;

public sealed class Context : DbContext, IUnitOfWork
{

    public Context(DbContextOptions options) : base(options)
    {
        
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}

