//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Queries;
using Lot.Inventaries.Domain.Repositories;
using Lot.Inventaries.Domain.Services;

namespace Lot.Inventaries.Application.Internal.QuerysServices;

public class BranchQueryService(IBranchRepository repository) : IBranchQueryService
{
    public async Task<IEnumerable<Branch>> Handle(GetBranchQuery query)
    {
        var branches = await repository.ListAsync();

        if (query.Id.HasValue)
        {
            var branch = await repository.FindByIdAsync(query.Id.Value);
            return branch != null ? new[] { branch } : Enumerable.Empty<Branch>();
        }

        if (!string.IsNullOrEmpty(query.Type))
        {
            branches = branches.Where(b => b.Type == query.Type);
        }

        return branches;
    }

    public async Task<Branch?> GetByIdAsync(int id)
    {
        return await repository.FindByIdAsync(id);
    }
}

