namespace FixMaster.Identity.Application.Common.Models;

public record AuthResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string Role,
    string Token);
