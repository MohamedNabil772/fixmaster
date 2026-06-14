using FixMaster.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FixMaster.Identity.Application.Users.Commands.UpdateProfile;

public record UpdateProfileCommand(
    string UserId,
    string FirstName,
    string LastName,
    string? ProfilePictureUrl) : IRequest;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
{
    private readonly UserManager<User> _userManager;

    public UpdateProfileCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        
        if (request.ProfilePictureUrl != null)
        {
            user.ProfilePictureUrl = request.ProfilePictureUrl;
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception("Failed to update profile: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
