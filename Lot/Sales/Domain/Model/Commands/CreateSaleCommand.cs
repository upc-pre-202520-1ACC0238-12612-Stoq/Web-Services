namespace Lot.Sales.Domain.Model.Commands;

/// <summary>
/// Command para crear una nueva venta con reducción automática de inventario.
/// Simplificado con solo el ID del producto y datos esenciales.
/// Soporta ventas individuales y ventas de combos.
/// </summary>
public record CreateSaleCommand(
    // Campos existentes para ventas normales
    int ProductId,
    int Quantity,
    decimal UnitPrice,
    string CustomerName,
    string? Notes = null,

    // ⭐ NUEVOS campos para soporte de combos
    int? ComboId = null,
    string? ComboName = null
);