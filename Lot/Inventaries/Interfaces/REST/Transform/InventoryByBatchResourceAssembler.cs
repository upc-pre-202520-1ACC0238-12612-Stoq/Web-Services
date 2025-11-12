//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Interfaces.REST.Resources;

namespace Lot.Inventaries.Interfaces.REST.Transform;

/// <summary>
/// Assembler para transformar Entities a Resources enriquecidos.
/// Convierte Entity a Resource usando datos de la relación FK con Products.
/// </summary>
public static class InventoryByBatchResourceAssembler
{
    public static InventoryByBatchResource ToResource(InventoryByBatch entity)
    {
        return new InventoryByBatchResource
        {
            Id = entity.Id,
            ProductoId = entity.ProductoId,
            ProductoNombre = entity.Product?.Name ?? string.Empty,
            UnidadNombre = entity.Unit?.Name ?? entity.Product?.Unit?.Name ?? string.Empty,
            UnidadAbreviacion = entity.Unit?.Abbreviation ?? entity.Product?.Unit?.Abbreviation ?? string.Empty,

            Proveedor = entity.Proveedor,
            FechaEntrada = entity.FechaEntrada,
            Cantidad = entity.Cantidad,
            Precio = entity.Precio,
            Total = entity.Total
        };
    }
}
