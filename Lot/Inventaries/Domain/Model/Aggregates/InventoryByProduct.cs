//L
using Lot.Inventaries.Domain.Model.ValueOjbects;
using Lot.ProductManagement.Domain.Model.Aggregates;

namespace Lot.Inventaries.Domain.Model.Aggregates;

/// <summary>
/// Entidad de inventario por producto.
/// Conectada con Products mediante FK para obtener datos de relación.
/// </summary>
public class InventoryByProduct
{
    public int Id { get; private set; }
    public int ProductoId { get; private set; }  // FK → Products
    public DateTime FechaEntrada { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Precio { get; private set; }
    public int StockMinimo { get; private set; }

    // Navigation property para EF Core - carga relaciones con Product
    public Product? Product { get; private set; }

    public InventoryByProduct() { }

    public InventoryByProduct(
        int productoId,
        Cantidad cantidad,
        Precio precio,
        StockMinimo stockMinimo)
    {
        ProductoId = productoId;
        FechaEntrada = DateTime.Now; // ✅ Fecha automática en hora local
        Cantidad = cantidad.Value;
        Precio = precio.Value;
        StockMinimo = stockMinimo.Value;
    }

    // Constructor con fecha manual (para compatibilidad)
    public InventoryByProduct(
        int productoId,
        DateTime fechaEntrada,
        Cantidad cantidad,
        Precio precio,
        StockMinimo stockMinimo)
    {
        ProductoId = productoId;
        FechaEntrada = fechaEntrada;
        Cantidad = cantidad.Value;
        Precio = precio.Value;
        StockMinimo = stockMinimo.Value;
    }

    // Helper: Verificar si hay stock bajo
    public bool StockBajo => Cantidad <= StockMinimo;

    /// <summary>
    /// Reduce el stock disponible validando que no quede negativo
    /// </summary>
    /// <param name="quantity">Cantidad a reducir</param>
    /// <exception cref="ArgumentException">Lanzada cuando no hay stock suficiente</exception>
    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to reduce must be greater than 0", nameof(quantity));

        if (Cantidad < quantity)
            throw new ArgumentException($"Insufficient stock. Available: {Cantidad}, Requested: {quantity}", nameof(quantity));

        Cantidad -= quantity;
    }

    /// <summary>
    /// Aumenta el stock disponible
    /// </summary>
    /// <param name="quantity">Cantidad a aumentar</param>
    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to increase must be greater than 0", nameof(quantity));

        Cantidad += quantity;
    }
}
