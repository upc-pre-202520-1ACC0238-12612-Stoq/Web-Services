namespace Lot.Inventaries.Interfaces.REST.Resources;

public class InventoryGeneralResource
{
    public List<InventoryByProductResource> Productos { get; set; }
    public List<InventoryByBatchResource> Lotes { get; set; }
}

