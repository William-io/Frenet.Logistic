using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Orders;

public static class OrderErros
{
    public static readonly Error NotProcessing = new(
        "Order.Processing",
        "Pedido não foi efetuado processamento!");
    
}