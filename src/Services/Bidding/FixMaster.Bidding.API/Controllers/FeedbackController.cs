using FixMaster.Bidding.Application.Feedback.Commands.SubmitFeedback;
using FixMaster.Bidding.Application.Feedback.Queries.GetProviderRating;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixMaster.Bidding.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly IMediator _mediator;

    public FeedbackController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Client")]
    [HttpPost]
    public async Task<ActionResult<Guid>> Submit(SubmitFeedbackCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpGet("provider/{masterId}")]
    public async Task<ActionResult<ProviderRatingDto>> GetRating(Guid masterId)
    {
        return Ok(await _mediator.Send(new GetProviderRatingQuery(masterId)));
    }
}
