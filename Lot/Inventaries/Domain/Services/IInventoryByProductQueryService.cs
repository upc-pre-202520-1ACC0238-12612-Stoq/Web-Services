//L
using Lot.Inventaries.Domain.Model.Aggregates;

namespace Lot.Inventaries.Domain.Services;

public interface IInventoryByProductQueryService
{
    Task<IEnumerable<InventoryByProduct>> GetAllAsync();
    Task<InventoryByProduct?> GetByIdAsync(int id);
}
