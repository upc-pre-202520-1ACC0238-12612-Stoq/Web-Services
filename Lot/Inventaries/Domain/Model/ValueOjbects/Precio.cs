//L

namespace Lot.Inventaries.Domain.Model.ValueOjbects;

public record Precio
{
    public decimal Value { get; }

    public Precio(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El precio no puede ser negativo.");
        
        Value = value;
    }
}
