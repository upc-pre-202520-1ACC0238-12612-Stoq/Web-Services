//L
namespace Lot.Inventaries.Interfaces.REST.Resources;

/// <summary>
/// Recurso para crear inventario por producto.
/// Conecta con productos existentes mediante ID.
/// </summary>
public class CreateInventoryByProductResource
{
    public int ProductoId { get; set; }           // FK → Products
    // ❌ FechaEntrada eliminada - ahora es automática
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public int StockMinimo { get; set; }
}
