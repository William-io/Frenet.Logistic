using Frenet.Logistic.Application.Orders.GetOrder;
using Frenet.Logistic.Application.Orders.ProcessOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frenet.Logistic.API.Controllers.Orders;

[ApiController]
[Route("api/orders")]
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
        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ProcessOrder(
        ProcessOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new ProcessOrderCommand(
            request.DispatchId,
            request.CustomerId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetOrder), new { id = result.Value }, result.Value);
    }
}
