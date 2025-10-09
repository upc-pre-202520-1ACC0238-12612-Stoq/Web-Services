namespace Lot.ProductManagement.Domain.Model.ValueObjects;

/// <summary>
/// Unit Id Value Object
/// </summary>
/// <remarks>
/// Este value object representa el identificador de una unidad de medida.
/// </remarks>
public record UnitId
{
    public int Value { get; init; }

    public UnitId()
    {
        Value = 0;
    }

    public UnitId(int value)
    {
        if (value < 0)
            throw new ArgumentException("Unit Id cannot be negative", nameof(value));
        Value = value;
    }

    public static implicit operator int(UnitId unitId) => unitId.Value;
    public static implicit operator UnitId(int value) => new(value);
} 