using MediatR;

namespace FixMaster.Identity.Application.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(string Email, string Password, string FirstName, string LastName) : IRequest<Guid>;

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        public Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Basic implementation for now
            var userId = Guid.NewGuid();
            return Task.FromResult(userId);
        }
    }
}
