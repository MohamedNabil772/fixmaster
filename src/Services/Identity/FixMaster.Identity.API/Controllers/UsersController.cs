using FixMaster.Identity.Application.Users.Commands.RegisterUser;
using FixMaster.Identity.Application.Users.Commands.LoginUser;
using FixMaster.Identity.Application.Users.Queries.GetAllUsers;
using FixMaster.Identity.Application.Users.Commands.UpdateUserRole;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FixMaster.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("{userId}/role")]
        public async Task<IActionResult> UpdateRole(string userId, [FromBody] string newRole)
        {
            await _mediator.Send(new UpdateUserRoleCommand(userId, newRole));
            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
