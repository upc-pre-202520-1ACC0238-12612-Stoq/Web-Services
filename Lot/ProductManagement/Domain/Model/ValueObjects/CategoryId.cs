namespace Lot.ProductManagement.Domain.Model.ValueObjects;

/// <summary>
/// Category Id Value Object
/// </summary>
/// <remarks>
/// Este value object representa el identificador de una categoría.
/// </remarks>
public record CategoryId
{
    public int Value { get; init; }

    public CategoryId()
    {
        Value = 0;
    }

    public CategoryId(int value)
    {
        if (value < 0)
            throw new ArgumentException("Category Id cannot be negative", nameof(value));
        Value = value;
    }

    public static implicit operator int(CategoryId categoryId) => categoryId.Value;
    public static implicit operator CategoryId(int value) => new(value);
} 