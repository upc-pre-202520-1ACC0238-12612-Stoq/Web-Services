//L
using Lot.Inventaries.Domain.Model.ValueOjbects;

namespace Lot.Inventaries.Domain.Model.Commands;

public class CreateInventoryByProductCommand
{
    public string Categoria { get; }
    public string Producto { get; }
    public DateTime FechaEntrada { get; }
    public Cantidad Cantidad { get; }
    public Precio Precio { get; }
    public StockMinimo StockMinimo { get; }
    public Unidad UnidadMedida { get; }

    public CreateInventoryByProductCommand(
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
        Cantidad = cantidad;
        Precio = precio;
        StockMinimo = stockMinimo;
        UnidadMedida = unidadMedida;
    }
}