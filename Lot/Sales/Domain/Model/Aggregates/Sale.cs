using Lot.Sales.Domain.Model.ValueObjects;

namespace Lot.Sales.Domain.Model.Aggregates;

/// <summary>
/// Agregado raíz que representa una venta realizada.
/// Encapsula toda la lógica de negocio de una transacción de venta.
/// Soporta ventas individuales y ventas de combos.
/// </summary>
public class Sale
{
    // Campos existentes de ventas
    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public string CategoryName { get; private set; } = string.Empty;
    public DateTime SaleDate { get; private set; }
    public SaleQuantity Quantity { get; private set; } = null!;
    public SalePrice UnitPrice { get; private set; } = null!;
    public decimal TotalAmount => Quantity.Value * UnitPrice.Value;
    public string CustomerName { get; private set; } = string.Empty;
    public string? Notes { get; private set; }

    // ⭐ NUEVOS campos para soporte de combos
    public int? ComboId { get; private set; }
    public string? ComboName { get; private set; } = string.Empty;

    /// <summary>
    /// Determina si esta venta es de un combo
    /// </summary>
    public bool IsComboSale => ComboId.HasValue;

    // Constructor por defecto para EF Core
    private Sale() { }

    /// <summary>
    /// Crea una nueva venta con validaciones de dominio
    /// </summary>
    public static Sale Create(int productId, string productName, string categoryName,
        int quantity, decimal unitPrice, string customerName, string? notes = null,
        int? comboId = null, string? comboName = null)
    {
        // Validaciones de dominio (las de quantity y unitPrice están en los Value Objects)
        if (productId <= 0)
            throw new ArgumentException("ProductId must be greater than 0", nameof(productId));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("ProductName cannot be null or empty", nameof(productName));

        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("CustomerName cannot be null or empty", nameof(customerName));

        // Los Value Objects validan quantity y unitPrice automáticamente
        var saleQuantity = new SaleQuantity(quantity);
        var salePrice = new SalePrice(unitPrice);

        return new Sale(productId, productName, categoryName, saleQuantity, salePrice, customerName, notes, comboId, comboName);
    }

    /// <summary>
    /// ⭐ NUEVO: Crea una venta de combo con datos del combo
    /// </summary>
    public static Sale CreateComboSale(int comboId, string comboName, int productId,
        string productName, string categoryName, int quantity, decimal unitPrice,
        string customerName, string? notes = null)
    {
        var fullNotes = $"Combo: {comboName}";
        if (!string.IsNullOrWhiteSpace(notes))
            fullNotes += $" - {notes}";

        return Create(
            productId: productId,
            productName: productName,
            categoryName: categoryName,
            quantity: quantity,
            unitPrice: unitPrice,
            customerName: customerName,
            notes: fullNotes,
            comboId: comboId,
            comboName: comboName
        );
    }

    // Constructor privado para uso del factory method
    private Sale(int productId, string productName, string categoryName,
        SaleQuantity quantity, SalePrice unitPrice, string customerName, string? notes,
        int? comboId = null, string? comboName = null)
    {
        ProductId = productId;
        ProductName = productName.Trim();
        CategoryName = categoryName?.Trim() ?? string.Empty;
        SaleDate = DateTime.UtcNow;
        Quantity = quantity;
        UnitPrice = unitPrice;
        CustomerName = customerName.Trim();
        Notes = notes?.Trim();

        // ⭐ NUEVOS campos para combos
        ComboId = comboId;
        ComboName = comboName?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Determina si es una venta grande (mayor a 10 unidades)
    /// </summary>
    public bool IsLargeSale() => Quantity.Value > 10;

    /// <summary>
    /// Determina si es una venta de alto valor (mayor a $100)
    /// </summary>
    public bool IsHighValueSale() => TotalAmount > 100;

    /// <summary>
    /// Obtiene el tipo de venta basado en cantidad y monto
    /// </summary>
    public string GetSaleType()
    {
        if (IsLargeSale() && IsHighValueSale()) return "Premium";
        if (IsLargeSale()) return "Bulk";
        if (IsHighValueSale()) return "High Value";
        return "Standard";
    }

    /// <summary>
    /// Valida que la venta sea consistente con el stock disponible
    /// </summary>
    public bool IsValidForStock(int availableStock) => Quantity.Value <= availableStock;
}