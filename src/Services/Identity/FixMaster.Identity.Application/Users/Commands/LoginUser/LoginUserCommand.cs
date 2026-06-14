using FixMaster.Identity.Application.Common.Interfaces;
using FixMaster.Identity.Application.Common.Models;
using FixMaster.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FixMaster.Common.Exceptions;

namespace FixMaster.Identity.Application.Users.Commands.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : IRequest<AuthResponse>;

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return new AuthResponse(
                Guid.Parse(user.Id),
                user.Email!,
                user.FirstName,
                user.LastName,
                roles.FirstOrDefault() ?? "Client",
                token);
        }
    }
}
