using FixMaster.Bidding.Application.Requests.Commands.CreateRequest;
using FixMaster.Bidding.Application.Requests.Queries.GetRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixMaster.Bidding.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ServiceRequestDto>>> GetAll([FromQuery] string? category)
    {
        var result = await _mediator.Send(new GetRequestsQuery(category));
        return Ok(result);
    }

    [Authorize(Roles = "Client")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequestCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
