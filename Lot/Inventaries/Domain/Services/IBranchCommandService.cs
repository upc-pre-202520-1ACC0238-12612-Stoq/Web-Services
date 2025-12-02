//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Commands;

namespace Lot.Inventaries.Domain.Services;

public interface IBranchCommandService
{
    Task<Branch?> Handle(CreateBranchCommand command);
    Task<Branch?> Handle(UpdateBranchCommand command);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateStockAsync(int branchId, int stockTotal);
}

