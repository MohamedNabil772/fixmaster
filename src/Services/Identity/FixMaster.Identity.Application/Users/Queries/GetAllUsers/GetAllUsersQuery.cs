using FixMaster.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FixMaster.Identity.Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<List<UserDto>>;

public record UserDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    IList<string> Roles);

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly UserManager<User> _userManager;

    public GetAllUsersQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.ToListAsync(cancellationToken);
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userDtos.Add(new UserDto(
                user.Id,
                user.Email!,
                user.FirstName,
                user.LastName,
                roles));
        }

        return userDtos;
    }
}
