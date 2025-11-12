//L
using Lot.Inventaries.Domain.Model.ValueOjbects;

namespace Lot.Inventaries.Domain.Model.Commands;

/// <summary>
/// Command para crear inventario por lote.
/// Conecta con Products mediante foreign key (ProductoId).
/// </summary>
public class CreateInventoryByBatchCommand
{
    public int ProductoId { get; }           // FK → Products
    public string Proveedor { get; }
    public int UnidadId { get; }             // FK → Units
    // ❌ FechaEntrada eliminada - ahora es automática
    public Cantidad Cantidad { get; }
    public Precio Precio { get; }

    public CreateInventoryByBatchCommand(
        int productoId,
        string proveedor,
        int unidadId,
        Cantidad cantidad,
        Precio precio)
    {
        ProductoId = productoId;
        Proveedor = proveedor;
        UnidadId = unidadId;
        Cantidad = cantidad;
        Precio = precio;
    }
}