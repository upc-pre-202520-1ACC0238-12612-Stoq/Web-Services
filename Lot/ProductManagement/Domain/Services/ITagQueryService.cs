using Lot.ProductManagement.Domain.Model.Aggregates;

namespace Lot.ProductManagement.Domain.Services;

public interface ITagQueryService
{
    Task<IEnumerable<Tag>> GetAllAsync();
} 