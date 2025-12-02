//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Domain.Model.Commands;
using Lot.Inventaries.Interfaces.REST.Resources;

namespace Lot.Inventaries.Interfaces.REST.Transform;

public static class BranchResourceAssembler
{
    public static BranchResource ToResource(Branch branch)
    {
        return new BranchResource
        {
            Id = branch.Id,
            Name = branch.Name,
            Type = branch.Type,
            Address = branch.Address,
            Latitude = branch.Latitude,
            Longitude = branch.Longitude,
            StockTotal = branch.StockTotal,
            CreatedAt = branch.CreatedAt,
            UpdatedAt = branch.UpdatedAt
        };
    }

    public static CreateBranchCommand ToCommandFromResource(CreateBranchResource resource)
    {
        return new CreateBranchCommand(
            resource.Name,
            resource.Type,
            resource.Address,
            resource.Latitude,
            resource.Longitude,
            resource.StockTotal
        );
    }
}

