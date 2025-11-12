//  L
using Lot.Inventaries.Domain.Model.Aggregates;

namespace Lot.Inventaries.Domain.Repositories;

public interface IInventoryByProductRepository
{
    Task<IEnumerable<InventoryByProduct>> ListAsync();
    Task<InventoryByProduct?> FindByIdAsync(int id);
    Task AddAsync(InventoryByProduct product);
    Task UpdateAsync(InventoryByProduct product);
    Task DeleteAsync(int id);

    // NUEVOS: Métodos de búsqueda por nombre
    Task<Lot.ProductManagement.Domain.Model.Aggregates.Product?> FindProductByNameAsync(string nombre, string? categoria = null);
    Task<IEnumerable<Lot.ProductManagement.Domain.Model.Aggregates.Product>> SearchProductsByNameAsync(string nombre, string? categoria = null);
}

