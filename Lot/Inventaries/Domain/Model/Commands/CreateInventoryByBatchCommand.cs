//L
using Lot.Inventaries.Domain.Model.ValueOjbects;

namespace Lot.Inventaries.Domain.Model.Commands;

public class CreateInventoryByBatchCommand
{
    public string Proveedor { get; }
    public string Producto { get; }
    public DateTime FechaEntrada { get; }
    public Cantidad Cantidad { get; }
    public Precio Precio { get; }
    public Unidad Unidad { get; }

    public CreateInventoryByBatchCommand(
        string proveedor,
        string producto,
        DateTime fechaEntrada,
        Cantidad cantidad,
        Precio precio,
        Unidad unidad)
    {
        Proveedor = proveedor;
        Producto = producto;
        FechaEntrada = fechaEntrada;
        Cantidad = cantidad;
        Precio = precio;
        Unidad = unidad;
    }
}