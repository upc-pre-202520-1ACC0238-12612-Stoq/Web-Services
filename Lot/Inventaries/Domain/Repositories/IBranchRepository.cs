//L
using Lot.Inventaries.Domain.Model.Aggregates;

namespace Lot.Inventaries.Domain.Repositories;

public interface IBranchRepository
{
    Task<IEnumerable<Branch>> ListAsync();
    Task<Branch?> FindByIdAsync(int id);
    Task AddAsync(Branch branch);
    Task UpdateAsync(Branch branch);
    Task DeleteAsync(int id);
}

