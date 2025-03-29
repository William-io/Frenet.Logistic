using FluentValidation;

namespace Frenet.Logistic.Application.Customers.RegisterCustomer;

internal sealed class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Phone).NotEmpty();
        RuleFor(x => x.Password).MinimumLength(5);
        RuleFor(x => x.Address.ZipCode).NotEmpty();
    }
}
