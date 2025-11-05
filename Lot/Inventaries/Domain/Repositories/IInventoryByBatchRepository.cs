using Lot.Inventaries.Domain.Model.Aggregates;

namespace Lot.Inventaries.Domain.Repositories;

public interface IInventoryByBatchRepository
{
    Task<IEnumerable<InventoryByBatch>> ListAsync();
    Task<InventoryByBatch?> FindByIdAsync(int id);
    Task AddAsync(InventoryByBatch batch);
    Task DeleteAsync(int id);
}