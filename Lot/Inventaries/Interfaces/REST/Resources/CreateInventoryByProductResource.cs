//L
namespace Lot.Inventaries.Interfaces.REST.Resources;

public class CreateInventoryByProductResource
{
    public string Categoria { get; set; }  
    public string Producto { get; set; }   
    public DateTime FechaEntrada { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public int StockMinimo { get; set; }
    public string UnidadMedida { get; set; }
}
