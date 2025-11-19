using IdentityService.Application.Contracts;

namespace IdentityService.Application.Services;

public interface IAuthService
{
    Task<string> Login(LoginRequest request, CancellationToken ct);
    Task<string> Register(RegisterRequest request, CancellationToken ct);
    Task<string> Logout();
}