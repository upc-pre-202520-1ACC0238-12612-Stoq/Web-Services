using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Repositories;
using Lot.ProductManagement.Domain.Services;

namespace Lot.ProductManagement.Application.Internal.QueryServices;

public class UnitQueryService : IUnitQueryService
{
    private readonly IUnitRepository _unitRepository;

    public UnitQueryService(IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository;
    }

    public async Task<IEnumerable<Unit>> GetAllAsync()
    {
        return await _unitRepository.FindAllUnitsAsync();
    }
} 