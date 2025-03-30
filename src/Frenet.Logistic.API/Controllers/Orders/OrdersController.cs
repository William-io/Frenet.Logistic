﻿using Asp.Versioning;
using Frenet.Logistic.Application.Orders.CancelOrder;
using Frenet.Logistic.Application.Orders.CompleteOrder;
using Frenet.Logistic.Application.Orders.ConfirmOrder;
using Frenet.Logistic.Application.Orders.DeleteOrder;
using Frenet.Logistic.Application.Orders.GetAllOrder;
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

    [HttpGet]
    public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
    {
        var query = new GetAllOrderQuery();

        Result<IReadOnlyList<GetAllOrderResponse>> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
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

    [HttpPut("confirm")]
    public async Task<IActionResult> ConfirmOrder(
        ConfirmOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new ConfirmOrderCommand(request.OrderId);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)       
            return BadRequest(result.Error);    

        return Ok(result);
    }

    [HttpPut("complete")]
    public async Task<IActionResult> CompleteOrder(
        CompleteOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CompleteOrderCommand(request.OrderId);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)      
            return BadRequest(result.Error);
        
        return Ok(result);
    }

    [HttpPut("cancel")]
    public async Task<IActionResult> CancelOrder(
        CancelOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CancelOrderCommand(request.OrderId);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteOrderCommand(id);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result);
    }
}
