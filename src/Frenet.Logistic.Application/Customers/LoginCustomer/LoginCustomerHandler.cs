using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Application.Authentication;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Customers;

namespace Frenet.Logistic.Application.Customers.LoginCustomer;

internal sealed class LoginCustomerHandler : ICommandHandler<LoginCustomerCommand, string>
{
    private readonly IJwtProvider _jwtProvider;
    private readonly ICustomerRepository _customerRepository;

    public LoginCustomerHandler(IJwtProvider jwtProvider, ICustomerRepository customerRepository)
    {
        _jwtProvider = jwtProvider;
        _customerRepository = customerRepository;
    }

    public async Task<Result<string>> Handle(LoginCustomerCommand request, CancellationToken cancellationToken)
    {
        Result<Email> email = Email.Create(request.Email);

        Customer? customer = await _customerRepository.GetByEmailAsync(email.Value, cancellationToken);

        if (customer is null)
        {
            return Result.Failure<string>(
                Error.Email.InvalidCredentials);
        }

        string token = _jwtProvider.Generate(customer);

        return Result.Success(token);
    }
}
