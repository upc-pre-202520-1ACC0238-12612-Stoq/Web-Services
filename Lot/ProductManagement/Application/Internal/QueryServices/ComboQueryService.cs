using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Model.Queries;
using Lot.ProductManagement.Domain.Repositories;
using Lot.ProductManagement.Domain.Services;

namespace Lot.ProductManagement.Application.Internal.QueryServices;

/// <summary>
/// Combo query service implementation
/// </summary>
public class ComboQueryService(IComboRepository comboRepository) : IComboQueryService
{
    public async Task<Combo?> Handle(GetComboByIdQuery query)
    {
        return await comboRepository.FindComboWithRelationsAsync(query.Id);
    }

    public async Task<IEnumerable<Combo>> Handle(GetAllCombosQuery query)
    {
        return await comboRepository.FindAllCombosAsync();
    }
}