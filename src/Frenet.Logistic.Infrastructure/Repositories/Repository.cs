﻿using Frenet.Logistic.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Frenet.Logistic.Infrastructure.Repositories;

internal abstract class Repository<T> where T : Entity
{
    protected readonly Context _context;

    protected Repository(Context context) => _context = context;

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Set<T>()
            .FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken);
    }

    public virtual void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

}