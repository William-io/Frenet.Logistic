using Asp.Versioning;
using Frenet.Logistic.Application.Orders.CancelOrder;
using Frenet.Logistic.Application.Orders.CompleteOrder;
using Frenet.Logistic.Application.Orders.ConfirmOrder;
using Frenet.Logistic.Application.Orders.GetOrder;
using Frenet.Logistic.Application.Orders.ProcessOrder;
using Frenet.Logistic.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frenet.Logistic.API.Controllers.Orders;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class OrdersController : ControllerBase
{

    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOrderQuery(id);

        Result<OrderResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessOrder(
        ProcessOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new ProcessOrderCommand(
            request.DispatchId,
            request.CustomerId);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)      
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmOrder(
        ConfirmOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new ConfirmOrderCommand(request.OrderId);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)       
            return BadRequest(result.Error);    

        return Ok(result);
    }

    [HttpPost("complete")]
    public async Task<IActionResult> CompleteOrder(
        CompleteOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CompleteOrderCommand(request.OrderId);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)      
            return BadRequest(result.Error);
        
        return Ok(result);
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> CancelOrder(
        CancelOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CancelOrderCommand(request.OrderId);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result);
    }
}
