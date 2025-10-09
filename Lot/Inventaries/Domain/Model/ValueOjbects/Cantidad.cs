//L

namespace Lot.Inventaries.Domain.Model.ValueOjbects;

public record Cantidad
{
    public int Value { get; }

    public Cantidad(int value)
    {
        if (value <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que cero.");
        
        Value = value;
    }
}
