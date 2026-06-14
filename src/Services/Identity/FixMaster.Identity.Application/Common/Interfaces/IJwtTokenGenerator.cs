using FixMaster.Identity.Domain.Entities;

namespace FixMaster.Identity.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, IEnumerable<string> roles);
}
