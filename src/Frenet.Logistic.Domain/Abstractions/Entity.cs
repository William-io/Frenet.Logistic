namespace Frenet.Logistic.Domain.Abstractions;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    
    public Entity(Guid id) => Id = id;
    public Guid Id { get; init; }

    #region Metodo para tratar os eventos do dominio,
    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    //Acionar partindo de alteracao
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    #endregion
}