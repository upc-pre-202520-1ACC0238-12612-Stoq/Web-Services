namespace Lot.ProductManagement.Domain.Model.Aggregates;

/// <summary>
/// Unit of Measure Aggregate Root
/// </summary>
/// <remarks>
/// Esta clase representa el agregado raíz de Unidad de Medida.
/// Contiene las propiedades y métodos para gestionar las unidades de medida.
/// </remarks>
public partial class Unit
{
    public int Id { get; }
    public string Name { get; private set; }
    public string Abbreviation { get; private set; }

    // Navegación
    public virtual ICollection<Product> Products { get; private set; }

    public Unit()
    {
        Name = string.Empty;
        Abbreviation = string.Empty;
        Products = new List<Product>();
    }

    public Unit(string name, string abbreviation)
    {
        Name = name;
        Abbreviation = abbreviation;
        Products = new List<Product>();
    }

    public void UpdateUnit(string name, string abbreviation)
    {
        Name = name;
        Abbreviation = abbreviation;
    }
} 