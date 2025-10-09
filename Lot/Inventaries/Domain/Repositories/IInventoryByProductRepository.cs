//  L
using Lot.Inventaries.Domain.Model.Aggregates;

namespace Lot.Inventaries.Domain.Repositories;

public interface IInventoryByProductRepository
{
    Task<IEnumerable<InventoryByProduct>> ListAsync();
    Task<InventoryByProduct?> FindByIdAsync(int id);
    Task AddAsync(InventoryByProduct product  );
    Task DeleteAsync(int id);
}

