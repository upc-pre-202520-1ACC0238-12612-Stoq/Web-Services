//L
using Lot.Inventaries.Domain.Model.Aggregates;
using Lot.Inventaries.Interfaces.REST.Resources;

namespace Lot.Inventaries.Interfaces.REST.Transform;

/// <summary>
/// Assembler para transformar Entities a Resources enriquecidos.
/// Convierte Entity a Resource usando datos de la relación FK con Products.
/// Maneja correctamente valores null para consistencia en respuestas API.
/// </summary>
public static class InventoryByProductResourceAssembler
{
    public static InventoryByProductResource ToResourceFromEntity(InventoryByProduct entity)
    {
        // Manejo seguro de propiedades null para evitar datos inconsistentes
        var product = entity.Product;

        return new InventoryByProductResource
        {
            Id = entity.Id,
            ProductoId = entity.ProductoId,

            // Valores por defecto descriptivos cuando las relaciones no están cargadas o son null
            ProductoNombre = product?.Name ?? "Producto no disponible",
            CategoriaNombre = product?.Category?.Name ?? "Sin categoría",
            UnidadNombre = product?.Unit?.Name ?? "Sin unidad",
            UnidadAbreviacion = product?.Unit?.Abbreviation ?? "N/A",

            FechaEntrada = entity.FechaEntrada,
            Cantidad = entity.Cantidad,
            Precio = entity.Precio,
            StockMinimo = entity.StockMinimo
        };
    }
}