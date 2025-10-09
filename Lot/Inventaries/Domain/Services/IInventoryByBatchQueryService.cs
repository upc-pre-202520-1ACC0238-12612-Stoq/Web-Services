//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Queries;

namespace Lot.Inventaries.Domain.Services;


public interface IInventoryByBatchQueryService
{
    Task<IEnumerable<InventoryByBatch>> Handle(GetInventoryByBatchQuery query);
}
