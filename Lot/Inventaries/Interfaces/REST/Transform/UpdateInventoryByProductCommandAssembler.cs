using Lot.Inventaries.Domain.Model.Commands;
using Lot.Inventaries.Domain.Model.ValueOjbects;
using Lot.Inventaries.Interfaces.REST.Resources;

namespace Lot.Inventaries.Interfaces.REST.Transform;

/// <summary>
/// Assembler para transformar Resources a Commands de actualización.
/// Realiza validaciones de negocio antes de crear ValueObjects.
/// </summary>
public static class UpdateInventoryByProductCommandAssembler
{
    public static UpdateInventoryByProductCommand ToCommandFromResource(int id, UpdateInventoryByProductResource resource)
    {
        // Validar valores antes de crear ValueObjects para mejor manejo de errores
        Cantidad? cantidad = null;
        Precio? precio = null;
        StockMinimo? stockMinimo = null;

        if (resource.Cantidad.HasValue)
        {
            if (resource.Cantidad.Value <= 0)
                throw new ArgumentException("La cantidad debe ser mayor que cero.");
            cantidad = new Cantidad(resource.Cantidad.Value);
        }

        if (resource.Precio.HasValue)
        {
            if (resource.Precio.Value < 0)
                throw new ArgumentException("El precio no puede ser negativo.");
            precio = new Precio(resource.Precio.Value);
        }

        if (resource.StockMinimo.HasValue)
        {
            if (resource.StockMinimo.Value < 0)
                throw new ArgumentException("El stock mínimo no puede ser negativo.");
            stockMinimo = new StockMinimo(resource.StockMinimo.Value);
        }

        return new UpdateInventoryByProductCommand(
            id: id,
            productoId: resource.ProductoId,
            cantidad: cantidad,
            precio: precio,
            stockMinimo: stockMinimo
        );
    }
}