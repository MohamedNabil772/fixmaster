using FixMaster.Bidding.Application.Statistics.Queries.GetAdminStats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixMaster.Bidding.API.Controllers;

[Authorize(Roles = "Admin,SuperAdmin")]
[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("admin")]
    public async Task<ActionResult<AdminStatsDto>> GetAdminStats([FromQuery] string? filterType, [FromQuery] string? service)
    {
        return Ok(await _mediator.Send(new GetAdminStatsQuery(filterType, service)));
    }
}
