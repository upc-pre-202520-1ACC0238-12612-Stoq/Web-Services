//L
namespace Lot.Inventaries.Interfaces.REST.Resources;

/// <summary>
/// Recurso de inventario por lote para la API REST.
/// Conecta con productos mediante FK.
/// </summary>
public class InventoryByBatchResource
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;      // Cargado de Product.Name
    public string UnidadNombre { get; set; } = string.Empty;        // Cargado de Unit.Name (específico del lote)
    public string UnidadAbreviacion { get; set; } = string.Empty;   // Cargado de Unit.Abbreviation (específico del lote)

    public string Proveedor { get; set; } = string.Empty;
    public DateTime FechaEntrada { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public decimal Total { get; set; }
}
