using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lot.Inventaries.Infraestructure.Persistence.EFC.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly AppDbContext _context;

    public BranchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Branch>> ListAsync()
    {
        return await _context.Set<Branch>().ToListAsync();
    }

    public async Task<Branch?> FindByIdAsync(int id)
    {
        return await _context.Set<Branch>().FindAsync(id);
    }

    public async Task AddAsync(Branch branch)
    {
        await _context.Set<Branch>().AddAsync(branch);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Branch branch)
    {
        _context.Set<Branch>().Update(branch);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<Branch>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<Branch>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

