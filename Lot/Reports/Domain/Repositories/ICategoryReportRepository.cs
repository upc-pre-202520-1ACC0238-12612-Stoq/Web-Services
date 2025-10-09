using Lot.Reports.Domain.Model.Aggregates;

namespace Lot.Reports.Domain.Repositories;

public interface ICategoryReportRepository
{
    Task<IEnumerable<CategoryReport>> FindAllAsync();
    Task<IEnumerable<CategoryReport>> FindByDateAsync(DateTime fechaConsulta);
    Task AddAsync(CategoryReport report);
    Task<CategoryReport?> FindByIdAsync(int id);
}