using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Customers;

namespace Frenet.Logistic.Application.Customers;

internal sealed class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(
           new FirstName(request.FirstName),
           new LastName(request.LastName),
           new Email(request.Email),
           new Phone(request.Phone),
           new Address(
               request.Address.Country,
               request.Address.State,
               request.Address.ZipCode,
               request.Address.City,
               request.Address.Street));

        _customerRepository.Add(customer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id;

    }
}
