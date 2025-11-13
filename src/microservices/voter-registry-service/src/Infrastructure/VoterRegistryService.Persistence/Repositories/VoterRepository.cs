using Microsoft.EntityFrameworkCore;
using VoterRegistryService.Domain.Entities;
using VoterRegistryService.Domain.Repositories;
using VoterRegistryService.Persistence.Context;

namespace VoterRegistryService.Persistence.Repositories;

public class VoterRepository(VoterRegistryDbContext dbContext) : IVoterRepository
{
    public void CreateAsync(Voter voter, CancellationToken ct)
    {
        dbContext.Voters.Add(voter);
    }

    public void UpdateAsync(Voter voter, CancellationToken ct)
    {
        dbContext.Voters.Update(voter);
    }

    public Task<Voter?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return dbContext.Voters.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public Task<Voter?> GetByNationalIdAsync(string nationalId, CancellationToken ct)
    {
        return dbContext.Voters.FirstOrDefaultAsync(x => x.NationalId == nationalId, ct);
    }

    public async Task<IReadOnlyList<Voter>> GetPagedAsync(int skip, int take, CancellationToken ct)
    {
        return await dbContext.Voters.AsNoTracking().Skip(skip).Take(take).ToListAsync(ct);
    }

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct)
    {
        return dbContext.Voters.AnyAsync(x => x.Id == id, ct);
    }

    public Task<bool> ExistsByNationalIdAsync(string nationalId, CancellationToken ct)
    {
        return dbContext.Voters.AnyAsync(x => x.NationalId == nationalId, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        return dbContext.SaveChangesAsync(ct);
    }
}