//L
namespace Lot.Inventaries.Interfaces.REST.Resources;

/// <summary>
/// Recurso para crear inventario por lote.
/// Conecta con productos existentes mediante ID.
/// </summary>
public class CreateInventoryByBatchResource
{
    public int ProductoId { get; set; }           // FK → Products
    public string Proveedor { get; set; } = string.Empty;
    // ❌ FechaEntrada eliminada - ahora es automática
    public int UnidadId { get; set; }             // FK → Units
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
}