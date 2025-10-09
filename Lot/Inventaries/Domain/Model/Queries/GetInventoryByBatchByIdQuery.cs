namespace Lot.Inventaries.Domain.Model.Queries;


public class GetInventoryByIdQuery
{
    public int InventoryId { get; }

    public GetInventoryByIdQuery(int inventoryId)
    {
        InventoryId = inventoryId;
    }
}