using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    void AddAsync(User user, CancellationToken ct = default);
    
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}