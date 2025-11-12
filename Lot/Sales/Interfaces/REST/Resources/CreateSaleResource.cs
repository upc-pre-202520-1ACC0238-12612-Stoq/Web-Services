namespace Lot.Sales.Interfaces.REST.Resources;

/// <summary>
/// Recurso para crear una nueva venta.
/// Simplificado con datos esenciales para realizar la venta.
/// Soporta ventas individuales y ventas de combos.
/// </summary>
public class CreateSaleResource
{
    // Campos para ventas normales (existentes)
    public int ProductId { get; set; }        // FK → Products
    public int Quantity { get; set; }         // Cantidad vendida
    public decimal UnitPrice { get; set; }    // Precio unitario de venta
    public string CustomerName { get; set; } = string.Empty;  // Nombre del cliente
    public string? Notes { get; set; }        // Notas opcionales

    // ⭐ NUEVOS campos para soporte de combos
    public int? ComboId { get; set; }         // FK → Combos (opcional)
    public string? ComboName { get; set; }    // Nombre del combo (solo lectura)

    /// <summary>
    /// Determina si esta venta es de un combo
    /// </summary>
    public bool IsComboSale => ComboId.HasValue;
}