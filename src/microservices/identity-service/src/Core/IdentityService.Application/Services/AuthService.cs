using IdentityService.Application.Contracts;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;

namespace IdentityService.Application.Services;

public class AuthService(IUserRepository userRepository) : IAuthService
{
    public async Task<string> Login(LoginRequest request, CancellationToken ct)
    {
        var user =await userRepository.GetByEmailAsync(request.Email, ct);
        if (user == null)
            return "This email is not registered.";
        
        if (user.Password != request.Password)
            return "Incorrect password.";
        
        return "Login successful.";
    }

    public async Task<string> Register(RegisterRequest request, CancellationToken ct)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email, ct);
        if (existingUser != null)
            return "This email is already registered.";
        
        var user = new User
        {
            Email = request.Email,
            Password = request.Password
        };
        
        userRepository.AddAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);
        
        return "Registration successful.";
    }

    public Task<string> Logout()
    {
        return Task.FromResult("Logout successful.");
    }
}