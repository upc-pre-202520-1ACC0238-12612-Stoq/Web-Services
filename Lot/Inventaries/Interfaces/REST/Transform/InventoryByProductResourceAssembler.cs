//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Interfaces.REST.Resources;

namespace Lot.Inventaries.Interfaces.REST.Transform;

/// <summary>
/// Assembler para transformar Entities a Resources enriquecidos.
/// Convierte Entity a Resource usando datos de la relación FK con Products.
/// </summary>
public static class InventoryByProductResourceAssembler
{
    public static InventoryByProductResource ToResourceFromEntity(InventoryByProduct entity)
    {
        return new InventoryByProductResource
        {
            Id = entity.Id,
            ProductoId = entity.ProductoId,
            ProductoNombre = entity.Product?.Name ?? string.Empty,
            CategoriaNombre = entity.Product?.Category?.Name ?? string.Empty,
            UnidadNombre = entity.Product?.Unit?.Name ?? string.Empty,
            UnidadAbreviacion = entity.Product?.Unit?.Abbreviation ?? string.Empty,

            FechaEntrada = entity.FechaEntrada,
            Cantidad = entity.Cantidad,
            Precio = entity.Precio,
            StockMinimo = entity.StockMinimo
        };
    }
}