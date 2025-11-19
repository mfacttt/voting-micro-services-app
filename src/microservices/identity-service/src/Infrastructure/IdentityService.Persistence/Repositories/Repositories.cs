using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Persistence.Repositories;

public sealed class UserRepository(IdentityDbContext context) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return context.Users.FirstOrDefaultAsync(x => x.Email == email, ct);
    }

    public void AddAsync(User user, CancellationToken ct = default)
    {
        context.Users.Add(user);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return context.SaveChangesAsync(ct);
    }
}