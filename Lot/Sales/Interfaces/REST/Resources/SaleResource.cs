namespace Lot.Sales.Interfaces.REST.Resources;

/// <summary>
/// Recurso de venta para la API REST con datos enriquecidos.
/// Incluye información del producto, combos y cálculos automáticos.
/// Soporta ventas individuales y ventas de combos.
/// </summary>
public class SaleResource
{
    // Campos existentes de ventas
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string SaleType { get; set; } = string.Empty;  // Calculado: Standard, Bulk, High Value, Premium

    // ⭐ NUEVOS campos para soporte de combos
    public int? ComboId { get; set; }             // ID del combo si aplica
    public string? ComboName { get; set; }        // Nombre del combo si aplica
    public bool IsComboSale { get; set; }         // Verdadero si es venta de combo
}