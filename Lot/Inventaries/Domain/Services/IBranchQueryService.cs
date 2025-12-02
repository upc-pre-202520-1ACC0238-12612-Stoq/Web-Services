//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Queries;

namespace Lot.Inventaries.Domain.Services;

public interface IBranchQueryService
{
    Task<IEnumerable<Branch>> Handle(GetBranchQuery query);
    Task<Branch?> GetByIdAsync(int id);
}

