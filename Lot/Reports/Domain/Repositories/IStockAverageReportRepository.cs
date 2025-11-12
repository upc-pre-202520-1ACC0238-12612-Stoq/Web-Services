using Lot.Reports.Domain.Model.Aggregates;

namespace Lot.Reports.Domain.Repositories;

public interface IStockAverageReportRepository
{
    Task AddAsync(StockAverageReport report);
    Task<IEnumerable<StockAverageReport>> FindAllAsync();
    Task<IEnumerable<StockAverageReport>> FindByDateAsync(DateTime fecha);
    Task<StockAverageReport?> FindByIdAsync(int id);
}