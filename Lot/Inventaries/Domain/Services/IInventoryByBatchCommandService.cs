//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Commands;

namespace Lot.Inventaries.Domain.Services;

public interface IInventoryByBatchCommandService
{
    Task<InventoryByBatch?> Handle(CreateInventoryByBatchCommand command);
    Task<bool> DeleteAsync(int id);
}