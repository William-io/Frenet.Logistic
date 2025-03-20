namespace Frenet.Logistic.Domain.Cursomers;

public interface IUserRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Customer user);
}