//L
using Lot.Inventaries.Domain.Model.ValueOjbects;

namespace Lot.Inventaries.Domain.Model.Commands;

/// <summary>
/// Command para actualizar inventario por producto.
/// Sigue el patrón de UpdateBranchCommand existente.
/// </summary>
public class UpdateInventoryByProductCommand
{
    public int Id { get; }

    // Propiedades opcionales para actualización parcial
    // Mismo patrón que UpdateBranchCommand
    public int? ProductoId { get; }
    public Cantidad? Cantidad { get; }
    public Precio? Precio { get; }
    public StockMinimo? StockMinimo { get; }

    public UpdateInventoryByProductCommand(
        int id,
        int? productoId = null,
        Cantidad? cantidad = null,
        Precio? precio = null,
        StockMinimo? stockMinimo = null)
    {
        Id = id;
        ProductoId = productoId;
        Cantidad = cantidad;
        Precio = precio;
        StockMinimo = stockMinimo;
    }
}