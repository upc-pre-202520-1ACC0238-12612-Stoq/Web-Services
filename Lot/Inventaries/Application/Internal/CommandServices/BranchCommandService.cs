//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Commands;
using Lot.Inventaries.Domain.Repositories;
using Lot.Inventaries.Domain.Services;
using Lot.Shared.Domain.Repositories;

namespace Lot.Inventaries.Application.Internal.CommandServices;

public class BranchCommandService(
    IBranchRepository repository,
    IUnitOfWork unitOfWork) : IBranchCommandService
{
    public async Task<Branch?> Handle(CreateBranchCommand command)
    {
        var branch = new Branch(
            command.Name,
            command.Type,
            command.Address,
            command.Latitude,
            command.Longitude,
            command.StockTotal
        );

        try
        {
            await repository.AddAsync(branch);
            await unitOfWork.CompleteAsync();
            return branch;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Branch?> Handle(UpdateBranchCommand command)
    {
        var branch = await repository.FindByIdAsync(command.Id);
        if (branch == null) return null;

        branch.Update(
            command.Name,
            command.Type,
            command.Address,
            command.Latitude,
            command.Longitude
        );

        try
        {
            await repository.UpdateAsync(branch);
            await unitOfWork.CompleteAsync();
            return branch;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await repository.FindByIdAsync(id);
        if (entity == null) return false;
        await repository.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> UpdateStockAsync(int branchId, int stockTotal)
    {
        var branch = await repository.FindByIdAsync(branchId);
        if (branch == null) return false;

        branch.UpdateStock(stockTotal);

        try
        {
            await repository.UpdateAsync(branch);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

