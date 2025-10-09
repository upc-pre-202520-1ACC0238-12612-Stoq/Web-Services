namespace Lot.ProductManagement.Domain.Model.ValueObjects;

/// <summary>
/// Product Id Value Object
/// </summary>
/// <remarks>
/// Este value object representa el identificador de un producto.
/// </remarks>
public record ProductId
{
    public int Value { get; init; }

    public ProductId()
    {
        Value = 0;
    }

    public ProductId(int value)
    {
        if (value < 0)
            throw new ArgumentException("Product Id cannot be negative", nameof(value));
        Value = value;
    }

    public static implicit operator int(ProductId productId) => productId.Value;
    public static implicit operator ProductId(int value) => new(value);
} 