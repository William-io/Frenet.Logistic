using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Orders;

public static class OrderErrors
{
    public static readonly Error NotProcessing = new(
        "Order.Processing",
        "Pedido não foi efetuado processamento ou já solicitado para envio!");
    
    public static readonly Error NotShipped = new(
        "Order.NotShipped",
        "Pedido não foi enviado!");
    
    public static readonly Error AlShipped = new(
        "Order.AlShipped",
        "Pedido já foi enviado!");
    
    public static readonly Error Overlap = new(
        "Order.Overlap",
        "O pedido atual já foi solicitado!");

    public static readonly Error NotFound = new(
        "Order.Found",
        "Pedido com o identificador especificado não foi encontrada");

    public static readonly Error Cancelled = new(
       "Order.Cancelled",
       "Pedido foi cancelado!");

    public static readonly Error NotCancelled = new(
       "Order.NotCancelled",
       "Pedido não foi cancelado!");

}