//L
using Lot.Inventaries.Domain.Model.ValueOjbects;

namespace Lot.Inventaries.Domain.Model.Commands;

/// <summary>
/// Command para crear inventario por producto.
/// Conecta con Products mediante foreign key (ProductoId).
/// </summary>
public class CreateInventoryByProductCommand
{
    public int ProductoId { get; }           // FK → Products
    // ❌ FechaEntrada eliminada - ahora es automática
    public Cantidad Cantidad { get; }
    public Precio Precio { get; }
    public StockMinimo StockMinimo { get; }

    public CreateInventoryByProductCommand(
        int productoId,
        Cantidad cantidad,
        Precio precio,
        StockMinimo stockMinimo)
    {
        ProductoId = productoId;
        Cantidad = cantidad;
        Precio = precio;
        StockMinimo = stockMinimo;
    }
}