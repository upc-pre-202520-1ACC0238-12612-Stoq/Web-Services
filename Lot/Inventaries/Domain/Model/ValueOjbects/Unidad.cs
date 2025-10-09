//L
namespace Lot.Inventaries.Domain.Model.ValueOjbects;

public record Unidad
{
    public string Value { get; }

    public Unidad(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("La unidad no puede estar vacía.");
        
        Value = value.Trim();
    }
}
