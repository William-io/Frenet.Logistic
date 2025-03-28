namespace Frenet.Logistic.Domain.Customers;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Customer?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);


    void Add(Customer user);
}