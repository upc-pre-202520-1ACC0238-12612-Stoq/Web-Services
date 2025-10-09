using Lot.ProductManagement.Domain.Model.Aggregates;

namespace Lot.ProductManagement.Domain.Services;

public interface IUnitQueryService
{
    Task<IEnumerable<Unit>> GetAllAsync();
} 