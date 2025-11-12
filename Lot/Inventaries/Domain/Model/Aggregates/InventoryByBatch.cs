//L
using Lot.Inventaries.Domain.Model.ValueOjbects;
using Lot.ProductManagement.Domain.Model.Aggregates;

namespace Lot.Inventaries.Domain.Model.Aggregates;

/// <summary>
/// Entidad de inventario por lote.
/// Conectada con Products mediante FK para obtener datos de relación.
/// </summary>
public class InventoryByBatch
{
    public int Id { get; private set; }
    public int ProductoId { get; private set; }  // FK → Products
    public string Proveedor { get; private set; }
    public int UnidadId { get; private set; }    // FK → Units
    public DateTime FechaEntrada { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Precio { get; private set; }

    // Navigation properties para EF Core
    public Product? Product { get; private set; }
    public Lot.ProductManagement.Domain.Model.Aggregates.Unit? Unit { get; private set; }

    // Propiedad calculada
    public decimal Total => Precio * Cantidad;

    public InventoryByBatch() { }

    public InventoryByBatch(
        int productoId,
        string proveedor,
        int unidadId,
        Cantidad cantidad,
        Precio precio)
    {
        ProductoId = productoId;
        Proveedor = proveedor;
        UnidadId = unidadId;
        FechaEntrada = DateTime.Now; 
        Cantidad = cantidad.Value;
        Precio = precio.Value;
    }

    // Constructor con fecha manual (para compatibilidad)
    public InventoryByBatch(
        int productoId,
        string proveedor,
        int unidadId,
        DateTime fechaEntrada,
        Cantidad cantidad,
        Precio precio)
    {
        ProductoId = productoId;
        Proveedor = proveedor;
        UnidadId = unidadId;
        FechaEntrada = fechaEntrada;
        Cantidad = cantidad.Value;
        Precio = precio.Value;
    }
}