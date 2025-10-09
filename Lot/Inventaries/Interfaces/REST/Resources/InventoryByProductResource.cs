//L
namespace Lot.Inventaries.Interfaces.REST.Resources;

public class InventoryByProductResource
{
    public int Id { get; set; }
    public string Categoria { get; set; }
    public string Producto { get; set; }
    public DateTime FechaEntrada { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public int StockMinimo { get; set; }
    public string UnidadMedida { get; set; } = string.Empty;

    public decimal Total => Precio * Cantidad;
}