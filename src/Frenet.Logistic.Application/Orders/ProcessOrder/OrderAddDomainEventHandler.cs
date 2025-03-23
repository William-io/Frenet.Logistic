using Frenet.Logistic.Application.Abstractions.Email;
using Frenet.Logistic.Domain.Customers;
using Frenet.Logistic.Domain.Orders;
using Frenet.Logistic.Domain.Orders.Events;
using MediatR;

namespace Frenet.Logistic.Application.Orders.ProcessOrder;

internal sealed class OrderAddDomainEventHandler : INotificationHandler<OrderingProcessingDomainEvent>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailService _emailService;
    
    public OrderAddDomainEventHandler(
        ICustomerRepository customerRepository, 
        IOrderRepository orderRepository,
        IEmailService emailService)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
        _emailService = emailService;
    }
    
    public async Task Handle(OrderingProcessingDomainEvent notification, CancellationToken cancellationToken)
    {
        Order? ordering = await _orderRepository.GetByIdAsync(notification.OrderId, cancellationToken);

        if (ordering is null)
            return;
        
        Customer? customer = await _customerRepository.GetByIdAsync(ordering.CustomerId, cancellationToken);

        if (customer is null)
            return;
        
        /*
         * Como tinha o campo EMAIl, resolvir implementar uma solução para o envio de email
         * (OPCIONAL-NAO-REQUERIDO)
         */
        await _emailService.SendAsync(
            customer.Email,
            "Pedido realizado!",
            "Você tem alguns minutos para confirmar seu pedido!");
    }
}