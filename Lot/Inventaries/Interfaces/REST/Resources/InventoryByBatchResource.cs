//L
namespace Lot.Inventaries.Interfaces.REST.Resources;

public class InventoryByBatchResource
{
    public int Id { get; set; }
    public string Proveedor { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
    public DateTime FechaEntrada { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public decimal Total { get; set; }
}
