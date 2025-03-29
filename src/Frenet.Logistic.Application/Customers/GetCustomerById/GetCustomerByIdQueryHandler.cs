using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Customers;

namespace Frenet.Logistic.Application.Customers.GetCustomerById;

internal sealed class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, CustomerResponse>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (customer is null)
        {
            return Result.Failure<CustomerResponse>(new Error(
              "Member.NotFound",
              $"Cliente {request.Id} não localizado!"));
        }

        return Result.Success(new CustomerResponse(
            customer.Id,
            customer.FirstName.Value,
            customer.LastName.Value,
            customer.Email.Value,
            customer.Phone.Value,
            customer.Address));
    }
}
