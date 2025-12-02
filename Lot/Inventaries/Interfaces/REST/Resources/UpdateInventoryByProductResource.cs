namespace Lot.Inventaries.Interfaces.REST.Resources;

/// <summary>
/// Recurso para actualizar inventario por producto.
/// Todos los campos son opcionales para actualización parcial.
/// Sigue el patrón de CreateInventoryByProductResource.
/// </summary>
public class UpdateInventoryByProductResource
{
    /// <summary>
    /// ID del producto a asociar (opcional)
    /// </summary>
    public int? ProductoId { get; set; }

    /// <summary>
    /// Nueva cantidad (opcional)
    /// </summary>
    public int? Cantidad { get; set; }

    /// <summary>
    /// Nuevo precio (opcional)
    /// </summary>
    public decimal? Precio { get; set; }

    /// <summary>
    /// Nuevo stock mínimo (opcional)
    /// </summary>
    public int? StockMinimo { get; set; }
}