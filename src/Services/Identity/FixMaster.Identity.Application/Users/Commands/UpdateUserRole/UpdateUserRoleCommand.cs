using FixMaster.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FixMaster.Identity.Application.Users.Commands.UpdateUserRole;

public record UpdateUserRoleCommand(string UserId, string NewRole) : IRequest;

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UpdateUserRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        if (!await _roleManager.RoleExistsAsync(request.NewRole))
        {
            throw new Exception("Role does not exist.");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, request.NewRole);
    }
}
