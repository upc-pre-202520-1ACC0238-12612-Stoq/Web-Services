using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Repositories;
using Lot.Inventaries.Domain.Services;

namespace Lot.Inventaries.Application.Internal.QuerysServices;

public class InventoryByProductQueryService : IInventoryByProductQueryService
{
    private readonly IInventoryByProductRepository _repository;

    public InventoryByProductQueryService(IInventoryByProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<InventoryByProduct>> GetAllAsync()
    {
        return await _repository.ListAsync();
    }

    public async Task<InventoryByProduct?> GetByIdAsync(int id)
    {
        return await _repository.FindByIdAsync(id);
    }
}