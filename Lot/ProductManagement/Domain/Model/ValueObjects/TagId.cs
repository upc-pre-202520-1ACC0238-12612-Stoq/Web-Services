namespace Lot.ProductManagement.Domain.Model.ValueObjects;

/// <summary>
/// Tag Id Value Object
/// </summary>
/// <remarks>
/// Este value object representa el identificador de una etiqueta.
/// </remarks>
public record TagId
{
    public int Value { get; init; }

    public TagId()
    {
        Value = 0;
    }

    public TagId(int value)
    {
        if (value < 0)
            throw new ArgumentException("Tag Id cannot be negative", nameof(value));
        Value = value;
    }

    public static implicit operator int(TagId tagId) => tagId.Value;
    public static implicit operator TagId(int value) => new(value);
} 