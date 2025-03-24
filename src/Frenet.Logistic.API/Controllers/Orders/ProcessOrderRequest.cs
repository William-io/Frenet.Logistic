namespace Frenet.Logistic.API.Controllers.Orders;

public sealed record ProcessOrderRequest(Guid DispatchId, Guid CustomerId);