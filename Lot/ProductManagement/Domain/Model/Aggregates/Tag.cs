namespace Lot.ProductManagement.Domain.Model.Aggregates;

/// <summary>
/// Tag Aggregate Root
/// </summary>
/// <remarks>
/// Esta clase representa el agregado raíz de Etiqueta.
/// Contiene las propiedades y métodos para gestionar las etiquetas de productos.
/// </remarks>
public partial class Tag
{
    public int Id { get; }
    public string Name { get; private set; }

    // Navegación
    public virtual ICollection<ProductTag> ProductTags { get; private set; }

    public Tag()
    {
        Name = string.Empty;
        ProductTags = new List<ProductTag>();
    }

    public Tag(string name)
    {
        Name = name;
        ProductTags = new List<ProductTag>();
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
} 