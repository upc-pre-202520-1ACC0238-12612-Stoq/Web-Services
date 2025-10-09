//L
namespace Lot.Inventaries.Domain.Model.ValueOjbects;

public record StockMinimo
{
    public int Value { get; }

    public StockMinimo(int value)
    {
        if (value < 0)
            throw new ArgumentException("El stock mínimo no puede ser negativo.");
        
        Value = value;
    }
}
