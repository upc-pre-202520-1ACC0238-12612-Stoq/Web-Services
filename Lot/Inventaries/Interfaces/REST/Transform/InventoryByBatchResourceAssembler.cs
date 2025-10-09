//L

using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Interfaces.REST.Resources;

namespace Lot.Inventaries.Interfaces.REST.Transform;

public static class InventoryByBatchResourceAssembler
{
    public static InventoryByBatchResource ToResource(InventoryByBatch entity)
    {
        return new InventoryByBatchResource
        {
            Id = entity.Id,
            Proveedor = entity.Proveedor,
            Producto = entity.Producto,
            FechaEntrada = entity.FechaEntrada,
            Cantidad = entity.Cantidad,
            Precio = entity.Precio,
            Unidad = entity.Unidad,
            Total = entity.Total
        };
    }
}
