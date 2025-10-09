using Lot.Reports.Domain.Model.Aggregates;
using Lot.Reports.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;

namespace Lot.Reports.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Stock Average Report repository implementation
/// </summary>
public class StockAverageReportRepository(AppDbContext context)
    : BaseRepository<StockAverageReport>(context), IStockAverageReportRepository
{
    public async Task<StockAverageReport> AddAsync(StockAverageReport report)
    {
        await Context.Set<StockAverageReport>().AddAsync(report);
        return report;
    }

    public async Task<IEnumerable<StockAverageReport>> FindAllAsync()
    {
        return await Context.Set<StockAverageReport>().ToListAsync();
    }

    public async Task<IEnumerable<StockAverageReport>> FindByDateAsync(DateTime fecha)
    {
        return await Context.Set<StockAverageReport>()
            .Where(r => r.FechaConsulta.Date == fecha.Date)
            .ToListAsync();
    }
    
    public async Task<StockAverageReport?> FindByIdAsync(int id) =>
        await Context.Set<StockAverageReport>().FindAsync(id);
}