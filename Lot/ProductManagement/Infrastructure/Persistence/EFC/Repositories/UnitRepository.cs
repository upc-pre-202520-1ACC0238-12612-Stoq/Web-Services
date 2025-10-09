using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Lot.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lot.ProductManagement.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Unit repository implementation
/// </summary>
public class UnitRepository(AppDbContext context) : BaseRepository<Unit>(context), IUnitRepository
{
    public async Task<Unit?> FindUnitByNameAsync(string name)
    {
        return await Context.Set<Unit>()
            .FirstOrDefaultAsync(u => u.Name == name);
    }

    public async Task<Unit?> FindUnitByAbbreviationAsync(string abbreviation)
    {
        return await Context.Set<Unit>()
            .FirstOrDefaultAsync(u => u.Abbreviation == abbreviation);
    }

    public async Task<IEnumerable<Unit>> FindAllUnitsAsync()
    {
        return await Context.Set<Unit>()
            .ToListAsync();
    }
} 