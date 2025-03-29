using Asp.Versioning;
using Frenet.Logistic.Application.Customers.GetCustomerById;
using Frenet.Logistic.Application.Customers.LoginCustomer;
using Frenet.Logistic.Application.Customers.RegisterCustomer;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frenet.Logistic.API.Controllers.Customers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ISender _sender;

    public CustomersController(ISender sender)
    {
        _sender = sender;
    }

    [HasPermission(Permission.ReadMember)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCustomerById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCustomerByIdQuery(id);

        Result<CustomerResponse> result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
       RegisterCustomerRequest request,
       CancellationToken cancellationToken)
    {
        var command = new RegisterCustomerCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Password,
            request.Address);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCustomerCommand request, CancellationToken cancellationToken)
    {
        var command = new LoginCustomerCommand(request.Email);

        Result<string> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }

        return Ok(result.Value);
    }

}
