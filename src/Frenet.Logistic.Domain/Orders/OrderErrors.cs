using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Orders;

public static class OrderErrors
{
    public static readonly Error NotProcessing = new(
        "Order.Processing",
        "Pedido não foi efetuado processamento!");
    
    public static readonly Error NotShipped = new(
        "Order.NotShipped",
        "Pedido não foi enviado!");
    
    public static readonly Error AlShipped = new(
        "Order.AlShipped",
        "Pedido já foi enviado!");
    
    public static readonly Error Overlap = new(
        "Order.Overlap",
        "O pedido atual está sobreposta a uma existente");
    
}