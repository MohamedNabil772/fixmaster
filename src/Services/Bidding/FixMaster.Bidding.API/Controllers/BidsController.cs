using FixMaster.Bidding.Application.Bids.Commands.SubmitBid;
using FixMaster.Bidding.Application.Bids.Queries.GetBidsByRequest;
using FixMaster.Bidding.Application.Requests.Commands.SelectMaster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixMaster.Bidding.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BidsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("request/{requestId}")]
    public async Task<ActionResult<IEnumerable<BidResponse>>> GetByRequest(Guid requestId)
    {
        return Ok(await _mediator.Send(new GetBidsByRequestQuery(requestId)));
    }

    [Authorize(Roles = "Master")]
    [HttpPost]
    public async Task<ActionResult<Guid>> SubmitBid(SubmitBidCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Roles = "Client")]
    [HttpPost("select-master")]
    public async Task<ActionResult> SelectMaster(SelectMasterCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
