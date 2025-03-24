using FluentValidation;

namespace Frenet.Logistic.Application.Orders.ProcessOrder;

public  class ProcessOrderCommandValidator : AbstractValidator<ProcessOrderCommand>
{
    public ProcessOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.DispatchId).NotEmpty();
        RuleFor(x => x.ProssedDate).NotEmpty();
    }
}
