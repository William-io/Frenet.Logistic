using Asp.Versioning;
using Frenet.Logistic.Application.Dispatchs.GetAllDispatchs;
using Frenet.Logistic.Application.Dispatchs.SearchDispatchs;
using Frenet.Logistic.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frenet.Logistic.API.Controllers.Dispatchs;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DispatchsController : ControllerBase
{
    private readonly ISender _mediator;

    public DispatchsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDispatchs(CancellationToken cancellationToken)
    {
        var query = new GetAllDispatchsQuery();

        Result<IReadOnlyList<GetAllDispatchsResponse>> result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> SearchDispatchs(Guid id, CancellationToken cancellationToken)
    {
        var query = new SearchDispatchsQuery(id);

        Result<IReadOnlyList<DispatchResponse>> result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        return Ok(result.Value);
    }
}
