namespace Lot.Sales.Domain.Model.ValueObjects;

/// <summary>
/// Value Object para representar el precio de venta unitario.
/// Garantiza validaciones de dominio para precios de venta.
/// </summary>
public record SalePrice
{
    public decimal Value { get; }

    public SalePrice(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("SalePrice must be greater than 0", nameof(value));
        Value = value;
    }

    public static implicit operator decimal(SalePrice price) => price.Value;
    public static implicit operator SalePrice(decimal value) => new SalePrice(value);
}