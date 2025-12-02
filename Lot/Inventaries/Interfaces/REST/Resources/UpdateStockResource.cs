namespace Lot.Inventaries.Interfaces.REST.Resources;

/// <summary>
/// Recurso específico para actualización parcial de stock y precios.
/// Los campos son opcionales para permitir actualizaciones parciales via PATCH.
/// </summary>
public class UpdateStockResource
{
    /// <summary>
    /// Nueva cantidad de stock (opcional)
    /// </summary>
    public int? Cantidad { get; set; }

    /// <summary>
    /// Nuevo precio unitario (opcional)
    /// </summary>
    public decimal? Precio { get; set; }

    /// <summary>
    /// Nuevo stock mínimo (opcional)
    /// </summary>
    public int? StockMinimo { get; set; }
}