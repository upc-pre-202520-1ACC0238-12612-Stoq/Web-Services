using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Queries;
using Lot.Inventaries.Domain.Repositories;
using Lot.Inventaries.Domain.Services;

namespace Lot.Inventaries.Application.Internal.QuerysServices;

public class InventoryByBatchQueryService : IInventoryByBatchQueryService
{
    private readonly IInventoryByBatchRepository _repository;

    public InventoryByBatchQueryService(IInventoryByBatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<InventoryByBatch>> Handle(GetInventoryByBatchQuery query)
    {
        return await _repository.ListAsync();
    }
}