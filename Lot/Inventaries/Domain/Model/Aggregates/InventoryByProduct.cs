//L
using Lot.Inventaries.Domain.Model.ValueOjbects;

namespace Lot.Inventaries.Domain.Model.Aggregates;

public class InventoryByProduct
{
    public int Id { get; private set; }
    public string Categoria { get; private set; }          
    public string Producto { get; private set; }         
    public DateTime FechaEntrada { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Precio { get; private set; }
    public int StockMinimo { get; private set; }
    public string UnidadMedida { get; private set; }

    public InventoryByProduct() { }

    public InventoryByProduct(
        string categoria,
        string producto,
        DateTime fechaEntrada,
        Cantidad cantidad,
        Precio precio,
        StockMinimo stockMinimo,
        Unidad unidadMedida)
    {
        Categoria = categoria;
        Producto = producto;
        FechaEntrada = fechaEntrada;
        Cantidad = cantidad.Value;
        Precio = precio.Value;
        StockMinimo = stockMinimo.Value;
        UnidadMedida = unidadMedida.Value;
    }
}
