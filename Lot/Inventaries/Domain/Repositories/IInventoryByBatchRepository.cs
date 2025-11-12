using Lot.Inventaries.Domain.Model.Aggregates;

namespace Lot.Inventaries.Domain.Repositories;

public interface IInventoryByBatchRepository
{
    Task<IEnumerable<InventoryByBatch>> ListAsync();
    Task<InventoryByBatch?> FindByIdAsync(int id);
    Task AddAsync(InventoryByBatch batch);
    Task DeleteAsync(int id);

    // NUEVOS: Métodos de búsqueda por nombre
    Task<Lot.ProductManagement.Domain.Model.Aggregates.Product?> FindProductByNameAsync(string nombre, string? categoria = null);
    Task<IEnumerable<Lot.ProductManagement.Domain.Model.Aggregates.Product>> SearchProductsByNameAsync(string nombre, string? categoria = null);
}