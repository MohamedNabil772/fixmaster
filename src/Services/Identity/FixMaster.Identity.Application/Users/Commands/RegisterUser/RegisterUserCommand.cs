using FixMaster.Identity.Application.Common.Interfaces;
using FixMaster.Identity.Application.Common.Models;
using FixMaster.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FixMaster.Identity.Application.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(string Email, string Password, string FirstName, string LastName, string Role = "Client") : IRequest<AuthResponse>;

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterUserCommandHandler(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            // Assign role
            await _userManager.AddToRoleAsync(user, request.Role);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return new AuthResponse(
                Guid.Parse(user.Id),
                user.Email,
                user.FirstName,
                user.LastName,
                request.Role,
                token);
        }
    }
}
