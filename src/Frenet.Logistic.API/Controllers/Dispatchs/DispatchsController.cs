using Frenet.Logistic.Application.Dispatchs.SearchDispatchs;
using Frenet.Logistic.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frenet.Logistic.API.Controllers.Dispatchs;

[Authorize]
[ApiController]
[Route("api/dispatchs")]
public class DispatchsController : ControllerBase
{
    private readonly ISender _mediator;

    public DispatchsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> SearchDispatchs(Guid Id, CancellationToken cancellationToken)
    {
        var query = new SearchDispatchsQuery(Id);

        Result<IReadOnlyList<DispatchResponse>> result = await _mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }
}
