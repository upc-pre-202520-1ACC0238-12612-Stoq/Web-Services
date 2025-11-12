//L
namespace Lot.Inventaries.Interfaces.REST.Resources;

/// <summary>
/// Recurso de inventario por producto para la API REST.
/// Conecta con productos mediante FK.
/// </summary>
public class InventoryByProductResource
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;      // Cargado de Product.Name
    public string CategoriaNombre { get; set; } = string.Empty;     // Cargado de Product.Category.Name
    public string UnidadNombre { get; set; } = string.Empty;        // Cargado de Product.Unit.Name
    public string UnidadAbreviacion { get; set; } = string.Empty;   // Cargado de Product.Unit.Abbreviation

    // CAMPOS ORIGINALES
    public DateTime FechaEntrada { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public int StockMinimo { get; set; }

    // CAMPOS CALCULADOS
    public decimal Total => Precio * Cantidad;
    public bool StockBajo => Cantidad <= StockMinimo;
}