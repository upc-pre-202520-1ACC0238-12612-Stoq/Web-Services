//L
using Lot.Inventaries.Domain.Model.ValueOjbects;

namespace Lot.Inventaries.Domain.Model.Aggregates;

public class InventoryByBatch
{
    public int Id { get; private set; }
    public string Proveedor { get; private set; }
    public string Producto { get; private set; }
    public DateTime FechaEntrada { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Precio { get; private set; }
    public string Unidad { get; private set; }

    public decimal Total => Precio * Cantidad;

    public InventoryByBatch()
    {
        Proveedor = string.Empty;
        Producto = string.Empty;
        Unidad = string.Empty;
    }

    public InventoryByBatch(
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
        Cantidad = cantidad.Value;
        Precio = precio.Value;
        Unidad = unidad.Value;
    }
}