namespace Lot.Sales.Domain.Model.ValueObjects;

/// <summary>
/// Value Object para representar la cantidad vendida en una transacción.
/// Garantiza validaciones de dominio para cantidades de venta.
/// </summary>
public record SaleQuantity
{
    public int Value { get; }

    public SaleQuantity(int value)
    {
        if (value <= 0)
            throw new ArgumentException("SaleQuantity must be greater than 0", nameof(value));
        Value = value;
    }

    public static implicit operator int(SaleQuantity quantity) => quantity.Value;
    public static implicit operator SaleQuantity(int value) => new SaleQuantity(value);
}