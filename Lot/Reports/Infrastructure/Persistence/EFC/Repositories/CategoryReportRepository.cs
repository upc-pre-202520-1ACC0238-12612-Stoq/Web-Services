using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;

namespace Lot.Reports.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Category Report repository implementation
/// </summary>
public class CategoryReportRepository(AppDbContext context)
    : BaseRepository<CategoryReport>(context), ICategoryReportRepository
{
    public async Task<CategoryReport> AddAsync(CategoryReport report)
    {
        await Context.Set<CategoryReport>().AddAsync(report);
        return report;
    }

    public async Task<IEnumerable<CategoryReport>> FindAllAsync()
    {
        return await Context.Set<CategoryReport>().ToListAsync();
    }

    public async Task<IEnumerable<CategoryReport>> FindByDateAsync(DateTime fecha)
    {
        return await Context.Set<CategoryReport>()
            .Where(r => r.FechaConsulta.Date == fecha.Date)
            .ToListAsync();
    }

    public async Task<CategoryReport?> FindByIdAsync(int id) =>
        await Context.Set<CategoryReport>().FindAsync(id);

}