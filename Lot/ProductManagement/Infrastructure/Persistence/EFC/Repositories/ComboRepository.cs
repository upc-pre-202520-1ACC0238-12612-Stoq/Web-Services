using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Lot.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lot.ProductManagement.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Combo repository implementation
/// </summary>
public class ComboRepository(AppDbContext context) : BaseRepository<Combo>(context), IComboRepository
{
    public async Task<Combo?> FindComboByNameAsync(string name)
    {
        return await Context.Set<Combo>()
            .Include(c => c.ComboItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<IEnumerable<Combo>> FindAllCombosAsync()
    {
        return await Context.Set<Combo>()
            .Include(c => c.ComboItems)
            .ThenInclude(ci => ci.Product)
            .ToListAsync();
    }

    public async Task<Combo?> FindComboWithRelationsAsync(int id)
    {
        return await Context.Set<Combo>()
            .Include(c => c.ComboItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}